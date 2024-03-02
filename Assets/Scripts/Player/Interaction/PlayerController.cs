using Core.Logging;
using Core.Resources;
using UnityEngine;

namespace Player.Interaction
{
    public class PlayerController : MonoBehaviour
    {
        private ILoggingService loggingService;

        void Awake()
        {
            loggingService = ObjectFactory.ResolveService<ILoggingService>();
            loggingService.Log("PlayerController.Awake");
        }
    }
}