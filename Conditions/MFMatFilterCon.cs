using System;
using UnityEditor;
using UnityEngine;

namespace Moonflow.MFAssetTools.MFMatProcessor
{
    [Serializable]
    public abstract class MFMatFilterCon : ScriptableObject
    {
        public abstract string condName { get; }
        [SerializeField]public bool or;
        
        public abstract bool Check(Material mat);

        public void Draw()
        {
            using (new EditorGUILayout.VerticalScope(GUILayout.Width(400), GUILayout.Height(25)))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    or = EditorGUILayout.ToggleLeft("或 | ", or, new GUIStyle(){fontStyle = or ? FontStyle.Bold : FontStyle.Normal, normal = new GUIStyleState(){textColor = or ? Color.white : Color.grey}},GUILayout.Width(40));
                    EditorGUILayout.LabelField(condName,
                        new GUIStyle()
                            { fontStyle = FontStyle.Bold, normal = new GUIStyleState() { textColor = Color.white } },
                        GUILayout.Width(60));
                    DrawStandard();
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    DrawSpecial();
                }
            }
        }

        public virtual void DrawSpecial()
        {
        }

        public virtual void DrawStandard()
        {
        }

        public virtual void UpdateRef()
        {
            
        }
    }
}