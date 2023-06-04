using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Moonflow.MFAssetTools.MFMatProcessor
{
    public abstract class MFMatDataSetter
    {
        public abstract string displayName { get; }
        public string targetPropName;
        public string oldPropName;
        public bool deliverMode;

        public virtual bool isLegal()
        { 
            if(string.IsNullOrEmpty(targetPropName) || string.IsNullOrEmpty(oldPropName))
                return false;
            return true;
        }
        public int HasOldProp(Material mat, out string line)
        {
            line = "";
            var path = AssetDatabase.GetAssetPath(mat);
            var text = File.ReadLines(path);
            var lines = text.ToList();
            var lineIndex = lines.FindIndex(line => line.Contains(oldPropName));
            if(lineIndex!=-1)
                line = lines[lineIndex];
            return lineIndex;
        }
        public virtual void Draw()
        {
            using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(400)))
            {
                EditorGUILayout.LabelField(displayName, EditorStyles.boldLabel);
                MFEditorUI.DivideLine(Color.gray);
                float normalLabelWidth = EditorGUIUtility.labelWidth;
                using (new EditorGUILayout.HorizontalScope(GUILayout.Width(400)))
                {
                    deliverMode = EditorGUILayout.ToggleLeft("Deliver Old Data", deliverMode);
                    EditorGUIUtility.labelWidth = 125;
                    GUI.color = string.IsNullOrEmpty(targetPropName) ? Color.red : Color.white;
                    targetPropName = EditorGUILayout.TextField("New Param Name", targetPropName);
                    GUI.color = Color.white;
                }
                EditorGUIUtility.labelWidth = 50;
                float normalFieldWidth = EditorGUIUtility.fieldWidth;
                EditorGUIUtility.fieldWidth = 10;
                if (deliverMode)
                {
                    GUI.color = string.IsNullOrEmpty(oldPropName) ? Color.red : Color.white;
                    oldPropName = EditorGUILayout.TextField("Old Param Name", oldPropName);
                    GUI.color = Color.white;
                }
                else
                {
                    DisplayManualData();
                }
                EditorGUIUtility.labelWidth = normalLabelWidth;
                EditorGUIUtility.fieldWidth = normalFieldWidth;
                SpecialDisplay();
            }
        }
        public abstract void DisplayManualData();

        public virtual void SpecialDisplay()
        {
            
        }
        
        public abstract void SetData(Material mat);
    }
    
    public abstract class MFMatDataSetter<T> : MFMatDataSetter
    {
        public T manualData;

        protected virtual bool GetOldProp(Material mat, out T data)
        {
            data = default;
            return true;
        }
    }
}