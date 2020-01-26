using System;
using EXILED.Shared.Helpers;
using Harmony;

namespace EXILED.Events.Patches
{
    [HarmonyPatch(typeof(CharacterClassManager), "SetClassID")]
    public class SetClassEvent
    {
        public static void Postfix(CharacterClassManager __instance, RoleType id)
        {
            if (EventPlugin.SetClassPatchDisable)
                return;

            try
            {
                Events.Events.InvokeSetClass(__instance, id);
            }
            catch (Exception e)
            {
                LogHelper.Error($"SetClass event error: {e}");
            }
        }
    }
}