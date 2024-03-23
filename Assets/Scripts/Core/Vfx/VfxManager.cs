using Core.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Vfx
{
    internal class VfxManager : MonoBehaviour, IVfxManager
    {
        public void OnResourceCreating()
        {         
        }

        public void PlayEffect(GameEffect gameEffect, Vector3 position, Quaternion rotation)
        {
            throw new System.NotImplementedException();
        }

        public void PlayEffect(GameEffect gameEffect, Vector3 position)
        {
            throw new System.NotImplementedException();
        }
    }
}
