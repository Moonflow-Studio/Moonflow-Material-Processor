using Moonflow.Core;
using UnityEditor;
using UnityEngine;

namespace Moonflow.MFAssetTools.MFMatProcessor
{
    public class MFMatKeywordSetter : MFMatDataSetter<string>
    {
        [SerializeField]public bool SetToEnable;
        [SerializeField]public string keyword;
        public override string displayName => "Keyword";
        public override void Draw()
        {
            using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(400)))
            {
                EditorGUILayout.LabelField(displayName, EditorStyles.boldLabel);
                MFEditorUI.DivideLine(Color.gray);
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button(SetToEnable ? "开启" : "关闭", GUILayout.Width(40)))
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
            //check if keyword is declared in material
            var keywords = mat.shaderKeywords;
            bool hasKeyword = false;
            for (int i = 0; i < keywords.Length; i++)
            {
                if (keywords[i] == keyword)
                {
                    hasKeyword = true;
                }
            }
            
            if (!hasKeyword)
            {
                Debug.LogError($"Keyword {keyword} is not declared in material {mat.name}");
                return;
            }
            
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