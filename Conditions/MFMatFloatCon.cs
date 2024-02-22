using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Moonflow.MFAssetTools.MFMatProcessor
{
    [Serializable]
    public class MFMatFloatCon : MFMatValueCon
    {
        [SerializeField]public string name;
        [SerializeField]public float threshold;
        
        public override string condName => "浮点参数";

        public override bool Check(Material mat)
        {
            if (mat.HasFloat(name))
            {
                float value = mat.GetFloat(name);
                switch (comp)
                {
                    case CompareFunction.Always:
                        return true;
                    case CompareFunction.Equal:
                        return Math.Abs(value - threshold) < 0.001f;
                    case CompareFunction.Greater:
                        return value > threshold;
                    case CompareFunction.GreaterEqual:
                        return value >= threshold;
                    case CompareFunction.Less:
                        return value < threshold;
                    case CompareFunction.LessEqual:
                        return value <= threshold;
                    case CompareFunction.NotEqual:
                        return Math.Abs(value - threshold) > 0.001f;
                    case CompareFunction.Never:
                        return false;
                    default:
                        return false;
                }
            }

            return false;
        }


        public override void DrawLeft(float width)
        {
            name = EditorGUILayout.TextField(name, GUILayout.Width(width));
        }

        public override void DrawRight(float width)
        {
            threshold = EditorGUILayout.FloatField(threshold, GUILayout.Width(width));
        }
    }
}