using Common;
using GameData;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Define;

namespace GameInteract
{
    public class CollectInteractComponent : InteractBaseComponent
    {
        [SerializeField] protected Content collectType = Content.None;
        [SerializeField] string objectID;

        public override void Interact()
        {
            CollectTimer timer = new(2);
            timer.OnFinished += EndCollect;
        }

        // 채집 타이머가 끝나면 채집이 종료
        private void EndCollect(ITimer timer)
        {
            Debug.Log($"{GetType()} : {collectType.ToString()} 종료.");

            List<(CollectData, float)> probList = new List<(CollectData, float)>();
            var data = GameSystem.Instance.GetCollectData(objectID);

            for (int i = 0; i < data.rows.Count; i++)
            {
                probList.Add((data.rows[i], data.rows[i].Probability));
                Debug.Log($"{GetType()} : {GameDB.GetItemData(data.rows[i].ID).Name} {data.rows[i].Count}개 흭득 가능");
            }

            var item = GameSystem.Random.Pick(probList);
            Debug.Log($"{GetType()} : {item.ID} {item.Count}개 흭득!");

            GameSystem.Life.AddExp<CollectInteractComponent>(10);

            EndInteract();
        }
    }

}