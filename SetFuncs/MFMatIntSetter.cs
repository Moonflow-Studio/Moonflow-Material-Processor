using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Moonflow.MFAssetTools.MFMatProcessor
{
    public class MFMatIntSetter:MFMatDataSetter<int>
    {
        public override string displayName => QueueMode ? "Queue":"Int";

        [SerializeField]public bool QueueMode;
        [SerializeField]public string expression = "x";
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
                        oldData = TranslateOldData(oldData);
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
        private int TranslateOldData(int oldProp)
        {
            ExpressionEvaluator.Evaluate(expression.Replace("x", oldProp.ToString()), out int result);
            return result;
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
            manualData = EditorGUILayout.IntField($"设置{targetType}值", manualData);
        }
        
        public override void SpecialDeliverDisplay()
        {
            base.SpecialDeliverDisplay();
            GUI.color = expression.Contains("x") ? Color.white : Color.red;
            expression = EditorGUILayout.TextField("y = ", expression);
            GUI.color = Color.white;
        }
    }
}