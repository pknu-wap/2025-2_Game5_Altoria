using System;
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

    public class RidingInteractComponent : BaseEntityComponent, IRiding, IMoveInput, IPlayerMovable
    {
        [SerializeField] Transform mountPoint;
        [SerializeField] Animator animator;
        [SerializeField] RidingOffset offset;
        [SerializeField] MoveData data;
        [SerializeField] float delay = 1.2f;

        IEntity rider;
        IMove move;
        MoveHandler moveHandler;  
        float lastRidingTime = -999f;

        public Transform MountPoint => mountPoint;
        public bool IsOccupied => rider != null;

        public IMove Move => move;
        public IMoveData MoveData => data;

        public event Action<IEntity> OnMounted;
        public event Action<IEntity> OnDismounted;

        void Start()
        {
            move = new Move();
            move.SetEntity(this);
            moveHandler = new MoveHandler(move); 
        }

        void Update()
        {
            moveHandler?.Tick();
        }

        public void Ride(IEntity entity)
        {
            if (Time.time - lastRidingTime < delay) return;
            lastRidingTime = Time.time;

            if (IsOccupied)
                Dismount(entity);
            else
                Mount(entity);
        }

        void Mount(IEntity entity)
        {
            if (entity == null || rider != null) return;

            rider = entity;
            Transform target = entity.transform;

 
            Vector3 mountForward = mountPoint.forward;
            mountForward.y = 0f; 
            if (mountForward.sqrMagnitude > 0.001f)
                target.rotation = Quaternion.LookRotation(mountForward);

            Vector3 worldPos = mountPoint.TransformPoint(offset.Position);
            target.position = worldPos;

           
            target.SetParent(mountPoint, true);
            target.localRotation = Quaternion.Euler(offset.Rotation);

         
            Vector3 worldScale = target.lossyScale;
            target.localScale = new Vector3(
                worldScale.x / target.lossyScale.x * target.localScale.x,
                worldScale.y / target.lossyScale.y * target.localScale.y,
                worldScale.z / target.lossyScale.z * target.localScale.z
            );

            Debug.Log($"[Riding] Mounted (aligned): {entity}");
            OnMounted?.Invoke(entity);
        }



        void Dismount(IEntity entity)
        {
            if (rider == null) return;
            OnMoveCancel();
            var target = rider.transform;
            target.SetParent(null);
            target.position = mountPoint.position + transform.forward * 1.5f;

            Debug.Log($"[Riding] Dismounted: {entity}");
            OnDismounted?.Invoke(entity);
            
            ResetEvent();
            rider = null;
        }
        void StopMove()
        { 
            Move.SetMoveInput(Vector3.zero);

        }
        public void ForceDismount()
        {
            if (rider != null) StopAllCoroutines();
        }

        
        void ResetEvent()
        {
            OnMounted = null;
            OnDismounted = null;
        }

        #region IMoveInput Implementation

        public void OnMoveInput(Vector2 dir)
        {
            if (rider == null) return;
            moveHandler?.SetInput(dir);

            if (animator)
                animator.SetBool("IsMove", dir.sqrMagnitude > 0.01f);
        }

        public void OnMoveCancel()
        {
            moveHandler?.SetInput(Vector2.zero);
            move.Stop();

            if (animator)
                animator.SetBool("IsMove", false);
        }

        public void OnJumpInput()
        {
            move.Jump();

            if (animator)
                animator.SetTrigger("Jump");
        }

        #endregion
    }
}
