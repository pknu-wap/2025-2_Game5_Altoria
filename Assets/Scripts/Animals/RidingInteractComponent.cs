using System;
using UnityEngine;
using static Define;

namespace GameInteract
{
    [Serializable]
    public class RidingOffset
    {
        public Vector3 Position;
        public Vector3 Rotation;
    }

    public class RidingInteractComponent : BaseEntityComponent, IRiding, IMoveInput, IPlayerMovable, IJumper
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

        Vector3 savedScale;
        Quaternion savedRotation;

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
            Transform root = rider.transform;

            savedRotation = root.rotation;
            savedScale = root.localScale;

            Vector3 forward = mountPoint.forward;
            forward.y = 0f;
            if (forward.sqrMagnitude > 0.001f)
                root.rotation = Quaternion.LookRotation(forward);

            root.SetParent(mountPoint, false);
            root.localPosition = Vector3.zero;         
            root.localRotation = Quaternion.identity;
            root.localPosition = offset.Position;    
            root.localRotation = Quaternion.Euler(offset.Rotation);

            if (entity is IModel model)
            {
                model.Model.localPosition = new Vector3(0, model.Model.localPosition.y, 0);
                model.Model.localRotation = Quaternion.Euler(0f, 0f, 0f); 
            }
            

            var parentScale = mountPoint.lossyScale;
            root.localScale = new Vector3(
                savedScale.x / parentScale.x,
                savedScale.y / parentScale.y,
                savedScale.z / parentScale.z
            );

            OnMounted?.Invoke(entity);
        }

        void Dismount(IEntity entity)
        {
            if (rider == null) return;

            OnMoveCancel();
            Transform root = rider.transform;

            root.SetParent(null, true);

            root.position = mountPoint.TransformPoint(new Vector3(0f, 0f, -1.5f)); 

            root.rotation = savedRotation;
            root.localScale = savedScale;

            OnDismounted?.Invoke(entity);

            if (entity is IModel model)
            {
                model.Model.localPosition = new Vector3(0, model.Model.localPosition.y, 0);
            
            }

            rider = null;
            OnMounted = null;
            OnDismounted = null;
        }

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
    }
}
