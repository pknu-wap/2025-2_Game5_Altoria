using System;
using UnityEngine;

namespace GameInteract
{
    public class SpawnInteractComponent : InteractBaseComponent, IDisposable, ISpawnable
    {
        [SerializeField] float respawnTime;
        [SerializeField] GameObject spawnableObject;

        RespawnTimer timer;

        public override void Interact()
        {
            if (spawnableObject != null)
                spawnableObject.SetActive(false);

            timer = new RespawnTimer(respawnTime);
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
