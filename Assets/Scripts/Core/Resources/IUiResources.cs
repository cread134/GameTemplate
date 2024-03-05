using UnityEngine.UIElements;

namespace Core.Resources
{
    public interface IUiResources : IResourceInstance
    {
        public PanelSettings PanelSettings { get; }
    }
}