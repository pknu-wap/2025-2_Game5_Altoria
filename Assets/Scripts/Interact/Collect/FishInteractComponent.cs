using UnityEngine;

namespace GameInteract
{
    public class FishInteractComponent : CollectInteractComponent
    {
        protected override void Interacting()
        {
            base.Interacting();

            Debug.Log($"{GetType()} : ≥¨Ω√ Ω√¿€!");
        }

        protected override void FuncForEndCollect()
        {
            Debug.Log($"{GetType()} : ≥¨Ω√ ≥°!");
        }
    }

}