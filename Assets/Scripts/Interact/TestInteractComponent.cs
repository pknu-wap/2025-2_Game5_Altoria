using UnityEngine;

namespace GameInteract
{
    public class TestInteractComponent : InteractBaseComponent
    {
        public override void Interact(IEntity entity)
        {
            Debug.Log("Interact");
        }

      
    }
}