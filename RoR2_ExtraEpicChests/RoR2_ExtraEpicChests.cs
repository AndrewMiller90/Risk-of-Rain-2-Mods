using BepInEx;
using RoR2;
using Frogtown;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace RoR2_ExtraEpicChests
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.sluppy.extraepicchests", "ExtraEpicChests", "1.0.0")]
    public class RoR2_ExtraEpicChests : BaseUnityPlugin
    {
        private static Dictionary<string, List<Vector3>> chestlocations =
        new Dictionary<string, List<Vector3>>()
        {
            ["golemplains"] = new List<Vector3>()
            {
                new Vector3(9.812f , -8.622f , -66.595f),
                new Vector3(-110.975f,-9.85f,45.977f)
            },
            ["blackbeach"] = new List<Vector3>()
            {
                new Vector3(48.185f,-99.242f,-30.545f),
                new Vector3(-120.93f,-85.062f,-281.085f)
            },
            ["goolake"] = new List<Vector3>()
            {
                new Vector3(155.278f,-37.578f,-40.805f)
            },
            ["foggyswamp"] = new List<Vector3>()
            {
                new Vector3(-38.244f,-64.084f,-288.083f)
            },
            ["wispgraveyard"] = new List<Vector3>()
            {
                new Vector3(-242.371f,81.558f,125.81f)
            },
            ["frozenwall"] = new List<Vector3>()
            {
                new Vector3(229.045f,118.76f,-232.755f),
                new Vector3(-119.821f,119.834f,28.632f)
            }
        };

        void Awake()
        {
            RoR2.Stage.onServerStageBegin += On_ServerStageBegin;
        }
        private void On_ServerStageBegin(Stage stage)
        {
            if (chestlocations.ContainsKey(stage.sceneDef.sceneName))
            {
                //grab first position
                var position = chestlocations[stage.sceneDef.sceneName][0];
                SpawnCard chestCard = Resources.Load<SpawnCard>("SpawnCards/InteractableSpawnCard/iscChest3");
                GameObject chest = chestCard.DoSpawn(position, Quaternion.Euler(0, 0, 0), null);
            }
        }
    }
}
