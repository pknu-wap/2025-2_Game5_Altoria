using UnityEngine;

namespace GameInteract
{
    [RequireComponent(typeof(Animator))]
    public class InteractableAnimalComponent : InteractBaseComponent,IMovable,IAmbient,IInteractStay
    {
        [Header("Actor Components")]
        [SerializeField] private Animator animator;

        
        const string moveParam = "MoveSpeed";
        const string ambientTrigger = "isAmbient";

        float currentSpeed;
        float threshold = 0.1f;
        float maxSpeed = 2.0f;
        Vector3 lastPosition;

        private void Awake()
        {
            lastPosition = transform.position;
        }

        void FixedUpdate()
        { 
            float rawSpeed = (transform.position - lastPosition).magnitude / Time.fixedDeltaTime;

           
            currentSpeed = Mathf.Lerp(currentSpeed, rawSpeed, Time.fixedDeltaTime * 8f);

        
            if (currentSpeed < threshold)
                currentSpeed = 0f;
            else
                currentSpeed = Mathf.Clamp(currentSpeed, threshold, maxSpeed);

           
            animator.SetFloat(moveParam, currentSpeed);
            lastPosition = transform.position;
        }



        public void MoveTo(Vector3 direction, float speed)
        {
            transform.position += direction * speed * Time.deltaTime;
            animator.SetFloat(moveParam, speed);
        }

        public void Stop()
        {
            animator.SetFloat(moveParam, 0f);
        }

        public void Ambient()
        {
            animator.SetTrigger(ambientTrigger);
        }

        public void HoldInteract()
        {
            throw new System.NotImplementedException();
        }
    }
}
