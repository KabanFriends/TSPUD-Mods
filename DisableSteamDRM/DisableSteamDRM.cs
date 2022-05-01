using System;
using MelonLoader;
using HarmonyLib;

namespace DisableSteamDRM
{
    public class DisableSteamDRM : MelonMod
    {
        public override void OnApplicationStart()
        {
            Harmony.PatchAll(typeof(DisableSteamDRM));
        }

        [HarmonyPatch(typeof(SteamManager), "Awake")]
        [HarmonyPrefix]
        public static bool NoSteamAwake()
        {
            return false;
        }
    }
}
