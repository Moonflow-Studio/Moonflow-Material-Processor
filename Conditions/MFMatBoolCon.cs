using UnityEditor;
using UnityEngine;

namespace Moonflow.MFAssetTools.MFMatProcessor
{
    public abstract class MFMatBoolCon : MFMatFilterCon
    {
        public bool equal;

        public override void DrawSpecial()
        {
            base.DrawSpecial();
            using (new EditorGUILayout.HorizontalScope())
            {
                DrawLeft(120);
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                GUI.color = equal ? Color.green : Color.yellow;
                if (GUILayout.Button(equal ? "=" : "≠",GUILayout.Width(30)))
                {
                    equal = !equal;
                }
                GUI.color = Color.white;
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                DrawRight(100);
            }
        }

        public virtual void DrawLeft(float width)
        {
            EditorGUILayout.LabelField("",GUILayout.Width(width));
        }

        public virtual void DrawRight(float width)
        {
            EditorGUILayout.LabelField("",GUILayout.Width(width));
        }
    }
}