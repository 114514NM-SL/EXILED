// -----------------------------------------------------------------------
// <copyright file="InteractingDoor.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Patches.Events.Player
{
#pragma warning disable SA1313
    using System.Collections.Generic;
    using System.Reflection.Emit;

    using API.Features;
    using Exiled.API.Features.Pools;
    using Exiled.Events.EventArgs.Player;

    using HarmonyLib;

    using Interactables.Interobjects.DoorUtils;

    using static HarmonyLib.AccessTools;

    /// <summary>
    ///     Patches <see cref="DoorVariant.ServerInteract(ReferenceHub, byte)" />.
    ///     Adds the <see cref="Handlers.Player.InteractingDoor" /> event.
    /// </summary>
    [HarmonyPatch(typeof(DoorVariant), nameof(DoorVariant.ServerInteract), typeof(ReferenceHub), typeof(byte))]
    internal static class InteractingDoor
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Pool.Get(instructions);

            LocalBuilder ev = generator.DeclareLocal(typeof(InteractingDoorEventArgs));

            List<Label> labels = null;
            Label interactionAllowed = generator.DefineLabel();
            Label permissionDenied = generator.DefineLabel();

            newInstructions.InsertRange(
                0,
                new CodeInstruction[]
                {
                    // InteractingDoorEventArgs ev = new(Player.Get(ply), __instance, true);
                    new(OpCodes.Ldarg_1),
                    new(OpCodes.Call, Method(typeof(Player), nameof(Player.Get), new[] { typeof(ReferenceHub) })),
                    new(OpCodes.Ldarg_0),
                    new(OpCodes.Ldc_I4_1),
                    new(OpCodes.Newobj, GetDeclaredConstructors(typeof(InteractingDoorEventArgs))[0]),
                    new(OpCodes.Stloc_S, ev.LocalIndex),
                });

            int offset = -3;
            int index = newInstructions.FindIndex(instruction => instruction.Calls(Method(typeof(DoorVariant), nameof(DoorVariant.LockBypassDenied)))) + offset;
            labels = newInstructions[index].ExtractLabels();

            newInstructions.InsertRange(
                index,
                new CodeInstruction[]
                {
                    // ev.IsAllowed = false;
                    new CodeInstruction(OpCodes.Ldloc_S, ev.LocalIndex).WithLabels(labels),
                    new(OpCodes.Ldc_I4_0),
                    new(OpCodes.Callvirt, PropertySetter(typeof(InteractingDoorEventArgs), nameof(InteractingDoorEventArgs.IsAllowed))),

                    // Handlers.Player.OnInteractingDoor(ev);
                    new(OpCodes.Ldloc_S, ev.LocalIndex),
                    new(OpCodes.Dup),
                    new(OpCodes.Call, Method(typeof(Handlers.Player), nameof(Handlers.Player.OnInteractingDoor))),

                    // if (ev.IsAllowed)
                    //    go to interactionAllowed;
                    new(OpCodes.Callvirt, PropertyGetter(typeof(InteractingDoorEventArgs), nameof(InteractingDoorEventArgs.IsAllowed))),
                    new(OpCodes.Brtrue_S, interactionAllowed),
                });

            offset = 2;
            index = newInstructions.FindIndex(instruction => instruction.Calls(Method(typeof(DoorVariant), nameof(DoorVariant.AllowInteracting)))) + offset;

            newInstructions.InsertRange(
                index,
                new CodeInstruction[]
                {
                    // ev.IsAllowed = false;
                    new CodeInstruction(OpCodes.Ldloc_S, ev.LocalIndex),
                    new(OpCodes.Ldc_I4_0),
                    new(OpCodes.Callvirt, PropertySetter(typeof(InteractingDoorEventArgs), nameof(InteractingDoorEventArgs.IsAllowed))),

                    // Handlers.Player.OnInteractingDoor(ev);
                    new(OpCodes.Ldloc_S, ev.LocalIndex),
                    new(OpCodes.Dup),
                    new(OpCodes.Call, Method(typeof(Handlers.Player), nameof(Handlers.Player.OnInteractingDoor))),

                    // if (ev.IsAllowed)
                    //    go to interactionAllowed;
                    new(OpCodes.Callvirt, PropertyGetter(typeof(InteractingDoorEventArgs), nameof(InteractingDoorEventArgs.IsAllowed))),
                    new(OpCodes.Brtrue_S, interactionAllowed),
                });

            // attaching permission denied label
            offset = -3;
            index = newInstructions.FindIndex(i => i.Calls(Method(typeof(DoorVariant), nameof(DoorVariant.PermissionsDenied)))) + offset;
            newInstructions[index].WithLabels(permissionDenied);

            offset = -6;
            index = newInstructions.FindIndex(instruction => instruction.Calls(PropertySetter(typeof(DoorVariant), nameof(DoorVariant.NetworkTargetState)))) + offset;

            newInstructions.InsertRange(
                index,
                new CodeInstruction[]
                {
                        // Handlers.Player.OnInteractingDoor(ev);
                        new(OpCodes.Ldloc_S, ev.LocalIndex),
                        new(OpCodes.Dup),
                        new(OpCodes.Call, Method(typeof(Handlers.Player), nameof(Handlers.Player.OnInteractingDoor))),

                        // if (!ev.IsAllowed)
                        //    go to permission denied;
                        new(OpCodes.Callvirt, PropertyGetter(typeof(InteractingDoorEventArgs), nameof(InteractingDoorEventArgs.IsAllowed))),
                        new(OpCodes.Brfalse_S, permissionDenied),

                        // insert interaction Allowed label
                        new CodeInstruction(OpCodes.Nop).WithLabels(interactionAllowed),
                });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Pool.Return(newInstructions);
        }
    }
}