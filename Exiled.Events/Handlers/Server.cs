// -----------------------------------------------------------------------
// <copyright file="Server.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Handlers
{
    using Exiled.Events.EventArgs;
    using Exiled.Events.Features;
    using Exiled.Events.EventArgs.Player;
    using Exiled.Events.EventArgs.Server;
    using Exiled.Events.Extensions;

    using static Exiled.Events.Events;

    /// <summary>
    /// Server related events.
    /// </summary>
    public static class Server
    {
        /// <summary>
        /// Gets or sets the event invoked before waiting for players.
        /// </summary>
        public static Event WaitingForPlayers { get; set; } = new();

        /// <summary>
        /// Gets or sets the event invoked after the start of a new round.
        /// </summary>
        public static Event RoundStarted { get; set; } = new();

        /// <summary>
        /// Gets or sets the event invoked before ending a round.
        /// </summary>
        public static Event<EndingRoundEventArgs> EndingRound { get; set; } = new();

        /// <summary>
        /// Gets or sets the event invoked after the end of a round.
        /// </summary>
        public static Event<RoundEndedEventArgs> RoundEnded { get; set; } = new();

        /// <summary>
        /// Gets or sets the event invoked before the restart of a round.
        /// </summary>
        public static Event RestartingRound { get; set; } = new();

        /// <summary>
        /// Gets or sets the event invoked when a player reports a cheater.
        /// </summary>
        public static Event<ReportingCheaterEventArgs> ReportingCheater { get; set; } = new();

        /// <summary>
        /// Gets or sets the event invoked before respawning a wave of Chaos Insurgency or NTF.
        /// </summary>
        public static Event<RespawningTeamEventArgs> RespawningTeam { get; set; } = new();

        /// <summary>
        /// Gets or sets the event invoked before adding an unit name.
        /// </summary>
        public static Event<AddingUnitNameEventArgs> AddingUnitName { get; set; } = new();

        /// <summary>
        /// Gets or sets the event invoked when sending a complaint about a player to the local server administrators.
        /// </summary>
        public static Event<LocalReportingEventArgs> LocalReporting { get; set; } = new();

        /// <summary>
        /// Gets or sets the event invoked after the "reload configs" command is ran.
        /// </summary>
        public static Event ReloadedConfigs { get; set; } = new();

        /// <summary>
        /// Gets or sets the event invoked after the "reload translations" command is ran.
        /// </summary>
        public static Event ReloadedTranslations { get; set; } = new();

        /// <summary>
        /// Gets or sets the event invoked after the "reload gameplay" command is ran.
        /// </summary>
        public static Event ReloadedGameplay { get; set; } = new();

        /// <summary>
        /// Gets or sets the event invoked after the "reload remoteadminconfigs" command is ran.
        /// </summary>
        public static Event ReloadedRA { get; set; } = new();

        /// <summary>
        /// Called before waiting for players.
        /// </summary>
        public static void OnWaitingForPlayers() => WaitingForPlayers.InvokeSafely();

        /// <summary>
        /// Called after the start of a new round.
        /// </summary>
        public static void OnRoundStarted() => RoundStarted.InvokeSafely();

        /// <summary>
        /// Called before ending a round.
        /// </summary>
        /// <param name="ev">The <see cref="EndingRoundEventArgs"/> instance.</param>
        public static void OnEndingRound(EndingRoundEventArgs ev) => EndingRound.InvokeSafely(ev);

        /// <summary>
        /// Called after the end of a round.
        /// </summary>
        /// <param name="ev">The <see cref="RoundEndedEventArgs"/> instance.</param>
        public static void OnRoundEnded(RoundEndedEventArgs ev) => RoundEnded.InvokeSafely(ev);

        /// <summary>
        /// Called before restarting a round.
        /// </summary>
        public static void OnRestartingRound() => RestartingRound.InvokeSafely();

        /// <summary>
        /// Called when a player reports a cheater.
        /// </summary>
        /// <param name="ev">The <see cref="ReportingCheaterEventArgs"/> instance.</param>
        public static void OnReportingCheater(ReportingCheaterEventArgs ev) => ReportingCheater.InvokeSafely(ev);

        /// <summary>
        /// Called before respawning a wave of Chaos Insurgency or NTF.
        /// </summary>
        /// <param name="ev">The <see cref="RespawningTeamEventArgs"/> instance.</param>
        public static void OnRespawningTeam(RespawningTeamEventArgs ev) => RespawningTeam.InvokeSafely(ev);

        /// <summary>
        /// Called before adding an unit name.
        /// </summary>
        /// <param name="ev">The <see cref="AddingUnitNameEventArgs"/> instance.</param>
        public static void OnAddingUnitName(AddingUnitNameEventArgs ev) => AddingUnitName.InvokeSafely(ev);

        /// <summary>
        /// Called when sending a complaint about a player to the local server administrators.
        /// </summary>
        /// <param name="ev">The <see cref="LocalReportingEventArgs"/> instance.</param>
        public static void OnLocalReporting(LocalReportingEventArgs ev) => LocalReporting.InvokeSafely(ev);

        /// <summary>
        /// Called after the "reload configs" command is ran.
        /// </summary>
        public static void OnReloadedConfigs() => ReloadedConfigs.InvokeSafely();

        /// <summary>
        /// Called after the "reload translations" command is ran.
        /// </summary>
        public static void OnReloadedTranslations() => ReloadedTranslations.InvokeSafely();

        /// <summary>
        /// Called after the "reload gameplay" command is ran.
        /// </summary>
        public static void OnReloadedGameplay() => ReloadedGameplay.InvokeSafely();

        /// <summary>
        /// Called after the "reload remoteadminconfigs" command is ran.
        /// </summary>
        public static void OnReloadedRA() => ReloadedRA.InvokeSafely();
    }
}
