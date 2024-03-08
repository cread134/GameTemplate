using Core.Logging;
using Core.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Core.Audio
{
    public class AudioManager : MonoBehaviour, IAudioManager
    {
        const int AUDIO_INSTANCE_COUNT = 20;
        bool initialised;
        
        ILoggingService loggingService;
        Queue<AudioInstance> m_AudioInstancePool;
        public void OnResourceCreating()
        {
            DontDestroyOnLoad(gameObject);
            loggingService = ObjectFactory.ResolveService<ILoggingService>();

            GenerateAudioInstances();
            initialised = true;
        }

        public AudioInstance PlaySound(AudioObject audioObject, Vector3 position, AudioSettings settings = null)
        {
            if (!initialised)
            {
                loggingService.LogError("AudioManager not initialised");
                return null;
            }
            settings = settings ?? AudioSettings.Default;
            if (m_AudioInstancePool.Count == 0)
            {
                GenerateAudioInstances();
            }
            var audioInstance = m_AudioInstancePool.Dequeue();
            if (!audioInstance.gameObject.activeSelf)
            {
                audioInstance.gameObject.SetActive(true);
            }
            audioInstance.PlaySound(position, audioObject, settings);
            return audioInstance;
        }

        void GenerateAudioInstances()
        {
            m_AudioInstancePool = new Queue<AudioInstance>();
            for (int i = 0; i < AUDIO_INSTANCE_COUNT; i++)
            {
                var audioInstance = AudioInstance.CreateAudioInstance((instance) =>
                {
                    instance.gameObject.SetActive(false);
                });
                audioInstance.gameObject.transform.SetParent(transform);
                m_AudioInstancePool.Enqueue(audioInstance);
            }
            loggingService.Log($"Generated {AUDIO_INSTANCE_COUNT} audio instances");
        }
    }
}
