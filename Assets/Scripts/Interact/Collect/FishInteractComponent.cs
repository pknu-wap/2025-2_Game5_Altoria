using System.Collections.Generic;
using UnityEngine;
using static Define;
using GameData;
using Common;// ªË¡¶ «“ ∞Õ

namespace GameInteract
{
    public class FishInteractComponent : InteractBaseComponent
    {
        [SerializeField] AreaType areaType;
        [Header("Setting of WorldUI")]
        [SerializeField] Canvas canvas;
        [SerializeField] RectTransform listRoot;
        Content collectType = Content.Fish;


        public override void EnterInteract()
        {
            base.EnterInteract();

            for(int i = 0; i < GameSystem.Instance.GetFishData(areaType).rows.Count; i++)
            {
                var fishSOData = GameSystem.Instance.GetFishData(areaType);
                var itemData = GameDB.GetItemData(fishSOData.rows[i].ID);
                var newGO = Resources.Load<GameObject>("UI/FishISlot");
                Instantiate(newGO, listRoot);
                if(newGO.TryGetComponent<FishSlot>(out var fishSlot))
                {
                    fishSlot.Init(itemData.SpriteAddress, fishSOData.rows[i].Probability.ToString());
                }
            }

            canvas.gameObject.SetActive(true);
        }

        public override void ExitInteract()
        {
            base.ExitInteract();

            canvas.gameObject.SetActive(false);
        }

        public override void Interact()
        {
            CollectTimer timer = new(2);
            timer.OnFinished += EndCollect;
        }

        protected void EndCollect(ITimer timer)
        {
            Debug.Log($"{GetType()} : ≥¨Ω√ ≥°!");

            List<(FishData, float)> probList = new();

            var fishDataSO = GameSystem.Instance.GetFishData(areaType);
            for (int i = 0; i < fishDataSO.rows.Count; i++)
            {
                probList.Add((fishDataSO.rows[i], fishDataSO.rows[i].Probability));
                Debug.Log($"{GetType()} : {GameDB.GetItemData(fishDataSO.rows[i].ID).Name} ≈âµÊ ∞°¥…");
            }
            var item = GameSystem.Random.Pick(probList);

            Debug.Log($"{GetType()} : {item.ID} ≈âµÊ!");

            GameSystem.Life.AddExp<CollectInteractComponent>(10);

            EndInteract();
        }
    }

}