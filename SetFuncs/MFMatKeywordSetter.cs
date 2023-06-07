using UnityEditor;
using UnityEngine;

namespace Moonflow.MFAssetTools.MFMatProcessor
{
    public class MFMatKeywordSetter : MFMatDataSetter<string>
    {
        public bool SetToEnable;
        public string keyword;
        public override string displayName => "Keyword";
        public override void Draw()
        {
            using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(400)))
            {
                EditorGUILayout.LabelField(displayName, EditorStyles.boldLabel);
                MFEditorUI.DivideLine(Color.gray);
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button(SetToEnable ? "Open" : "Close", GUILayout.Width(40)))
                    {
                        SetToEnable = !SetToEnable;
                    }
                    float normalLabelWidth = EditorGUIUtility.labelWidth;
                    EditorGUIUtility.labelWidth = 75;
                    EditorGUILayout.TextField("Keyword", keyword);
                    EditorGUIUtility.labelWidth = normalLabelWidth;
                }
            }
        }

        public override void DisplayManualData()
        {
            // throw new System.NotImplementedException();
        }

        public override void SetData(Material mat)
        {
            if (SetToEnable)
            {
                mat.EnableKeyword(keyword);
            }
            else
            {
                mat.DisableKeyword(keyword);
            }
        }
    }
}