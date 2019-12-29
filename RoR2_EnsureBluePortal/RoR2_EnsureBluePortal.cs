using BepInEx;
using UnityEngine;
using RoR2;

namespace RoR2_FuckThisButton
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.sluppy.ensureblueportal", "EnsureBluePortal", "1.0.0")]
    public class RoR2_EnsureBluePortal : BaseUnityPlugin
    {
        void Awake()
        {
            On.RoR2.Run.OnServerTeleporterPlaced += EnsureBluePortal;
        }
        private void EnsureBluePortal(On.RoR2.Run.orig_OnServerTeleporterPlaced orig, Run self, SceneDirector sceneDirector, GameObject teleporter)
         {
            orig(self, sceneDirector, teleporter);
            TeleporterInteraction.instance.baseShopSpawnChance = 100;
        }
    }
}
