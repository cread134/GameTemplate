using System;
using UnityEngine;

namespace Core.Vfx
{
    public interface IEffectInstance
    {
        void OnSpawn();
        void Play(Action<GameEffect, GameObject> onFinish);
        void Stop();
    }
}