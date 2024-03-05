using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Extension
{
    public class DocumentFactory : MonoBehaviour
    {

        public static UIDocument CreateDocument(string name, VisualTreeAsset visualTree, Transform parent)
        {
            var canvas = new GameObject(name);
            canvas.layer = LayerMask.NameToLayer("UI");
            canvas.transform.SetParent(parent);
            var document = canvas.AddComponent<UIDocument>();
            document.visualTreeAsset = visualTree;
            return document;
        }
    }
}
