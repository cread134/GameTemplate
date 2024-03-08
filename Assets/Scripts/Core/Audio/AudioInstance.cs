using Core.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Core.Audio
{
    public class AudioInstance : MonoBehaviour
    {
        private AudioSource _audioSource;
        private EventHandler<AudioInstance> _onSoundFinished;

        public static AudioInstance CreateAudioInstance(Action<AudioInstance> soundFinishCallback)
        {
            GameObject audioInstance = new GameObject("AudioInstance");
            var audioInstanceScript = audioInstance.AddComponent<AudioInstance>();
            audioInstanceScript._audioSource = audioInstance.AddComponent<AudioSource>();
            audioInstanceScript._audioSource.playOnAwake = false;
            audioInstanceScript._audioSource.spatialBlend = 1;
            audioInstanceScript._onSoundFinished += (obj, instance) => soundFinishCallback(instance);
            return audioInstanceScript;
        }

        public void PlaySound(Vector3 position, AudioObject audioObject, AudioSettings audioSettings)
        {
            if(_audioSource == null)
            {
                return;
            }

            StopSound();

            var clip = audioObject.GetClip();
            _audioSource.clip = clip;
            _audioSource.volume = audioSettings.volume;
            _audioSource.pitch = audioSettings.pitch;
            _audioSource.loop = audioSettings.loop;
            _audioSource.outputAudioMixerGroup = audioObject.mixerGroup;
            transform.position = position;

            _audioSource.Play();
            if(!audioSettings.loop)
            {
                _soundFinishCoroutine = StartCoroutine(WaitForSoundToFinish());
            }
        }

        Coroutine _soundFinishCoroutine;
        IEnumerator WaitForSoundToFinish()
        {
            yield return new WaitUntil(() => !_audioSource.isPlaying);
            _onSoundFinished?.Invoke(this, this);
        }

        public void StopSound()
        {
            if(_audioSource == null)
            {
                return;
            }

            if (_soundFinishCoroutine != null)
            {
                StopCoroutine(_soundFinishCoroutine);
            }

            if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
                _onSoundFinished?.Invoke(this, this);
            }
        }
    }
}
