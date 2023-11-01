using UnityEditor;
using UnityEngine;

namespace Moonflow.MFAssetTools.MFMatProcessor
{
    public class MFMatShaderCon : MFMatBoolCon
    {
        public Shader shader;
        public bool missing;
        public override string condName => "Shader";
        private string shaderGUID;
        public override bool Check(Material mat)
        {
            if (mat.shader.name == "Hidden/InternalErrorShader")
            {
                return missing;
            }
            if (shader == null) return false;
            return equal ? mat.shader == shader : mat.shader != shader;
        }
        
        public override void DrawLeft(float width)
        {
            missing = EditorGUILayout.ToggleLeft("Include Missing", missing, GUILayout.Width(width));
        }
        
        public bool Check(string shaderFromMatGuid)
        {
            return equal ? shaderFromMatGuid == shaderGUID : shaderFromMatGuid != shaderGUID;
        }
        
        public override void DrawRight(float width)
        {
            shader = (Shader) EditorGUILayout.ObjectField(shader, typeof(Shader), false, GUILayout.Width(width));
        }

        public override void UpdateRef()
        {
            base.UpdateRef();
            if(shader != null)
                shaderGUID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(shader));
        }
    }
}