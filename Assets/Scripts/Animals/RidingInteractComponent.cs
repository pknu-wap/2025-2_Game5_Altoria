using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static Define;

namespace GameInteract
{
    [Serializable]
    public class RidingOffset
    {
        public Vector3 Position;
        public Vector3 Rotation;
    }

    public class RidingInteractComponent : InteractBaseComponent
    {
        [SerializeField] Transform mountPoint;
        [SerializeField] Animator animator;
        [SerializeField] RidingOffset offset;
        [SerializeField] float delay = 1.2f;
        [SerializeField] bool riding = false;

        IEntity rider;
        float lastInteractTime = -999f; 

        public override void Interact(IEntity entity)
        {
            base.Interact(entity);


            if (Time.time - lastInteractTime < delay) return;


            lastInteractTime = Time.time;

            if (riding)
                Dismount();
            else
                Mount(entity);
        }

        void Mount(IEntity entity)
        {
            if (entity == null || riding) return;
            if (entity is not PlayerController player) return;

            riding = true;
            rider = player;

            Debug.Log("[Riding] Mount");

            var agent = player.GetComponent<NavMeshAgent>();
            if (agent != null) agent.enabled = false;

            var controller = player.GetComponent<CharacterController>();
            if (controller != null) controller.enabled = false;

            player.transform.SetParent(mountPoint);
            player.transform.localPosition = offset.Position;
            player.transform.localRotation = Quaternion.Euler(offset.Rotation);

            player.Move.SetMoveInput(Vector3.zero);
        }

        void Dismount()
        {
            if (!riding || rider == null) return;

            if (rider is PlayerController player)
            {
                riding = false;
                player.transform.SetParent(null);
                player.transform.position = mountPoint.position + transform.right * 1.0f;
                Debug.Log("[Riding] Dismount");

    
                var agent = player.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    agent.enabled = true;
                    agent.Warp(player.transform.position);
                }

                var controller = player.GetComponent<CharacterController>();
                if (controller != null)
                    controller.enabled = true;
            }

            rider = null;
            ExitInteract();
        }
    }
}
