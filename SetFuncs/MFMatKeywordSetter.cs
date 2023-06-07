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
                    SetToEnable = EditorGUILayout.ToggleLeft(SetToEnable ? "Open" : "Close", SetToEnable, GUILayout.Width(100));
                    float normalLabelWidth = EditorGUIUtility.labelWidth;
                    EditorGUIUtility.labelWidth = 75;
                    EditorGUILayout.TextField("Set Keyword", keyword);
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