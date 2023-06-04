using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Moonflow.MFAssetTools.MFMatProcessor
{
    public class MFMatVectorCon: MFMatValueCon
    {
        public enum ChannelType
        {
            x,y,z,w
        }
        public ChannelType channel;
        public string name;
        public float threshold;
        public override string condName => "Vector";
        public override bool Check(Material mat)
        {
            
            if (mat.HasVector(name))
            {
                Vector4 vector = mat.GetVector(name);
                float value = 0;
                switch (channel)
                {
                    case ChannelType.x :
                        value = vector.x;
                        break;
                    case ChannelType.y:
                        value = vector.y;
                        break;
                    case ChannelType.z:
                        value = vector.z;
                        break;
                    case ChannelType.w:
                        value = vector.w;
                        break;
                }
                
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
            name = EditorGUILayout.TextField(name, GUILayout.Width(width-30));
            channel = (ChannelType)EditorGUILayout.EnumPopup(channel, GUILayout.Width(27));
        }

        public override void DrawRight(float width)
        {
            threshold = EditorGUILayout.FloatField(threshold, GUILayout.Width(width));
        }
    }
}