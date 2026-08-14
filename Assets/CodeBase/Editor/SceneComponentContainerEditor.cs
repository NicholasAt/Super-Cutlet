using CodeBase.Logic;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(SceneComponentContainer))]
    public class SceneComponentContainerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var container = (SceneComponentContainer)target;
            if (GUILayout.Button("Collect Components"))
            {
                container.CollectComponents();
            }
        }
    }
}