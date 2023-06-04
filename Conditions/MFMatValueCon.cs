using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Moonflow.MFAssetTools.MFMatProcessor
{
    public abstract class MFMatValueCon : MFMatFilterCon
    {
        public CompareFunction comp;
        private static string [] compNames = new string[]
        {
            ">", "<", "=", "≠", "≥", "≤"
        };
        private int [] compValues = new int[]
        {
            (int) CompareFunction.Greater, (int) CompareFunction.Less, (int) CompareFunction.Equal,
            (int) CompareFunction.NotEqual, (int) CompareFunction.GreaterEqual, (int) CompareFunction.LessEqual
        };

        public override void DrawSpecial()
        {
            base.DrawSpecial();
            using (new EditorGUILayout.HorizontalScope())
            {
                DrawLeft(120);
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                //draw compValues
                GUI.color = Color.green;
                comp = (CompareFunction)EditorGUILayout.IntPopup((int)comp, compNames, compValues, GUILayout.Width(30));
                GUI.color = Color.white;
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                DrawRight(100);
            }
        }

        public abstract void DrawLeft(float width);
        public abstract void DrawRight(float width);
    }
}