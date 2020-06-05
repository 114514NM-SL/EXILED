﻿// -----------------------------------------------------------------------
// <copyright file="Shoot.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Patches.Events
{
    #pragma warning disable SA1313
    using System;
    using Exiled.Core.API.Features;
    using Exiled.Events.Handlers.EventArgs;
    using Exiled.Core;
    using HarmonyLib;
    using UnityEngine;

    /// <summary>
    /// Patches <see cref="WeaponManager.CallCmdShoot(GameObject, string, Vector3, Vector3, Vector3)"/>.
    /// Adds the <see cref="Handlers.Player.Shooting"/> and <see cref="Handlers.Player.Shot"/> events.
    /// </summary>
    [HarmonyPatch(typeof(WeaponManager), nameof(WeaponManager.CallCmdShoot))]
    public class Shoot
    {
        /// <summary>
        /// Prefix of <see cref="WeaponManager.CallCmdShoot(GameObject, string, Vector3, Vector3, Vector3)"/>.
        /// </summary>
        /// <param name="__instance">The <see cref="WeaponManager"/> instance.</param>
        /// <param name="target">The shoot target.</param>
        /// <param name="hitboxType">The hitbox type.</param>
        /// <param name="dir">The shoot direction.</param>
        /// <param name="sourcePos">The shoot source position.</param>
        /// <param name="targetPos">The shoot target position.</param>
        /// <returns>Returns a value indicating whether the original method has to be executed or not.</returns>
        public static bool Prefix(WeaponManager __instance, GameObject target, string hitboxType, Vector3 dir, Vector3 sourcePos, Vector3 targetPos)
        {
            if (!__instance._iawRateLimit.CanExecute(true))
                return false;
            int itemIndex = __instance.hub.inventory.GetItemIndex();
            if (itemIndex < 0 || itemIndex >= __instance.hub.inventory.items.Count || __instance.curWeapon < 0 ||
                ((__instance.reloadCooldown > 0.0 || __instance.fireCooldown > 0.0) && !__instance.isLocalPlayer) ||
                (__instance.hub.inventory.curItem != __instance.weapons[__instance.curWeapon].inventoryID ||
                 __instance.hub.inventory.items[itemIndex].durability <= 0.0))
                return false;

            Log.Debug("Invoking shoot event", PluginManager.ShouldDebugBeShown);

            var shootingEventArgs = new ShootingEventArgs(Player.Get(__instance.gameObject), target, targetPos);

            Handlers.Player.OnShooting(shootingEventArgs);

            if (!shootingEventArgs.IsAllowed)
                return false;

            if (Vector3.Distance(__instance.camera.transform.position, sourcePos) > 6.5)
            {
                __instance.GetComponent<CharacterClassManager>().TargetConsolePrint(
                    __instance.connectionToClient,
                    "Shot rejected - Code 2.2 (difference between real source position and provided source position is too big)",
                    "gray");
            }
            else
            {
                __instance.hub.inventory.items.ModifyDuration(
                    itemIndex,
                    __instance.hub.inventory.items[itemIndex].durability - 1f);
                __instance.scp268.ServerDisable();
                __instance.fireCooldown =
                  (float)(1.0 / (__instance.weapons[__instance.curWeapon].shotsPerSecond *
                                  (double)__instance.weapons[__instance.curWeapon].allEffects.firerateMultiplier) *
                           0.800000011920929);
                float sourceRangeScale = __instance.weapons[__instance.curWeapon].allEffects.audioSourceRangeScale;
                __instance.GetComponent<Scp939_VisionController>()
                  .MakeNoise(Mathf.Clamp((float)(sourceRangeScale * (double)sourceRangeScale * 70.0), 5f, 100f));
                bool flag = target != null;
                if (targetPos == Vector3.zero)
                {
                    if (Physics.Raycast(sourcePos, dir, out RaycastHit raycastHit, 500f, __instance.raycastMask))
                    {
                        HitboxIdentity component = raycastHit.collider.GetComponent<HitboxIdentity>();
                        if (component != null)
                        {
                            WeaponManager componentInParent = component.GetComponentInParent<WeaponManager>();
                            if (componentInParent != null)
                            {
                                flag = false;
                                target = componentInParent.gameObject;
                                hitboxType = component.id;
                                targetPos = componentInParent.transform.position;
                            }
                        }
                    }
                }
                else
                {
                    if (Physics.Linecast(sourcePos, targetPos, out RaycastHit raycastHit, __instance.raycastMask))
                    {
                        HitboxIdentity component = raycastHit.collider.GetComponent<HitboxIdentity>();
                        if (component != null)
                        {
                            WeaponManager componentInParent = component.GetComponentInParent<WeaponManager>();
                            if (componentInParent != null)
                            {
                                if (componentInParent.gameObject == target)
                                {
                                    flag = false;
                                }
                                else if (componentInParent.scp268.Enabled)
                                {
                                    flag = false;
                                    target = componentInParent.gameObject;
                                    hitboxType = component.id;
                                    targetPos = componentInParent.transform.position;
                                }
                            }
                        }
                    }
                }

                CharacterClassManager c = null;
                if (target != null)
                    c = target.GetComponent<CharacterClassManager>();
                if (c != null && __instance.GetShootPermission(c, false))
                {
                    if (Math.Abs(__instance.camera.transform.position.y - c.transform.position.y) > 40.0)
                    {
                        __instance.GetComponent<CharacterClassManager>().TargetConsolePrint(
                            __instance.connectionToClient,
                            "Shot rejected - Code 2.1 (too big Y-axis difference between source and target)",
                            "gray");
                    }
                    else if (Vector3.Distance(c.transform.position, targetPos) > 6.5)
                    {
                        __instance.GetComponent<CharacterClassManager>().TargetConsolePrint(
                            __instance.connectionToClient,
                            "Shot rejected - Code 2.3 (difference between real target position and provided target position is too big)",
                            "gray");
                    }
                    else if (Physics.Linecast(__instance.camera.transform.position, sourcePos, __instance.raycastServerMask))
                    {
                        __instance.GetComponent<CharacterClassManager>().TargetConsolePrint(
                            __instance.connectionToClient,
                            "Shot rejected - Code 2.4 (collision between source positions detected)",
                            "gray");
                    }
                    else if (flag && Physics.Linecast(sourcePos, targetPos, __instance.raycastServerMask))
                    {
                        __instance.GetComponent<CharacterClassManager>().TargetConsolePrint(
                            __instance.connectionToClient,
                            "Shot rejected - Code 2.5 (collision on shot line detected)",
                            "gray");
                    }
                    else
                    {
                        float num1 = Vector3.Distance(__instance.camera.transform.position, target.transform.position);
                        float num2 = __instance.weapons[__instance.curWeapon].damageOverDistance.Evaluate(num1);
                        string upper = hitboxType.ToUpper();
                        if (upper != "HEAD")
                        {
                            if (upper != "LEG")
                            {
                                if (upper == "SCP106")
                                    num2 /= 10f;
                            }
                            else
                            {
                                num2 /= 2f;
                            }
                        }
                        else
                        {
                            num2 *= 4f;
                        }

                        Log.Debug("Invoking late shoot.", PluginManager.ShouldDebugBeShown);

                        var shotEventArgs = new ShotEventArgs(Player.Get(__instance.gameObject), target, hitboxType, num1, num2);

                        Handlers.Player.OnShot(shotEventArgs);

                        if (!shotEventArgs.CanHurt)
                            return false;

                        __instance.hub.playerStats.HurtPlayer(
                          new PlayerStats.HitInfo(
                            num2 * __instance.weapons[__instance.curWeapon].allEffects.damageMultiplier *
                            __instance.overallDamagerFactor,
                            __instance.hub.nicknameSync.MyNick + " (" + __instance.hub.characterClassManager.UserId + ")",
                            DamageTypes.FromWeaponId(__instance.curWeapon),
                            __instance.hub.queryProcessor.PlayerId),
                          c.gameObject);
                        __instance.RpcConfirmShot(true, __instance.curWeapon);
                        __instance.PlaceDecal(true, new Ray(__instance.camera.position, dir), (int)c.CurClass, num1);
                    }
                }
                else if (target != null && hitboxType == "window" && target.GetComponent<BreakableWindow>() != null)
                {
                    float damage = __instance.weapons[__instance.curWeapon].damageOverDistance
                      .Evaluate(Vector3.Distance(__instance.camera.transform.position, target.transform.position));
                    target.GetComponent<BreakableWindow>().ServerDamageWindow(damage);
                    __instance.RpcConfirmShot(true, __instance.curWeapon);
                }
                else
                {
                    __instance.PlaceDecal(false, new Ray(__instance.camera.position, dir), __instance.curWeapon, 0.0f);
                    __instance.RpcConfirmShot(false, __instance.curWeapon);
                }
            }

            return false;
        }
    }
}
