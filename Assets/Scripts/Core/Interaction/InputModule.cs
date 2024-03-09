using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class InputModule : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
