using System;
using UnityEngine;

namespace Player.Interaction
{
    public interface IPlayerController
    {
        public void Activate();

        public EventHandler<Vector2> LookDelta { get; set; }
        public EventHandler<Vector2> MoveDelta { get; set; }

        public EventHandler OnMainDown { get; set; }
        public EventHandler OnMainUp { get; set; }

        public EventHandler OnSecondaryDown { get; set; }
        public EventHandler OnSecondaryUp { get; set; }

        public EventHandler OnJumpDown { get; set; }
        public EventHandler OnJumpUp { get; set; }
    }
}