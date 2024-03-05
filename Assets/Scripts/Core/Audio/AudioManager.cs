using Core.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Audio
{
    public class AudioManager : MonoBehaviour, IAudioManager
    {
        public void OnResourceCreating()
        {
            DontDestroyOnLoad(gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
