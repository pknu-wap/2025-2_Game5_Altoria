using GameInteract;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Define;

namespace GameInteract
{
    public class CollectInteractComponent : InteractBaseComponent
    {
        [SerializeField] protected CollectType collectType = CollectType.None;
        [SerializeField] protected CollectInteractSO dropTable;
        [SerializeField] protected CollectToolSO currentTool;

        private bool doing = false;

        public override void Interact()
        {
            Interacting();
        }

        protected virtual void Interacting()
        {
            if (doing) 
            { 
                Debug.Log($"{GetType()} : 이미 진행중"); 
                return; 
            }
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

        protected virtual void FuncForEndCollect() {}
    }

}