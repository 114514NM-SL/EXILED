// -----------------------------------------------------------------------
// <copyright file="ChangingMicroHIDState.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Patches.Events.Player
{
#pragma warning disable SA1118
    using System.Collections.Generic;
    using System.Reflection.Emit;

    using Exiled.Events.EventArgs;

    using HarmonyLib;

    using InventorySystem.Items.MicroHID;

    using NorthwoodLib.Pools;

    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="MicroHIDItem.ServerSendStatus(HidStatusMessageType, byte)"/>.
    /// Adds the <see cref="Handlers.Player.OnChangingMicroHIDState"/> event.
    /// </summary>
    [HarmonyPatch(typeof(MicroHIDItem), nameof(MicroHIDItem.ServerSendStatus))]
    internal static class ChangingMicroHIDState
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            var returnLabel = generator.DefineLabel();

            var continueLabel = generator.DefineLabel();

            var ev = generator.DeclareLocal(typeof(ChangingMicroHIDStateEventArgs));

            // if (msgType == HidStatusMessageType.State) {...
            //
            // var ev = new ChangingMicroHIDState(Player, this, HidState, HidState, true);
            //
            // Handlers.Player.OnChangingMicroHIDState(ev);
            //
            // if (!ev.IsAllowed)
            //     return;
            //
            // code = ev.NewState;
            // ...}
            //
            // else { Original Code }
            newInstructions.InsertRange(0, new[]
            {
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Ldc_I4_1),
                new CodeInstruction(OpCodes.Bne_Un_S, continueLabel),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(MicroHIDItem), nameof(MicroHIDItem.Owner))),
                new CodeInstruction(OpCodes.Call, Method(typeof(API.Features.Player), nameof(API.Features.Player.Get), new[] { typeof(ReferenceHub) })),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, Field(typeof(MicroHIDItem), nameof(MicroHIDItem.State))),
                new CodeInstruction(OpCodes.Ldarg_2),
                new CodeInstruction(OpCodes.Ldc_I4_1),
                new CodeInstruction(OpCodes.Newobj, GetDeclaredConstructors(typeof(ChangingMicroHIDStateEventArgs))[0]),
                new CodeInstruction(OpCodes.Dup),
                new CodeInstruction(OpCodes.Dup),
                new CodeInstruction(OpCodes.Stloc_S, ev.LocalIndex),
                new CodeInstruction(OpCodes.Call, Method(typeof(Handlers.Player), nameof(Handlers.Player.OnChangingMicroHIDState))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(ChangingMicroHIDStateEventArgs), nameof(ChangingMicroHIDStateEventArgs.IsAllowed))),
                new CodeInstruction(OpCodes.Brfalse_S, returnLabel),
                new CodeInstruction(OpCodes.Ldloc_S, ev.LocalIndex),
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(ChangingMicroHIDStateEventArgs), nameof(ChangingMicroHIDStateEventArgs.NewState))),
                new CodeInstruction(OpCodes.Starg_S, 2),
            });

            var index = newInstructions.FindIndex(instruction => instruction.opcode == OpCodes.Ldloca_S);

            newInstructions[index].WithLabels(continueLabel);

            newInstructions[newInstructions.Count - 1].WithLabels(returnLabel);

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}
