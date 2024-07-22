// -----------------------------------------------------------------------
// <copyright file="AudioPlayer.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.API.Features.Audio
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Exiled.API.Features.Attributes;
    using Exiled.API.Features.Audio.EventArgs;
    using Exiled.API.Features.DynamicEvents;
    using MEC;
    using Mirror;
    using NVorbis;
    using UnityEngine;
    using VoiceChat;
    using VoiceChat.Codec;
    using VoiceChat.Codec.Enums;
    using VoiceChat.Networking;

    /// <summary>
    /// Represents a NPC that can play audios.
    /// </summary>
    public class AudioPlayer : MonoBehaviour
    {
        private readonly Queue<float> streamBuffer = new();

        private CoroutineHandle playbackHandler;

        private MemoryStream memoryStream;

        private VorbisReader vorbisReader;

        private float allowedSamples;

        private int samplesPerSecond;

        private float[] sendBuffer;

        private float[] readBuffer;

        /// <summary>
        /// Gets all the audio players present in the server in the form of dictionary.
        /// </summary>
        public static Dictionary<Npc, AudioPlayer> Dictionary { get; } = new();

        /// <summary>
        /// Gets all the audio players present in the server in the form of list.
        /// </summary>
        public static IEnumerable<AudioPlayer> List => Dictionary.Values.ToList();

        /// <summary>
        /// Gets or sets the <see cref="TDynamicEventDispatcher{T}"/> which handles all delegates to be fired before selecting an audio file.
        /// </summary>
        [DynamicEventDispatcher]
        public static TDynamicEventDispatcher<SelectingAudioEventArgs> SelectingAudio { get; set; } = new();

        /// <summary>
        /// Gets or sets the <see cref="TDynamicEventDispatcher{T}"/> which handles all delegates to be fired after selecting an audio file.
        /// </summary>
        [DynamicEventDispatcher]
        public static TDynamicEventDispatcher<AudioSelectedEventArgs> AudioSelected { get; set; } = new();

        /// <summary>
        /// Gets or sets the <see cref="TDynamicEventDispatcher{T}"/> which handlers all delegates to be fired before starting to play an audio file.
        /// </summary>
        [DynamicEventDispatcher]
        public static TDynamicEventDispatcher<StartPlayingAudioEventArgs> StartPlaying { get; set; } = new();

        /// <summary>
        /// Gets or sets the <see cref="TDynamicEventDispatcher{T}"/> which handlers all delegates to be fired after finishing and audio file.
        /// </summary>
        [DynamicEventDispatcher]
        public static TDynamicEventDispatcher<AudioFinishedEventArgs> AudioFinished { get; set; } = new();

        /// <summary>
        /// Gets the NPC linked to this <see cref="AudioPlayer"/>.
        /// </summary>
        public Npc Owner { get; private set; }

        /// <summary>
        /// Gets the current audio queue of the Audio player instance.
        /// </summary>
        public List<AudioFile> AudioQueue { get; } = new();

        /// <summary>
        /// Gets the audio file that is being player.
        /// </summary>
        public AudioFile CurrentAudio { get; private set; }

        /// <summary>
        /// Gets the type of the audio player (If the audio player is playing audios from files it will be File, if the audio player is inactive, it will be None).
        /// </summary>
        public AudioPlayerType PlayerType { get; private set; } = AudioPlayerType.None;

        /// <summary>
        /// Gets or sets a value indicating whether the audio player instance should be destroyed when finishing the current audio.
        /// </summary>
        public bool DestroyWhenFinishing { get; set; } = false;

        private bool Finished { get; set; }

        private bool ShouldStop { get; set; }

        private bool ShouldPlay { get; set; }

        private PlaybackBuffer PlaybackBuffer { get; } = new();

        private OpusEncoder Encoder { get; } = new(OpusApplicationType.Voip);

        private byte[] EncodedBytes { get; } = new byte[512];

        /// <summary>
        /// Gets the audio player of the specified npc or creates one for it.
        /// </summary>
        /// <param name="npc">The npc to get the audio player instance of or create one.</param>
        /// <returns>The Audio Player instance of the npc.</returns>
        public static AudioPlayer GetOrCreate(Npc npc)
        {
            if (Dictionary.TryGetValue(npc, out AudioPlayer player))
                return player;

            player = npc.GameObject.AddComponent<AudioPlayer>();
            player.Owner = npc;

            Dictionary.Add(npc, player);

            return player;
        }

        /// <summary>
        /// Checks if the audio file is .ogg.
        /// </summary>
        /// <param name="audioFile">The <see cref="AudioFile"/>to check.</param>
        /// <returns>If the audio file is .ogg or not.</returns>
        public bool IsOggFile(AudioFile audioFile)
        {
            if (!File.Exists(audioFile.FilePath))
                return false;

            return Path.GetExtension(audioFile.FilePath) == ".ogg";
        }

        /// <summary>
        /// Checks if the audio file is .ogg.
        /// </summary>
        /// <param name="audioFile">The Audio File to check.</param>
        /// <returns>If the audio file is .ogg or not.</returns>
        public bool IsOggFile(string audioFile)
        {
            if (!File.Exists(audioFile))
                return false;

            return Path.GetExtension(audioFile) == ".ogg";
        }

        /// <summary>
        /// Enqueue an audio file.
        /// </summary>
        /// <param name="audioFile">The audio file to enqueue.</param>
        /// <param name="queuePosition">The position to insert the audio in the queue.</param>
        /// <remarks>If the queue position is not specified, the audio file will be inserted at the end of the queue.</remarks>
        public void Enqueue(AudioFile audioFile, int queuePosition = -1)
        {
            if (audioFile == null)
                return;

            if (!audioFile.IsEnabled || !IsOggFile(audioFile))
                return;

            if (queuePosition == -1)
                AudioQueue.Add(audioFile);
            else
                AudioQueue.Insert(queuePosition, audioFile);
        }

        /// <summary>
        /// Plays a specific audio from the queue.
        /// </summary>
        /// <param name="queuePosition">The position of the audio in the queue.</param>
        /// <remarks>If the queue position is not specified, the first audio in the queue will be played.</remarks>
        public void Play(int queuePosition = 0)
        {
            if (AudioQueue.Count == 0)
                return;

            CurrentAudio = queuePosition == 0 ? AudioQueue.First() : AudioQueue.ElementAt(queuePosition);

            SelectingAudioEventArgs selectingAudioEventArgs = new(this, CurrentAudio, true);
            SelectingAudio.InvokeAll(selectingAudioEventArgs);

            if (!selectingAudioEventArgs.IsAllowed)
            {
                AudioQueue.Remove(CurrentAudio);
                CurrentAudio = null;
                return;
            }

            if (playbackHandler.IsRunning)
                return;

            AudioQueue.Remove(CurrentAudio);

            AudioSelectedEventArgs audioSelectedEventArgs = new(this, CurrentAudio);
            AudioSelected.InvokeAll(audioSelectedEventArgs);

            PlayerType = AudioPlayerType.File;

            playbackHandler = Timing.RunCoroutine(AudioPlayback(CurrentAudio));
        }

        /// <summary>
        /// Stops the audio that is being played.
        /// </summary>
        /// <param name="clearQueue">If the queue should be cleared or not.</param>
        public void Stop(bool clearQueue = false)
        {
            if (clearQueue)
                AudioQueue.Clear();

            ShouldStop = true;
        }

        /// <summary>
        /// Destroys the audio instance and the npc.
        /// </summary>
        public void Destroy()
        {
            if (playbackHandler.IsRunning)
                Timing.KillCoroutines(playbackHandler);

            Dictionary.Remove(Owner);

            NetworkServer.RemovePlayerForConnection(Owner.Connection, true);
        }

        private bool IsMonoFile()
        {
            if (vorbisReader.Channels < 2)
                return true;

            vorbisReader.Dispose();
            memoryStream.Dispose();

            return false;
        }

        private bool CheckSampleRate()
        {
            if (vorbisReader.SampleRate is 48000)
                return true;

            vorbisReader.Dispose();
            memoryStream.Dispose();

            return false;
        }

        private IEnumerator<float> AudioPlayback(AudioFile audioFile)
        {
            ShouldStop = false;
            Finished = false;

            if (audioFile.IsLooping)
                AudioQueue.Add(audioFile);

            Log.Debug("Starting Audio Playback");

            memoryStream = new MemoryStream(File.ReadAllBytes(audioFile.FilePath));
            memoryStream.Seek(0, SeekOrigin.Begin);

            vorbisReader = new VorbisReader(memoryStream);

            if (!IsMonoFile())
            {
                Log.Error("The Current audio file is not valid, audio files must be Mono to be played");

                if (AudioQueue.Count >= 1)
                    Play(0);

                yield break;
            }

            if (!CheckSampleRate())
            {
                Log.Error("The Current audio file is not valid, audio files must have a samplerate of 48000 Hz");

                if (AudioQueue.Count >= 1)
                    Play(0);

                yield break;
            }

            StartPlayingAudioEventArgs startPlayingAudioEventArgs = new(this, audioFile, true);
            StartPlaying.InvokeAll(startPlayingAudioEventArgs);

            if (!startPlayingAudioEventArgs.IsAllowed)
            {
                if (AudioQueue.Count >= 1)
                    Play(0);

                yield break;
            }

            Log.Debug($"Playing {audioFile.FilePath}");

            samplesPerSecond = VoiceChatSettings.SampleRate * VoiceChatSettings.Channels;
            sendBuffer = new float[(samplesPerSecond / 5) + 1920];
            readBuffer = new float[(samplesPerSecond / 5) + 1920];

            while (vorbisReader.ReadSamples(readBuffer, 0, readBuffer.Length) > 0)
            {
                if (ShouldStop)
                {
                    vorbisReader.SeekTo(vorbisReader.TotalSamples - 1);
                    ShouldStop = false;
                }

                while (streamBuffer.Count >= readBuffer.Length)
                {
                    ShouldPlay = true;
                    yield return Timing.WaitForOneFrame;
                }

                foreach (float t in readBuffer)
                    streamBuffer.Enqueue(t);
            }

            Log.Debug("Audio Finished");

            if (!DestroyWhenFinishing && audioFile.IsLooping)
            {
                Play(-1);
                yield break;
            }

            if (!DestroyWhenFinishing && AudioQueue.Count >= 1)
            {
                Finished = true;
                Play();
            }

            Finished = true;

            AudioFinishedEventArgs audioFinishedEventArgs = new(this, audioFile);
            AudioFinished.InvokeAll(audioFinishedEventArgs);

            CurrentAudio = null;
            PlayerType = AudioPlayerType.None;

            if (DestroyWhenFinishing)
                Destroy();
        }

        private void Update()
        {
            if (Owner is null || streamBuffer.Count is 0 || !ShouldPlay)
                return;

            allowedSamples += Time.deltaTime * samplesPerSecond;
            int samplesToCopy = Mathf.Min(Mathf.FloorToInt(allowedSamples), streamBuffer.Count);

            if (samplesToCopy > 0)
            {
                for (int i = 0; i < samplesToCopy; i++)
                {
                    PlaybackBuffer.Write(streamBuffer.Dequeue() * (CurrentAudio.Volume / 100f));
                }
            }

            allowedSamples -= samplesToCopy;

            while (PlaybackBuffer.Length >= 480)
            {
                PlaybackBuffer.ReadTo(sendBuffer, 480L, 0L);
                int encodedData = Encoder.Encode(sendBuffer, EncodedBytes, 480);

                foreach (Player player in Player.List)
                {
                    if (player.Connection is null || !player.IsVerified)
                        continue;

                    player.Connection.Send(new VoiceMessage(Owner.ReferenceHub, CurrentAudio.Channel, EncodedBytes, encodedData, false));
                }
            }
        }
    }
}