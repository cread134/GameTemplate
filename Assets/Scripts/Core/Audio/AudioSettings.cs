namespace Core.Audio
{
    [System.Serializable]
    public class AudioSettings
    {
        public float volume = 1;
        public float pitch = 1;
        public bool loop = false;

        public static AudioSettings Default
        {
            get
            {
                return new AudioSettings
                {
                    volume = 1,
                    pitch = 1,
                    loop = false
                };
            }
        }
    }
}
