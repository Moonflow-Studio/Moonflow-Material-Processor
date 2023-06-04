using UnityEditor;
using UnityEngine;

namespace Moonflow.MFAssetTools.MFMatProcessor
{
    public class MFMatRefCon : MFMatFilterCon
    {
        public bool accurate;
        public string name;
        public Object obj;
        public override string condName => "Reference";
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

        public override void DrawSpecial()
        {
            base.DrawSpecial();
            using (new EditorGUILayout.HorizontalScope())
            {
                accurate = EditorGUILayout.ToggleLeft("", accurate, GUILayout.Width(20));
                EditorGUILayout.LabelField("Accurate", accurate ? EditorStyles.boldLabel : EditorStyles.label,GUILayout.Width(60));
                EditorGUILayout.LabelField(" / " ,GUILayout.Width(10));
                EditorGUILayout.LabelField("Including", accurate ? EditorStyles.label : EditorStyles.boldLabel ,GUILayout.Width(60));
                EditorGUILayout.LabelField(" " ,GUILayout.Width(31));
                if (accurate)
                {
                    obj = EditorGUILayout.ObjectField(obj, typeof(Object), false, GUILayout.Width(80));
                }
                else
                {
                    name = EditorGUILayout.TextField(name, GUILayout.Width(80));
                }
            }
        }
    }
}