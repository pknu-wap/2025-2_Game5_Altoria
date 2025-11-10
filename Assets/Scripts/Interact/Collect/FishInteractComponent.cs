using System.Collections.Generic;
using UnityEngine;
using static Define;
using GameData;

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

        public override void Interact()
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

            // TODO: 현재 진행 중인 상호작용 타입에 대한 장착된 아이템 가져오기
            var item = Common.GameSystem.Random.Pick(probList, GameDB.GetUpgradeData(0).Bous);
            // TODO: item 인벤토리에 저장

            Common.GameSystem.Life.AddExp<CollectInteractComponent>(10);

            EndInteract();
        }
    }

}