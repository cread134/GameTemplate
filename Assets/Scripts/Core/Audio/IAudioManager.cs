using Core.Resources;
using UnityEngine;

namespace Core.Audio
{
    public interface IAudioManager : IResourceInstance
    {
        public AudioInstance PlaySound(AudioObject audioObject, Vector3 position, AudioSettings settings = null);
    }
}