using Core.Resources;
using UnityEngine;

namespace Core.Vfx
{
    public interface IVfxManager
    {
        public void PlayEffect(GameEffect gameEffect, Vector3 position, Quaternion rotation);
        public void PlayEffect(GameEffect gameEffect, Vector3 position);
    }
}