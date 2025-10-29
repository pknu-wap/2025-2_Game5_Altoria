using Common;
using GameData;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Define;
using static UnityEngine.Rendering.DebugUI;

namespace GameInteract
{
    public class CollectInteractComponent : InteractBaseComponent
    {
        [SerializeField] protected Content collectType = Content.None;
        [SerializeField] string objectID;

        public override void Interact()
        {
            CollectTimer timer = new(2);
            timer.OnFinished += EndCollect;
        }

        void EndCollect(ITimer timer)
        {
            Debug.Log($"{GetType()} : {collectType.ToString()} Á¾·á.");

            List<(CollectGroup, float)> probList = new List<(CollectGroup, float)>();
            var dic = GameDB.GetCollectData(objectID).Value;
            var data = dic[objectID];

            for (int i = 0; i < data.CollectGroup.Count; i++)
                probList.Add((data.CollectGroup[i], data.CollectGroup[i].Probability));

            var item = GameSystem.Random.Pick(probList);
            Debug.Log($"{GetType()} : {objectID} {item.Count}°³ Å‰µæ!");

            GameSystem.Life.AddExp<CollectInteractComponent>(10);

            EndInteract();
        }

        public void SetObjectID(string id)
        {
            objectID = id;
        }

        public void SetCollectType(Content type)
        {
            collectType = type;
        }
    }
}