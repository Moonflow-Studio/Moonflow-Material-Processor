using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Moonflow.MFAssetTools.MFMatProcessor
{
    public class MFMatFloatSetter : MFMatDataSetter<float>
    {
        public override string displayName => "Float";
        [SerializeField]public string expression = "x";
        public override void SetData(Material mat)
        {
            if (!deliverMode)
            {
                if (mat.HasProperty(targetPropName))
                {
                    mat.SetFloat(targetPropName, manualData);
                }
            }
            else
            {
                if (GetOldProp(mat, out float oldData) && mat.HasProperty(targetPropName))
                {
                    oldData = TranslateOldData(oldData);
                    mat.SetFloat(targetPropName, oldData);
                }
            }
        }
        
        private float TranslateOldData(float oldProp)
        {
            ExpressionEvaluator.Evaluate(expression.Replace("x", oldProp.ToString()), out float result);
            return result;
        }

        protected override bool GetOldProp(Material mat, out float oldProp)
        {
            int lineIndex = HasOldProp(mat, out string line);
            if (lineIndex != -1)
            {
                var match = Regex.Match(line, @"-\s\w*\s*: ");
                if (match.Success)
                {
                    oldProp = float.Parse(line.Trim().Replace(match.Value, ""));
                    return true;
                }
            }

            oldProp = 0;
            return false;
        }

        public override void DisplayManualData()
        {
            float oldwidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 100;
            manualData = EditorGUILayout.FloatField("Set Float Value", manualData);
            EditorGUIUtility.labelWidth = oldwidth;
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