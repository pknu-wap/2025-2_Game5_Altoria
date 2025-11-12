
using UnityEngine;

namespace GameInteract
{
    public class PickUpItemComponent :InteractBaseComponent,IDestroyable
    {
        
        public override void Interact(IEntity entity)
        {
            //TODO: ITEMDATA  

            Destroy(this.gameObject);
        }
        public void Destroy(GameObject obj) { Destroy(obj); }
    }
}