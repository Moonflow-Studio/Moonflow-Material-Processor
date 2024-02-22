using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Moonflow.MFAssetTools.MFMatProcessor
{
    [Serializable]
    public class MFMatRefCon : MFMatFilterCon
    {
        [SerializeField]public bool accurate;
        [SerializeField]public string name;
        [SerializeField]public Object obj;
        public override string condName => "引用";
        public override bool Check(Material mat)
        {
            //getpath of the object
            string matPath = AssetDatabase.GetAssetPath(obj);
            //get all paths of dependencies
            string[] paths = AssetDatabase.GetDependencies(matPath, accurate);
            //check if any of the paths match the name when accurate is true, else check if any of the paths contains the name
            foreach (string s in paths)
            {
                if (!accurate)
                {
                    if (s.Contains(name))
                    {
                        return true;
                    }
                }
                else
                {
                    //get path of the object
                    string refPath = AssetDatabase.GetAssetPath(obj);
                    if (s == refPath)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override void DrawStandard()
        {
            base.DrawStandard();
            using (new EditorGUILayout.HorizontalScope())
            {
                // accurate = EditorGUILayout.ToggleLeft("", accurate, GUILayout.Width(20));
                if (GUILayout.Button(accurate ? "精确匹配" : "包含名字",GUILayout.Width(120)))
                {
                    accurate = !accurate;
                }
                // EditorGUILayout.LabelField("精确匹配", accurate ? EditorStyles.boldLabel : EditorStyles.label,GUILayout.Width(50));
                // EditorGUILayout.LabelField(" / " ,GUILayout.Width(10));
                // EditorGUILayout.LabelField("包含名字", accurate ? EditorStyles.label : EditorStyles.boldLabel ,GUILayout.Width(50));
                // EditorGUILayout.LabelField("", GUILayout.Width(30));
                if (accurate)
                {
                    obj = EditorGUILayout.ObjectField(obj, typeof(Object), false, GUILayout.Width(160));
                }
                else
                {
                    name = EditorGUILayout.TextField(name, GUILayout.Width(160));
                }
            }
        }
    }
}