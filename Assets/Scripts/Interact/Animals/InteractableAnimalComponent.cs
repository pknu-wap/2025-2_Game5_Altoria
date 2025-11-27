using Common;
using GameData;
using GameUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameInteract
{
    [RequireComponent(typeof(Animator))]
    public class InteractableAnimalComponent : InteractBaseComponent,IMovable,IAmbient,IInteractStay
    {
        [Header("Actor Components")]
        [SerializeField] private Animator animator;

        [Header("Item ID")]
        [SerializeField] string itemID;

        [Header("CollTime")]
        bool interactCoolTime = false;
        float coollTimeDuration = 20.0f;

        const string moveParam = "MoveSpeed";
        const string ambientBool = "isAmbient";

        float currentSpeed;
        float threshold = 0.1f;
        float maxSpeed = 2.0f;
        Vector3 lastPosition;
        bool isAmbient = false;
        private void Awake()
        {
            lastPosition = transform.position;
        }

        void FixedUpdate()
        { 
            float rawSpeed = (transform.position - lastPosition).magnitude / Time.fixedDeltaTime;

            currentSpeed = Mathf.Lerp(currentSpeed, rawSpeed, Time.fixedDeltaTime * 8f);
        
            if (currentSpeed < threshold) currentSpeed = 0f;
            else currentSpeed = Mathf.Clamp(currentSpeed, threshold, maxSpeed);

            if(currentSpeed>0&&isAmbient)
            {
                isAmbient = false;
                animator.SetBool(ambientBool, isAmbient);
            }
            animator.SetFloat(moveParam, currentSpeed);
            lastPosition = transform.position;
        }


        public override void Interact(IEntity entity)
        {
            base.Interact(entity);
            if (interactCoolTime)
                return;

            interactCoolTime = true;
            CollectTimer timer = new(2);
            timer.OnFinished += EndCollect;
        }

        void EndCollect(ITimer timer)
        {
            GetComponent<Collider>().enabled = false;
            EndInteract();
            StartCoroutine("CoolTime");

            List<(CollectGroup, float)> probList = new List<(CollectGroup, float)>();
            var dic = GameDB.GetCollectData(itemID).Value;
            var data = dic[itemID];

            for (int i = 0; i < data.CollectGroup.Count; i++)
                probList.Add((data.CollectGroup[i], data.CollectGroup[i].Probability));

            GameSystem.Life.AddExp<CollectInteractComponent>(10);

            var equipData = GameSystem.Inventory.GetEquipItem(Type);
            int bous = 0;
            if (equipData != null)
            {
                bous = GameDB.GetUpgradeData(equipData.Level).Bous;
            }
            var item = GameSystem.Random.Pick(probList, bous);

            var popUp = Manager.UI.ShowPopup<GetItemPopUp>();
            popUp.SetData(itemID, item.Count);


        }

        IEnumerator CoolTime()
        {
            yield return new WaitForSeconds(coollTimeDuration);

            GetComponent<Collider>().enabled = true;
            interactCoolTime = false;
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
            isAmbient = true;
            animator.SetBool(ambientBool,isAmbient);
        }

        public void HoldInteract()
        {
            throw new System.NotImplementedException();
        }
    }
}
