using GameInteract;
using System;
using UnityEngine;

namespace GameInteract
{
    public class SpawnInteractComponent : InteractBaseComponent, ITimer,IDisposable
    {
        [SerializeField] float time;
        public float Timer => time;
        public Action<ITimer> TimeEndInvoke { get; private set; }
        //TODO : 스폰시킬 객체  
          
        public void Dispose()
        {
           //해당객체 비활성화 시키기 
        }

        public override void Interact() { }
        public void StartTimer() { }
    }
}