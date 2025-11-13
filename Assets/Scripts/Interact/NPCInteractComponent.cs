using GameInteract;
using System;
using UnityEngine;

namespace GameUI
{
    public class NPCInteractComponent : InteractBaseComponent
    {
      
        public override void Interact(IEntity entity) { StartDialogue(); }
        void StartDialogue()
        { 

        }
        void EndDialogue() { ExitInteract(); }
    }
}