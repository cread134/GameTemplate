using UnityEngine;

namespace Player.Interaction
{
    internal interface IPlayerMouseLook
    {
        public void UpdateDelta(object sender, Vector2 delta);
    }
}