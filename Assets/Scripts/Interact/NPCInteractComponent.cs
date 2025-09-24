using GameInteract;
using System;
using UnityEngine;

namespace GameUI
{
    public class NPCInteractComponent : InteractBaseComponent
    {
      
        public override void Interact() { StartDialogue(); }
        void StartDialogue()
        { 

        }
        void EndDialogue() { ExitInteract(); }
    }
}