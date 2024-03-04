using Player.Interaction;

namespace Player.PlayerResources
{
    public interface IPlayerBehaviour
    {
        public void OnBehaviourInit(IPlayerController playerController, IPlayerResources playerResources);
        public void StartBehaviour();
    }
}