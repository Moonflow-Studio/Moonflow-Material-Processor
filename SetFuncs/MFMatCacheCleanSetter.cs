using UnityEditor;
using UnityEngine;

namespace Moonflow.MFAssetTools.MFMatProcessor
{
    public class MFMatCacheCleanSetter : MFMatDataSetter
    {
        public override string displayName => "Old Saved Properties Cleaner";
        public override void DisplayManualData()
        {
            noDeliver = true;
        }

        public override void SetData(Material mat)
        {
            SerializedObject so = new SerializedObject(mat);
            RemoveUnusedProperties(mat, so, "m_SavedProperties.m_TexEnvs", PropertyType.TexEnv);
            RemoveUnusedProperties(mat, so, "m_SavedProperties.m_Floats", PropertyType.Float);
            RemoveUnusedProperties(mat, so, "m_SavedProperties.m_Colors", PropertyType.Color);
            RemoveUnusedProperties(mat, so, "m_SavedProperties.m_Ints", PropertyType.Int);
        }
        private enum PropertyType { TexEnv, Int, Float, Color }
        private static bool ShaderHasProperty(Material mat, string name, PropertyType type)
        {
            switch (type)
            {
                case PropertyType.TexEnv:
                    return mat.HasTexture(name);
                case PropertyType.Int:
                    return mat.HasInteger(name);
                case PropertyType.Float:
                    return mat.HasFloat(name);
                case PropertyType.Color:
                    return mat.HasColor(name);
            }
            return false;
        }
        
        private static bool HasShader(Material mat)
        {
            return mat.shader.name != "Hidden/InternalErrorShader";
        }
        private static string GetName(SerializedProperty property)
        {
            return property.FindPropertyRelative("first").stringValue; //return property.displayName;
        }
        private void RemoveUnusedProperties(Material mat, SerializedObject so, string path, PropertyType type)
        {
            if (!HasShader(mat))
            {
                Debug.LogError("Material " + mat.name + " doesn't have a shader");
                return;
            }
 
            var properties =so.FindProperty(path);
            if (properties != null && properties.isArray)
            {
                for (int j = properties.arraySize - 1; j >= 0; j--)
                {
                    string propName = GetName(properties.GetArrayElementAtIndex(j));
                    bool exists = ShaderHasProperty(mat, propName, type);
 
                    if (!exists)
                    {
                        Debug.Log("Removed " + type + " Property: " + propName);
                        properties.DeleteArrayElementAtIndex(j);
                        so.ApplyModifiedProperties();
                    }
                }
            }
        }
    }
}