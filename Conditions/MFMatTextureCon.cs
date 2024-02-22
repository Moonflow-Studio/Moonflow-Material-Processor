using System;
using UnityEditor;
using UnityEngine;

namespace Moonflow.MFAssetTools.MFMatProcessor
{
    [Serializable]
    public class MFMatTextureCon : MFMatBoolCon
    {
        [SerializeField]public bool any;
        [SerializeField]public string name;
        [SerializeField]public Texture2D texture;
        public override string condName => "纹理";

        private string textureGUID;
        public override bool Check(Material mat)
        {
            //get all texture properties on the material
            int count = ShaderUtil.GetPropertyCount(mat.shader);
            for (int i = 0; i < count; i++)
            {
                if (ShaderUtil.GetPropertyType(mat.shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
                {
                    string propertyName = ShaderUtil.GetPropertyName(mat.shader, i);
                    if (any)
                    {
                        if (mat.GetTexture(propertyName) == texture)
                        {
                            return equal;
                        }
                    }
                    else
                    {
                        if (propertyName == name)
                        {
                            if (equal)
                            {
                                return mat.GetTexture(propertyName) == texture;
                            }
                            else
                            {
                                return mat.GetTexture(propertyName) != texture;
                            }
                        }
                    }
                }
            }
            return false;
        }
        public bool Check(string[] texfromMatGuid)
        {
            if (any)
            {
                foreach (string s in texfromMatGuid)
                {
                    if (s == textureGUID)
                    {
                        return equal;
                    }
                }
            }
            else
            {
                foreach (string s in texfromMatGuid)
                {
                    if (s == textureGUID)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override void DrawLeft(float width)
        {
            any = EditorGUILayout.ToggleLeft("任意", any, GUILayout.Width(any ? width : 40));
            if (!any)
            {
                name = EditorGUILayout.TextField(name, GUILayout.Width(width - 43));
            }
        }
        
        public override void DrawRight(float width)
        {
            texture = (Texture2D) EditorGUILayout.ObjectField(texture, typeof(Texture2D), false, GUILayout.Width(width));
        }

        public override void UpdateRef()
        {
            base.UpdateRef();
            if(texture!=null)
                //get the GUID of the texture
                textureGUID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(texture));
        }
    }
}