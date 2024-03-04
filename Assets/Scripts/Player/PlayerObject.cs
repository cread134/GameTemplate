using Core.Logging;
using Core.Resources;
using Player.Interaction;
using Player.PlayerResources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player
{
    public class PlayerObject : MonoBehaviour
    {
        private ILoggingService _loggingService;

        [Header("Dependencies")]
        [SerializeField] private PlayerController playerController;
        [SerializeField] private DefaultPlayerResources playerResources; 

        private void Awake()
        {
            _loggingService = ObjectFactory.ResolveService<ILoggingService>();

            var behaviours = RecurseChildrenForBehaviours(transform);

            playerResources.ConfigureResources(behaviours);
            playerController.Activate();

            InitBehaviours(behaviours);
            StartBehaviours(behaviours);
        }

        void InitBehaviours(List<IPlayerBehaviour> behaviours)
        {
            foreach (var behaviour in behaviours)
            {
                _loggingService.Log($"Initialising player behaviour: {behaviour}");
                behaviour.OnBehaviourInit(playerController, playerResources);
            }
        }

        void StartBehaviours(List<IPlayerBehaviour> behaviours)
        {
            foreach (var behaviour in behaviours)
            {
                _loggingService.Log($"Starting player behaviour: {behaviour}");
                behaviour.StartBehaviour();
            }
        }

        List<IPlayerBehaviour> RecurseChildrenForBehaviours(Transform root)
        {
            var behaviours = root.GetComponentsInChildren<IPlayerBehaviour>();
            return behaviours.ToList();
        }
    }
}