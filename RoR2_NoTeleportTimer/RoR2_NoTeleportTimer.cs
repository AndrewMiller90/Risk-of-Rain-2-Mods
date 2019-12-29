using BepInEx;
using RoR2;
using UnityEngine;

namespace RoR_NoTeleportTimer
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.sluppy.noteleportertimer", "NoTeleportTimer", "1.0.0")]
    public class RoR_NoTeleportTimer : BaseUnityPlugin
    {
        void Awake()
        {
            On.RoR2.BossGroup.OnDefeatedServer += BossGroup_OnDefeatedServer;
        }

        private void BossGroup_OnDefeatedServer(On.RoR2.BossGroup.orig_OnDefeatedServer orig, BossGroup self)
        {
            orig(self);
            TeleporterInteraction.instance.remainingChargeTimer = 0f;
        }
    }
}