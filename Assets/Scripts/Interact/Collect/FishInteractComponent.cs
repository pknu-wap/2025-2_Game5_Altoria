using Common;
using GameData;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace GameInteract
{
    public class FishInteractComponent : InteractBaseComponent
    {
        [SerializeField] AreaType areaType;
        [Header("Setting of WorldUI")]
        const string path = "UI/";

        public override void EnterInteract()
        {
            base.EnterInteract();
        }

        public override void ExitInteract()
        {
            base.ExitInteract();
        }

        public override void Interact(IEntity entity)
        {
            CollectTimer timer = new(2);
            timer.OnFinished += EndCollect;
        }

        protected void EndCollect(ITimer timer)
        {
            
            List<(FishGroup, float)> probList = new();

            var fishDic = GameDB.GetFishData(areaType).Value;
            var data = fishDic[areaType.ToString()];

            for (int i = 0; i < data.FishGroups.Count; i++)
                probList.Add((data.FishGroups[i], data.FishGroups[i].Probability));

            var item = Common.GameSystem.Random.Pick(probList, GameDB.GetUpgradeData(GameSystem.Inventory.GetEquipItemLevel(Type)).Bous);
            GameSystem.Inventory.AddItem(gameObject.name, 1);

            Common.GameSystem.Life.AddExp<CollectInteractComponent>(10);

            EndInteract();
        }
    }

}