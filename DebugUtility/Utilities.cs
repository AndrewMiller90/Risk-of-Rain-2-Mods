using BepInEx;
using Frogtown;
using RoR2;
using UnityEngine;

namespace DebugUtility
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.sluppy.debugutilities", "DebugUtilities", "1.0.0")]
    public class Utilities : BaseUnityPlugin
    {
        void Awake()
        {
            On.RoR2.PingerController.CmdPing += On_CmdPing; 
            RoR2.Stage.onServerStageBegin += On_ServerStageBegin;
        }
        private void On_CmdPing(On.RoR2.PingerController.orig_CmdPing orig, PingerController self, PingerController.PingInfo incomingPing)
        {
            orig(self, incomingPing);
            FrogtownShared.SendChat(string.Format("Origin: {0}", FormatVector3String(incomingPing.origin)));
            FrogtownShared.SendChat(string.Format("Normal: {0}", FormatVector3String(incomingPing.normal)));
        }
        private string FormatVector3String(Vector3 vector)
        {
            return string.Format("{0}|{1}|{2}", vector.x.ToString(), vector.y.ToString(), vector.z.ToString());
        }
        private void On_ServerStageBegin(Stage stage)
        {
            FrogtownShared.SendChat(stage.sceneDef.sceneName);
        }
    }
}
