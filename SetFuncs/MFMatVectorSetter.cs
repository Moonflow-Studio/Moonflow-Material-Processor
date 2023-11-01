using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Moonflow.MFAssetTools.MFMatProcessor
{
    public class MFMatVectorSetter : MFMatDataSetter<Vector4>
    {
        public override string displayName => colorMode ? "Color" : "Vector";

        public string oldChannel = "xyzw";
        public string newChannel = "xyzw";
        public bool colorMode;

        public MFMatVectorSetter()
        {
            oldChannel = "xyzw";
            newChannel = "xyzw";
            colorMode = false;
        }
        public MFMatVectorSetter(bool c)
        {
            colorMode = c;
        }
        public override void SetData(Material mat)
        {
            if (!deliverMode)
            {
                if (mat.HasProperty(targetPropName))
                {
                    SetVector(mat, manualData);
                }
            }
            else
            {
                if (GetOldProp(mat, out Vector4 oldData) && mat.HasProperty(targetPropName))
                {
                    SetVector(mat, oldData);
                }
            }
        }

        private void SetVector(Material mat, Vector4 oldData)
        {
            Vector4 current = mat.GetVector(targetPropName);
            Vector4 newData = Vector4.zero;
            for (int i = 0; i < 4; i++)
            {
                SwitchChannel(ref newData, i, oldData);
            }
        }

        private void SwitchChannel(ref Vector4 data, int index, Vector4 oldData)
        {
            Vector4 current = data;
            switch (newChannel[index])
            {
                case 'x':
                    data.x = oldData.x;
                    break;
                case 'y':
                    data.y = oldData.x;
                    break;
                case 'z':
                    data.z = oldData.x;
                    break;
                case 'w':
                    data.w = oldData.x;
                    break;
            }
        }
        protected override bool GetOldProp(Material mat, out Vector4 oldProp)
        {
            int lineIndex = HasOldProp(mat, out string line);
            oldProp = Vector4.zero;
            if (lineIndex != -1)
            {
                var match = Regex.Match(line, @"-\s_\w+:\s\{r:\s([(-?\d+)(\.\d+)?]*),\sg:\s([(-?\d+)(\.\d+)?]*),\sb:\s([(-?\d+)(\.\d+)?]*),\sa:\s([(-?\d+)(\.\d+)?]*)\}");
                if (match.Success)
                {
                    oldProp.x = float.Parse(match.Groups[1].Value);
                    oldProp.y = float.Parse(match.Groups[2].Value);
                    oldProp.z = float.Parse(match.Groups[3].Value);
                    oldProp.w = float.Parse(match.Groups[4].Value);
                    return true;
                }
            }

            return false;
        }

        public override void DisplayManualData()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                string targetType = colorMode ? "Color" : "Vector4";
                EditorGUILayout.LabelField($"Set {targetType} Value", GUILayout.Width(125));
                if (colorMode)
                {
                    Color color = manualData;
                    color = EditorGUILayout.ColorField("", color, GUILayout.Width(275));
                    manualData = color;
                }
                else
                {
                    manualData = EditorGUILayout.Vector4Field("", manualData, GUILayout.Width(275));
                }
            }
        }

        public override void SpecialDisplay()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Channel Order", GUILayout.Width(100));
                
                EditorGUILayout.LabelField("Old Channel", GUILayout.Width(80));
                float normalLabelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 5;
                GUI.color = IsChannelFormatLegal(oldChannel) ? Color.white : Color.red;
                oldChannel = EditorGUILayout.TextField(".",oldChannel, GUILayout.Width(50));
                GUI.color = Color.white;
                EditorGUIUtility.labelWidth = normalLabelWidth;
                
                EditorGUILayout.LabelField(" >>>> ", GUILayout.Width(30));
                
                EditorGUILayout.LabelField("New Channel", GUILayout.Width(80));
                EditorGUIUtility.labelWidth = 5;
                GUI.color = IsChannelFormatLegal(newChannel, false, oldChannel.Length)? Color.white : Color.red;
                newChannel = EditorGUILayout.TextField(".", newChannel, GUILayout.Width(50));
                GUI.color = Color.white;
                
                EditorGUIUtility.labelWidth = normalLabelWidth;
            }
        }

        public bool IsChannelFormatLegal(string channel, bool canRepeat = true, int numLimit = 0)
        {
            if(string.IsNullOrEmpty(channel))
                return false;
            //长度在[1,4]
            if (channel.Length < 1 || channel.Length > 4)
            {
                return false;
            }
            //只能包含xyzwrgba
            for (int i = 0; i < channel.Length; i++)
            {
                if (!"xyzwrgba".Contains(channel[i].ToString()))
                {
                    return false;
                }
            }
            //不能重复
            if (!canRepeat)
            {
                for (int i = 0; i < channel.Length; i++)
                {
                    for (int j = i + 1; j < channel.Length; j++)
                    {
                        if (channel[i] == channel[j])
                        {
                            return false;
                        }
                    }
                }
            }

            if (numLimit != 0)
            {
                if (channel.Length != numLimit) return false;
            }
            return true;
        }

        public override bool isLegal()
        {
            if(string.IsNullOrEmpty(targetPropName) || string.IsNullOrEmpty(oldPropName))
                return false;
            if (!IsChannelFormatLegal(oldChannel)) return false;
            if (!IsChannelFormatLegal(newChannel, false)) return false;
            return true;
        }
    }
}