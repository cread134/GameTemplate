using System;

namespace Core.SceneManagement
{
    public class SceneLoadingHook
    {
        public string message;
        public Func<bool> condition;
    }
}
