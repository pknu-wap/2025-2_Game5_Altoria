using Common;
using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameInteract
{
    public class CollectInteractComponent : InteractBaseComponent
    {
        [SerializeField] string objectID;

        bool interactCollTime = false;

        [Header("Respawn")]
        float returnDuration = 20.0f;
        Vector3 orignScale;

        public override void Interact(IEntity entity)
        {
            base.Interact(entity);
            if (interactCollTime)
            {
                Debug.Log("해당 오브젝트 쿨타임");
                return;
            }

            Debug.Log("상호작용!");
            interactCollTime = true;
            CollectTimer timer = new(2);
            orignScale = transform.localScale;
            timer.OnFinished += EndCollect;
        }

        void EndCollect(ITimer timer)
        {
            Debug.Log("상호작용 종료");
            GetComponent<Collider>().enabled = false;
            transform.localScale = Vector3.zero;
            StartCoroutine("ScaleUP");
            List<(CollectGroup, float)> probList = new List<(CollectGroup, float)>();
            var dic = GameDB.GetCollectData(objectID).Value;
            var data = dic[objectID];

            for (int i = 0; i < data.CollectGroup.Count; i++)
                probList.Add((data.CollectGroup[i], data.CollectGroup[i].Probability));

            var item = GameSystem.Random.Pick(probList);
            // TODO: item 인벤토리에 저장

            GameSystem.Life.AddExp<CollectInteractComponent>(10);

            Debug.Log("EndInteract() 호출");
            EndInteract();
        }

        public void SetObjectID(string id)
        {
            objectID = id;
        }

        IEnumerator ScaleUP()
        {
            Debug.Log("ScaleUP!");
            float elapsed = 0f;

            while (elapsed < returnDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / returnDuration;
                transform.localScale = Vector3.Lerp(Vector3.zero, orignScale, Mathf.SmoothStep(0, 1, t));
                yield return null;
            }

            transform.localScale = orignScale; // 정확히 원래 크기로
            GetComponent<Collider>().enabled = true;
            interactCollTime = false;
            Debug.Log("ScaleUP 종료");
        }
    }
}