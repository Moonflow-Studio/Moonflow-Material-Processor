using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Moonflow.MFAssetTools.MFMatProcessor
{
    [Serializable]
    public class MFPathCon : MFMatFilterCon
    {
        [SerializeField]public bool exclude;
        [SerializeField]public string path;
        public override string condName => "路径";
        public override bool Check(Material mat)
        {
            string relative = AssetDatabase.GetAssetPath(mat);
            return exclude ? !relative.Contains(path) : relative.Contains(path);
        }

        public override void DrawStandard()
        {
            // exclude = EditorGUILayout.ToggleLeft("", exclude, GUILayout.Width(20));
            if (GUILayout.Button(exclude ? "排除" : "包括", GUILayout.Width(120)))
            {
                exclude = !exclude;
            }
            // EditorGUILayout.LabelField("Include", exclude ? EditorStyles.label : EditorStyles.boldLabel,GUILayout.Width(50));
            // EditorGUILayout.LabelField(" / " ,GUILayout.Width(10));
            // EditorGUILayout.LabelField("Exclude", exclude ? EditorStyles.boldLabel : EditorStyles.label ,GUILayout.Width(50));
            // EditorGUILayout.LabelField("", GUILayout.Width(40));
            // EditorGUILayout.LabelField("");
            // EditorGUILayout.LabelField("");
            path = EditorGUILayout.TextField(path, GUILayout.Width(160));
        }
    }
}