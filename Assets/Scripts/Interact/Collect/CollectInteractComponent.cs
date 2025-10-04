using GameInteract;
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
            Interacting();
        }

        protected virtual void Interacting()
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
            var item = Manager.Collect.GetRandomItem(dropTable, currentTool);
            Debug.Log($"결과: {item} 획득!");

            FuncForEndCollect();
        }

        protected virtual void FuncForEndCollect() 
        {

        }
    }

}