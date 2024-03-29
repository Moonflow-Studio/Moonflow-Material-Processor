using System;
using UnityEditor;
using UnityEngine;

namespace Moonflow.MFAssetTools.MFMatProcessor
{
    [Serializable]

    public class MFMatKeywordCon : MFMatBoolCon
    {
        [SerializeField]public string keyword;
        [SerializeField]public bool needEnabled;
        public override string condName => "Keyword";
        
        public override bool Check(Material mat)
        {
            //get all keywords on material
            string[] keywords = mat.shaderKeywords;
            //check if keyword is in the list
            bool hasKeyword = false;
            foreach (string s in keywords)
            {
                if (s == keyword)
                {
                    hasKeyword = true;
                    break;
                }
            }
            //get enabled keywords
            bool enabled = mat.IsKeywordEnabled(keyword);
            //check if keyword is enabled
            if (!hasKeyword) return false;
            if (needEnabled)
            {
                return enabled;
            }
            else
            {
                return true;
            }
        }
        
        public override void DrawLeft(float width)
        {
            needEnabled = EditorGUILayout.ToggleLeft("必须已启用", needEnabled, GUILayout.Width(width));
        }
        
        public override void DrawRight(float width)
        {
            keyword = EditorGUILayout.TextField(keyword, GUILayout.Width(width));
        }
    }
}