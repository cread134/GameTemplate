using Player.PlayerResources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Player
{
    public class GameUiView : MonoBehaviour
    {
        private UIDocument document;

        public bool ShowUI
        {
            get => document.rootVisualElement.visible;
            set => document.rootVisualElement.visible = value;
        }

        private void Awake()
        {
            document = GetComponent<UIDocument>();
        }
    }
}
