using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Moonflow.MFAssetTools.MFMatProcessor
{
    public class MFMatShaderSetter : MFMatDataSetter<Shader>
    {
        public override string displayName => "Shader";

        [SerializeField]public Shader newShader;

        public override bool isLegal()
        {
            if(newShader == null || !newShader.isSupported)
                return false;
            return true;
        }

        public override void Draw()
        {
            using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(400)))
            {
                EditorGUILayout.LabelField(displayName, EditorStyles.boldLabel);
                MFEditorUI.DivideLine(Color.gray);

                float normalLabelWidth = EditorGUIUtility.labelWidth;
                using (new EditorGUILayout.HorizontalScope(GUILayout.Width(400)))
                {
                    EditorGUIUtility.labelWidth = 50;
                    GUI.color = ReferenceEquals(newShader, null) ? Color.red : Color.white;
                    newShader = EditorGUILayout.ObjectField("新Shader", newShader, typeof(Shader), false) as Shader;
                    GUI.color = Color.white;
                    EditorGUIUtility.labelWidth = normalLabelWidth;
                }
            }
        }
    
        public override void SetData(Material mat)
        {
            mat.shader = newShader;
        }

        public override void DisplayManualData()
        {
            // manualData = EditorGUILayout.ObjectField("设置值", manualData, typeof(Shader), false) as Shader;
        }
    }
}