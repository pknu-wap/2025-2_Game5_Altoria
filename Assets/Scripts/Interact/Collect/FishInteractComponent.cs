using UnityEngine;

namespace GameInteract
{
    public class FishInteractComponent : CollectInteractComponent
    {
        protected override void FuncForEndCollect()
        {
            Debug.Log($"{GetType()} : ³¬½Ã ³¡!");
        }
    }

}