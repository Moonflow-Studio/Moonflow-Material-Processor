using System;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Moonflow.MFAssetTools.MFMatProcessor
{
    [Serializable]
    public class MFMatNameCon : MFMatBoolCon
    {
        public bool shaderName;
        public string nameRegex;
        public override string condName => "名字匹配";
        public override bool Check(Material mat)
        {
            string pattern = nameRegex;
            Regex reg = new Regex(pattern);
            bool isMatch = reg.IsMatch(shaderName ? mat.shader.name : mat.name);
            return equal ? isMatch : !isMatch;
        }

        public override void DrawLeft(float width)
        {
            if (GUILayout.Button((shaderName?"Shader":"材质")+"名(正则)", GUILayout.Width(width)))
            {
                shaderName = !shaderName;
            }
        }

        public override void DrawRight(float width)
        {
            nameRegex = EditorGUILayout.TextField(nameRegex, GUILayout.Width(width));
        }
    }
}