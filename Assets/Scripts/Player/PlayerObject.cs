using Core.Logging;
using Core.Resources;
using Player.Interaction;
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

        private void Awake()
        {
            _loggingService = ObjectFactory.ResolveService<ILoggingService>();

            playerController.Activate();
            InitBehaviours();
        }

        void InitBehaviours()
        {
            var behaviours = RecurseChildrenForBehaviours(transform);
            foreach (var behaviour in behaviours)
            {
                Debug.Log($"Initialising player behaviour: {behaviour}");
                behaviour.OnBehaviourInit(playerController);
            }
        }

        List<IPlayerBehaviour> RecurseChildrenForBehaviours(Transform root)
        {
            var behaviours = root.GetComponentsInChildren<IPlayerBehaviour>();
            return behaviours.ToList();
        }
    }
}