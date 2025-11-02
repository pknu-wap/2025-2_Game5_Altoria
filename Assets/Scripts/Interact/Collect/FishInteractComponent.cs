using System.Collections.Generic;
using UnityEngine;
using static Define;
using GameData;
using Common;

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

            var fishDic = GameDB.GetFishData(areaType).Value;
            var data = fishDic[areaType.ToString()];

            //for (int i = 0; i < data.FishGroups.Count; i++)
            //{
            //    var itemData = GameDB.GetItemData(data.FishGroups[i].ID);
            //    var newGO = Resources.Load<GameObject>(path + nameof(FishSlot));
            //    Instantiate(newGO, listRoot);
            //    if (newGO.TryGetComponent<FishSlot>(out var fishSlot))
            //    {
            //        fishSlot.Init(itemData.SpriteAddress, data.FishGroups[i].Probability.ToString());
            //    }
            //}

            //canvas.gameObject.SetActive(true);
        }

        public override void ExitInteract()
        {
            base.ExitInteract();

            //canvas.gameObject.SetActive(false);
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

            var item = Common.GameSystem.Random.Pick(probList);

          
            Common.GameSystem.Life.AddExp<CollectInteractComponent>(10);

            EndInteract();
        }
    }

}