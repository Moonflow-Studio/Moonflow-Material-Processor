using System;
using UnityEditor;
using UnityEngine;

namespace Moonflow.MFAssetTools.MFMatProcessor
{    
    [Serializable]
    public abstract class MFMatBoolCon : MFMatFilterCon
    {
        [SerializeField]public bool equal;

        public override void DrawStandard()
        {
            base.DrawStandard();
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
                DrawRight(120);
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