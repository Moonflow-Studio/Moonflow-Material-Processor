using UnityEditor;
using UnityEngine;

namespace Moonflow.MFAssetTools.MFMatProcessor
{
    public abstract class MFMatFilterCon : ScriptableObject
    {
        public abstract string condName { get; }
        public bool or;
        
        public abstract bool Check(Material mat);

        public void Draw()
        {
            using (new EditorGUILayout.VerticalScope(GUILayout.Width(400), GUILayout.Height(25)))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    or = EditorGUILayout.ToggleLeft("OR | ", or, new GUIStyle(){fontStyle = or ? FontStyle.Bold : FontStyle.Normal, normal = new GUIStyleState(){textColor = or ? Color.white : Color.grey}},GUILayout.Width(40));
                    EditorGUILayout.LabelField(condName,
                        new GUIStyle()
                            { fontStyle = FontStyle.Bold, normal = new GUIStyleState() { textColor = Color.white } },
                        GUILayout.Width(60));
                    DrawSpecial();
                }
            }
        }

        public virtual void DrawSpecial()
        {
        }

        public virtual void UpdateRef()
        {
            
        }
    }
}