using System;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Moonflow.MFAssetTools.MFMatProcessor
{
    [Serializable]
    public class MFMatNameCon : MFMatBoolCon
    {
        [SerializeField]public string matNameRegex;
        public override string condName => "Mat Name";
        public override bool Check(Material mat)
        {
            string pattern = matNameRegex;
            Regex reg = new Regex(pattern);
            bool isMatch = reg.IsMatch(mat.name);
            return equal ? isMatch : !isMatch;
        }

        public override void DrawLeft(float width)
        {
            EditorGUILayout.LabelField("  (Regex)", GUILayout.Width(width));
        }

        public override void DrawRight(float width)
        {
            matNameRegex = EditorGUILayout.TextField(matNameRegex, GUILayout.Width(width));
        }
    }
}