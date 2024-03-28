using Core.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Vfx
{
    internal class VfxManager : MonobehaviourResourceInstance, IVfxManager
    {
        const int MAX_EFFECTS = 10;
        const int MAX_EFFECT_POOLS = 10;

        Dictionary<GameEffect, Queue<GameObject>> effectPools = new Dictionary<GameEffect, Queue<GameObject>>();
        Dictionary<GameEffect, int> poolUsage = new Dictionary<GameEffect, int>();

        internal override void OnCreating()
        {
        }
        public void PlayEffect(GameEffect gameEffect, Vector3 position)
        {
            PlayEffect(gameEffect, position, Quaternion.identity);
        }

        public void PlayEffect(GameEffect gameEffect, Vector3 position, Quaternion rotation)
        {
            if (!effectPools.ContainsKey(gameEffect))
            {
                AddPool(gameEffect);
            }
            poolUsage[gameEffect] = Mathf.Clamp(poolUsage[gameEffect] + 1, 0, 50);

            var effect = effectPools[gameEffect].Dequeue();
            effect.transform.position = position;
            effect.transform.rotation = rotation;
            effect.SetActive(true);
            var effectInstances = effect.GetComponents<IEffectInstance>();
            foreach (var effectInstance in effectInstances)
            {
                effectInstance.Play((effect, effectInstance) =>
                {
                    if (effectInstance != null)
                    {
                        effectInstance.SetActive(false);
                    }
                });
            }
            effectPools[gameEffect].Enqueue(effect);
        }

        void AddPool(GameEffect gameEffect)
        {
            if (effectPools.ContainsKey(gameEffect))
            {
                return;
            }
            if (effectPools.Count >= MAX_EFFECT_POOLS)
            {
                var poolToRemove = effectPools.OrderBy(x => poolUsage[x.Key]).First().Key;
                RemovePool(poolToRemove);
            }
            effectPools.Add(gameEffect, new Queue<GameObject>());
            for (int i = 0; i < MAX_EFFECTS; i++)
            {
                var effect = Instantiate(gameEffect.effectPrefab, transform);
                var effectInstances = effect.GetComponents<IEffectInstance>();
                foreach (var effectInstance in effectInstances)
                {
                    effectInstance.OnSpawn();
                }
                effect.SetActive(false);
                effectPools[gameEffect].Enqueue(effect);
            }

            poolUsage.Add(gameEffect, 0);
        }
        void RemovePool(GameEffect gameEffect) 
        { 
            if (effectPools.ContainsKey(gameEffect))
            {
                var pool = effectPools[gameEffect];
                foreach (var effect in pool)
                {
                    Destroy(effect);
                }
                effectPools.Remove(gameEffect);

                poolUsage.Remove(gameEffect);
            }
        }
    }
}
