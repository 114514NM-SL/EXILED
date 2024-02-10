// -----------------------------------------------------------------------
// <copyright file="Slapped.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Patches.Events.Scp3114
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Emit;
    using Exiled.API.Features;
    using Exiled.API.Features.Pools;
    using Exiled.Events.Attributes;
    using Exiled.Events.EventArgs.Scp3114;
    using Exiled.Events.Handlers;

    using HarmonyLib;
    using PlayerRoles.FirstPersonControl;
    using PlayerRoles.PlayableScps.Scp3114;
    using PlayerRoles.PlayableScps.Subroutines;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="Scp3114Slap.DamagePlayers" />.
    /// Adds the <see cref="Handlers.Scp3114.Slapped" /> event.
    /// </summary>
    [EventPatch(typeof(Scp3114), nameof(Scp3114.Slapped))]
    [HarmonyPatch(typeof(PlayerRoles.PlayableScps.Scp3114.Scp3114Slap), nameof(PlayerRoles.PlayableScps.Scp3114.Scp3114Slap.DamagePlayers))]
    internal class Slapped
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Pool.Get(instructions);

            int index = newInstructions.FindIndex(instruction =>
                instruction.opcode == OpCodes.Ret);

            Label label = generator.DefineLabel();

            newInstructions[index] = new CodeInstruction(OpCodes.Br, label);

            newInstructions.InsertRange(newInstructions.Count - 1, new CodeInstruction[]
            {
                // Player::Get(Owner)
                new CodeInstruction(OpCodes.Ldarg_0).WithLabels(label).MoveLabelsFrom(newInstructions[newInstructions.Count - 1]),
                new(OpCodes.Callvirt, PropertyGetter(typeof(Scp3114Slap), nameof(Scp3114Slap.Owner))),
                new(OpCodes.Call, Method(typeof(API.Features.Player), nameof(API.Features.Player.Get), new[] { typeof(ReferenceHub) })),

                new(OpCodes.Ldarg_0),
                new(OpCodes.Ldsfld, Field(typeof(ScpAttackAbilityBase<Scp3114Role>), nameof(ScpAttackAbilityBase<Scp3114Role>._syncAttack))),

                new(OpCodes.Ldloc_1),
                new(OpCodes.Call, Method(typeof(API.Features.Player), nameof(API.Features.Player.Get), new[] { typeof(ReferenceHub) })),

                // SlappedEventArgs ev = new(player);
                new(OpCodes.Newobj, GetDeclaredConstructors(typeof(SlappedEventArgs))[0]),

                // Handlers.Scp3114.OnSlapped(ev)
                new(OpCodes.Call, Method(typeof(Handlers.Scp3114), nameof(Handlers.Scp3114.OnSlapped))),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                Log.Info($"{newInstructions[z].opcode} {newInstructions[z].operand} : {string.Join(", ", newInstructions[z].labels.Select(x => newInstructions.FindIndex(y => y.operand == (object)x)))}");

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Pool.Return(newInstructions);
        }
    }
}
