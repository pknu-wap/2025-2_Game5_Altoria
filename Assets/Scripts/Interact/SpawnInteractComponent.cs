using System;
using UnityEngine;

namespace GameInteract
{
    public class SpawnInteractComponent : InteractBaseComponent, IDisposable, IInteractSpawnable
    {
        [SerializeField] float respawnTime;
        [SerializeField] GameObject spawnableObject;

        RespawnTimer timer = new();

        public override void Interact(IEntity entity)
        {
            if (spawnableObject != null)
                spawnableObject.SetActive(false);

            timer.SetTimer(respawnTime);
            timer.OnFinished += OnRespawn;
        }

        void OnRespawn(ITimer timer)
        {
            spawnableObject.SetActive(true);

            this.timer.OnFinished -= OnRespawn;
            this.timer = null;
        }

        public void Dispose()
        {
            if (timer != null)
            {
                timer.OnFinished -= OnRespawn;
                timer = null;
            }
        }
    }
}
