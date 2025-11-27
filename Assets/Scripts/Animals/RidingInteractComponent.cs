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

    public class RidingInteractComponent : BaseEntityComponent, IRiding, IMoveInput, IPlayerMovable,IJumper
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
            Transform root = entity.transform;
      
            Vector3 mountForward = mountPoint.forward;
            mountForward.y = 0f;
            if (mountForward.sqrMagnitude > 0.001f)
                root.rotation = Quaternion.LookRotation(mountForward);

            Vector3 worldPos = mountPoint.TransformPoint(offset.Position);
            root.position = worldPos;

            root.SetParent(mountPoint, true);

            root.localRotation = Quaternion.Euler(offset.Rotation);
         
            if (entity is IModel model) model.Model.localRotation = Quaternion.identity;
            Vector3 worldScale = root.lossyScale;
            root.localScale = new Vector3(
                worldScale.x / root.lossyScale.x * root.localScale.x,
                worldScale.y / root.lossyScale.y * root.localScale.y,
                worldScale.z / root.lossyScale.z * root.localScale.z
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
            animator.SetTrigger("Jump");

        }

        #endregion
    }
}
