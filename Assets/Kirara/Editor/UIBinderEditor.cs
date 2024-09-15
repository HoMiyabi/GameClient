using UnityEditor;
using UnityEngine;

namespace Kirara
{
    [CustomEditor(typeof(UIBinder))]
    public class UIBinderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var script = (UIBinder)target;
            if (GUILayout.Button("生成"))
            {
                script.EditorGenerateUI();
            }
            if (GUILayout.Button("绑定"))
            {
                script.EditorBindUI();
            }
            base.OnInspectorGUI();
        }
    }
}