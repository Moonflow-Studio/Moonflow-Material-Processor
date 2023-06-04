using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Moonflow.MFAssetTools.MFMatProcessor
{
    public class MFMatIntSetter:MFMatDataSetter<int>
    {
        public override string displayName => QueueMode ? "Queue":"Int";

        public bool QueueMode;

        public MFMatIntSetter(bool queueMode)
        {
            QueueMode = queueMode;
        }
        public override void SetData(Material mat)
        {
            if (QueueMode)
            {
                mat.renderQueue = manualData;
            }
            else
            {
                if (!deliverMode)
                {
                    if (mat.HasProperty(targetPropName))
                    {
                        mat.SetInt(targetPropName, manualData);
                    }
                }
                else
                {
                    if (GetOldProp(mat, out int oldData) && mat.HasProperty(targetPropName))
                    {
                        mat.SetInt(targetPropName, oldData);
                    }
                }
            }
        }

        public override void Draw()
        {
            if (!QueueMode)
            {
                base.Draw();
            }
            else
            {
                using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(400)))
                {
                    EditorGUILayout.LabelField(displayName, EditorStyles.boldLabel);
                    MFEditorUI.DivideLine(Color.gray);
                    DisplayManualData();
                }
            }
        }

        protected override bool GetOldProp(Material mat, out int oldProp)
        {
            int lineIndex = HasOldProp(mat, out string line);
            if (lineIndex != -1)
            {
                var match = Regex.Match(line, @"-\s\w*\s*: ");
                if (match.Success)
                {
                    oldProp = int.Parse(line.Trim().Replace(match.Value, ""));
                    return true;
                }
            }
            oldProp = 0;
            return false;
        }

        public override void DisplayManualData()
        {
            string targetType = QueueMode ? "Queue" : "Int";
            float oldwidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 100;
            manualData = EditorGUILayout.IntField($"Set {targetType} Value", manualData);
            EditorGUIUtility.labelWidth = oldwidth;
        }
    }
}