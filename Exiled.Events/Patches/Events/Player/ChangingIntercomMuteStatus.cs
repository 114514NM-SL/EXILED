// -----------------------------------------------------------------------
// <copyright file="ChangingIntercomMuteStatus.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Patches.Events.Player
{
#pragma warning disable SA1118
    using System;
    using System.Collections.Generic;
    using System.Reflection.Emit;

    using Exiled.API.Features;
    using Exiled.Events.EventArgs;

    using HarmonyLib;

    using NorthwoodLib.Pools;

    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patch the <see cref="CharacterClassManager.NetworkIntercomMuted"/>.
    /// Adds the <see cref="Handlers.Player.ChangingIntercomMuteStatus"/> event.
    /// </summary>
    [HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.NetworkIntercomMuted), MethodType.Setter)]
    internal static class ChangingIntercomMuteStatus
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            // Define the return label.
            var returnLabel = generator.DefineLabel();

            // Define the continue label and add it.
            // Used if IsAllowed is true.
            var continueLabel = generator.DefineLabel();
            newInstructions[0].WithLabels(continueLabel);

            // Define the issue mute label.
            // Used if IsAllowed is false to re-issue a mute.
            var issueMuteLabel = generator.DefineLabel();

            newInstructions.InsertRange(0, new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, Field(typeof(CharacterClassManager), nameof(CharacterClassManager._hub))),
                new CodeInstruction(OpCodes.Call, Method(typeof(Player), nameof(Player.Get), new[] { typeof(ReferenceHub) })),
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Ldc_I4_1),
                new CodeInstruction(OpCodes.Newobj, GetDeclaredConstructors(typeof(ChangingIntercomMuteStatusEventArgs))[0]),
                new CodeInstruction(OpCodes.Dup),
                new CodeInstruction(OpCodes.Call, Method(typeof(Handlers.Player), nameof(Handlers.Player.OnChangingIntercomMuteStatus))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(ChangingIntercomMuteStatusEventArgs), nameof(ChangingIntercomMuteStatusEventArgs.IsAllowed))),
                new CodeInstruction(OpCodes.Brtrue_S, continueLabel),
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Brfalse_S, issueMuteLabel),
                new CodeInstruction(OpCodes.Ldstr, "ICOM-"),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(CharacterClassManager), nameof(CharacterClassManager.UserId))),
                new CodeInstruction(OpCodes.Call, Method(typeof(string), nameof(string.Concat), new[] { typeof(string), typeof(string) })),
                new CodeInstruction(OpCodes.Call, Method(typeof(MuteHandler), nameof(MuteHandler.RevokePersistentMute))),
                new CodeInstruction(OpCodes.Ret),
                new CodeInstruction(OpCodes.Ldstr, "ICOM-").WithLabels(issueMuteLabel),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(CharacterClassManager), nameof(CharacterClassManager.UserId))),
                new CodeInstruction(OpCodes.Call, Method(typeof(string), nameof(string.Concat), new[] { typeof(string), typeof(string) })),
                new CodeInstruction(OpCodes.Call, Method(typeof(MuteHandler), nameof(MuteHandler.IssuePersistentMute))),
                new CodeInstruction(OpCodes.Ret),
            });

            // Add the return label.
            newInstructions[newInstructions.Count - 1].WithLabels(returnLabel);

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}
