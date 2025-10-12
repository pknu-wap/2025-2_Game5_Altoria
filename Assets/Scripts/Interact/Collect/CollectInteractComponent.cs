using GameInteract;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Define;

namespace GameInteract
{
    public class CollectInteractComponent : InteractBaseComponent
    {
        [SerializeField] protected CollectType collectType = CollectType.None;
        [Header("지역&콘텐츠에 따른 SO")]
        [SerializeField] protected CollectInteractSO dropTable;
        [Header("플레이어가 사용하는 도구(추후 인스펙터에서 설정X)")]
        [SerializeField] protected CollectToolSO currentTool;

        private bool doing = false;

        public override void Interact()
        {
            if (doing)
                return;
            doing = true;

            CollectTimer timer = new(2);
            timer.OnFinished += EndCollect;
        }

        // 채집 타이머가 끝나면 채집이 종료
        private void EndCollect(ITimer timer)
        {
            Debug.Log($"{GetType()} : {collectType.ToString()} 종료.");

            doing = false;
            
            List<(CollectItem, float)> probList= new List<(CollectItem, float)> ();
            for (int i = 0; i < dropTable.drops.Count; i++)
            {
                probList.Add((dropTable.drops[i], dropTable.drops[i].baseProbability));
            }
            var item = Manager.Random.Pick(probList);

            Debug.Log($"결과: {item.itemID} 획득!");

            EndInteract();
            FuncForEndCollect();
        
        }

        protected virtual void FuncForEndCollect() 
        {

        }

    }

}