// -----------------------------------------------------------------------
// <copyright file="SearchingPickupEvent.cs" company="Exiled Team">
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
    using Exiled.Events.Handlers;

    using HarmonyLib;

    using InventorySystem.Items.Pickups;
    using InventorySystem.Searching;

    using Mirror;
    using Mirror.LiteNetLib4Mirror;

    using NorthwoodLib.Pools;

    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="SearchCoordinator.ReceiveRequestUnsafe"/>.
    /// Adds the <see cref="Handlers.Player.SearchingPickup"/> event.
    /// </summary>
    [HarmonyPatch(typeof(SearchCoordinator), nameof(SearchCoordinator.ReceiveRequestUnsafe))]
    internal static class SearchingPickupEvent
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            int offset = 1;
            int index = newInstructions.FindIndex(i => i.opcode == OpCodes.Stind_Ref) + offset;

            LocalBuilder ev = generator.DeclareLocal(typeof(SearchingPickupEventArgs));

            Label allowLabel = generator.DefineLabel();

            newInstructions.RemoveRange(index, 14);

            //
            newInstructions.InsertRange(index, new[]
            {
                new(OpCodes.Ldarg_0),
                new(OpCodes.Callvirt, PropertyGetter(typeof(SearchCoordinator), nameof(SearchCoordinator.Hub))),
                new(OpCodes.Call, Method(typeof(API.Features.Player), nameof(API.Features.Player.Get), new[] { typeof(ReferenceHub) })),

                new(OpCodes.Ldloca_S, 0),
                new(OpCodes.Callvirt, PropertyGetter(typeof(SearchRequest), nameof(SearchRequest.Target))),

                new(OpCodes.Ldloca_S, 0),
                new(OpCodes.Callvirt, PropertyGetter(typeof(SearchRequest), nameof(SearchRequest.Body))),

                new(OpCodes.Ldarg_2),
                new(OpCodes.Ldind_Ref),

                new(OpCodes.Ldloca_S, 0),
                new(OpCodes.Callvirt, PropertyGetter(typeof(SearchRequest), nameof(SearchRequest.Target))),
                new(OpCodes.Ldarg_0),
                new(OpCodes.Callvirt, PropertyGetter(typeof(SearchCoordinator), nameof(SearchCoordinator.Hub))),
                new(OpCodes.Callvirt, Method(typeof(ItemPickupBase), nameof(ItemPickupBase.SearchTimeForPlayer))),

                new(OpCodes.Newobj, GetDeclaredConstructors(typeof(SearchingPickupEventArgs))[0]),
                new(OpCodes.Dup),
                new(OpCodes.Dup),
                new(OpCodes.Stloc_S, ev.LocalIndex),
                new(OpCodes.Call, Method(typeof(Handlers.Player), nameof(Handlers.Player.OnSearchPickupRequest))),
                new(OpCodes.Callvirt, PropertyGetter(typeof(SearchingPickupEventArgs), nameof(SearchingPickupEventArgs.IsAllowed))),
                new(OpCodes.Brtrue_S, allowLabel),

                new(OpCodes.Ldarg_1),
                new(OpCodes.Initobj, typeof(SearchSession)),
                new(OpCodes.Ldarg_2),
                new(OpCodes.Ldnull),
                new(OpCodes.Stind_Ref),
                new(OpCodes.Ldc_I4_1),
                new(OpCodes.Ret),

                new CodeInstruction(OpCodes.Ldarg_2).WithLabels(allowLabel),
                new(OpCodes.Ldloc_S, ev.LocalIndex),
                new(OpCodes.Callvirt, PropertyGetter(typeof(SearchingPickupEventArgs), nameof(SearchingPickupEventArgs.SearchCompletor))),
                new(OpCodes.Stind_Ref),

                new(OpCodes.Ldloc_S, ev.LocalIndex),
                new(OpCodes.Callvirt, PropertyGetter(typeof(SearchingPickupEventArgs), nameof(SearchingPickupEventArgs.SearchSession))),
                new(OpCodes.Stloc_1),
            });

            offset = -5;
            index = newInstructions.FindIndex(i => i.opcode == OpCodes.Stloc_S &&
                                                   i.operand is LocalBuilder { LocalIndex: 4 }) + offset;

            newInstructions.RemoveRange(index, 5);

            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Ldloc_S, ev.LocalIndex),
                new(OpCodes.Callvirt, PropertyGetter(typeof(SearchingPickupEventArgs), nameof(SearchingPickupEventArgs.SearchTime))),
            });

            for (int z = 0; z < newInstructions.Count; z++)
            {
                Log.Debug(newInstructions[z]);
                yield return newInstructions[z];
            }

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }

        /*private static bool Prefix(SearchCoordinator __instance, ref bool __result, out SearchSession? session, out SearchCompletor completor)
        {
            try
            {
                SearchRequest request = __instance.SessionPipe.Request;

                SearchingPickupEventArgs ev = new(Player.Get(__instance.Hub),
                    request.Target,
                    request.Body,
                    SearchCompletor.FromPickup(__instance, request.Target, __instance.ServerMaxRayDistanceSqr),
                    request.Target.SearchTimeForPlayer(__instance.Hub));
                Handlers.Player.OnSearchPickupRequest(ev);

                completor = ev.SearchCompletor;
                if (!ev.IsAllowed)
                {
                    session = null;
                    completor = null;
                    __result = true;
                    return false;
                }

                SearchSession body = ev.SearchSession;
                if (!__instance.isLocalPlayer)
                {
                    double num = NetworkTime.time - request.InitialTime;
                    double num2 = LiteNetLib4MirrorServer.Peers[__instance.connectionToClient.connectionId].Ping * 0.001 * __instance.serverDelayThreshold;
                    float searchTime = ev.SearchTime;
                    if (num < 0.0 || num2 < num)
                    {
                        body.InitialTime = NetworkTime.time - num2;
                        body.FinishTime = body.InitialTime + searchTime;
                    }
                    else if (Math.Abs(body.FinishTime - body.InitialTime - searchTime) > 0.001)
                    {
                        body.FinishTime = body.InitialTime + searchTime;
                    }
                }

                session = body;
                __result = true;
                return false;
            }
            catch (Exception exception)
            {
                Log.Error($"{typeof(SearchingPickupEvent).FullName}.{nameof(Prefix)}:\n{exception}");
                session = null;
                completor = null;
                return true;
            }
        }*/
    }
}
