using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Moonflow.MFAssetTools.MFMatProcessor
{
    [Serializable]
    public abstract class MFMatDataSetter : ScriptableObject
    {
        public abstract string displayName { get; }
        [SerializeField]public string targetPropName;
        [SerializeField]public string oldPropName;
        [SerializeField]public bool deliverMode;
        protected bool noDeliver = false;

        public virtual bool isLegal()
        {
            if (noDeliver || !deliverMode) return true;
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
                if (noDeliver) return;
                MFEditorUI.DivideLine(Color.gray);
                float normalLabelWidth = EditorGUIUtility.labelWidth;
                using (new EditorGUILayout.HorizontalScope(GUILayout.Width(400)))
                {
                    deliverMode = EditorGUILayout.ToggleLeft("传递旧数值", deliverMode);
                    EditorGUIUtility.labelWidth = 75;
                    GUI.color = string.IsNullOrEmpty(targetPropName) ? Color.red : Color.white;
                    targetPropName = EditorGUILayout.TextField("新参数名", targetPropName);
                    GUI.color = Color.white;
                }
                EditorGUIUtility.labelWidth = 50;
                float normalFieldWidth = EditorGUIUtility.fieldWidth;
                EditorGUIUtility.fieldWidth = 10;
                if (deliverMode)
                {
                    GUI.color = string.IsNullOrEmpty(oldPropName) ? Color.red : Color.white;
                    oldPropName = EditorGUILayout.TextField("旧参数名", oldPropName);
                    GUI.color = Color.white;
                    SpecialDeliverDisplay();
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

        public virtual void SpecialDeliverDisplay()
        {
            
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