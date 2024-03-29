using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Moonflow.MFAssetTools.MFMatProcessor
{
    [Serializable]
    public class MFMatQueueCon : MFMatValueCon
    {
        [SerializeField]public int threshold;
        public override string condName => "队列";
        public override bool Check(Material mat)
        {
            int value = mat.renderQueue;
            switch (comp)
            {
                case CompareFunction.Always:
                    return true;
                case CompareFunction.Equal:
                    return value == threshold;
                case CompareFunction.Greater:
                    return value > threshold;
                case CompareFunction.GreaterEqual:
                    return value >= threshold;
                case CompareFunction.Less:
                    return value < threshold;
                case CompareFunction.LessEqual:
                    return value <= threshold;
                case CompareFunction.NotEqual:
                    return value != threshold;
                case CompareFunction.Never:
                    return false;
                default:
                    return false;
            }
            return false;
        }

        public override void DrawLeft(float width)
        {
            EditorGUILayout.LabelField("队列", GUILayout.Width(width));
        }

        public override void DrawRight(float width)
        {
            threshold = EditorGUILayout.IntField(threshold, GUILayout.Width(width));
        }
    }
}