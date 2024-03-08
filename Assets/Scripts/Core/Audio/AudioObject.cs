using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Core.Audio
{
    [CreateAssetMenu]
    public class AudioObject : ScriptableObject
    {
        public string key;
        public List<AudioClip> clips;
        public AudioMixerGroup mixerGroup;
        internal AudioClip GetClip()
        {
            return clips[UnityEngine.Random.Range(0, clips.Count)];
        }
    }
}