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

        public override void Interact()
        {
            if (interactCollTime)
                return;

            interactCollTime = true;
            CollectTimer timer = new(2);
            orignScale = transform.localScale;
            timer.OnFinished += EndCollect;
        }

        void EndCollect(ITimer timer)
        {
            GetComponent<Collider>().enabled = false;
            transform.localScale = Vector3.zero;

            StartCoroutine("ScaleUP");

            List<(CollectGroup, float)> probList = new List<(CollectGroup, float)>();
            var dic = GameDB.GetCollectData(objectID).Value;
            var data = dic[objectID];

            for (int i = 0; i < data.CollectGroup.Count; i++)
                probList.Add((data.CollectGroup[i], data.CollectGroup[i].Probability));

            // TODO: 현재 진행 중인 상호작용 타입에 대한 장착된 아이템 가져오기
            var item = GameSystem.Random.Pick(probList, GameDB.GetUpgradeData(0).Bous);
            // TODO: item 인벤토리에 저장

            GameSystem.Life.AddExp<CollectInteractComponent>(10);

            EndInteract();
        }

        public void SetObjectID(string id)
        {
            objectID = id;
        }

        IEnumerator ScaleUP()
        {
            float elapsed = 0f;

            while (elapsed < returnDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / returnDuration;
                transform.localScale = Vector3.Lerp(Vector3.zero, orignScale, Mathf.SmoothStep(0, 1, t));
                yield return null;
            }

            transform.localScale = orignScale;
            GetComponent<Collider>().enabled = true;
            interactCollTime = false;
        }
    }
}