using System;
using UnityEditor;
using UnityEngine;

namespace Moonflow.MFAssetTools.MFMatProcessor
{
    public class MFMatTextureSetter : MFMatDataSetter<Texture>
    {
        public override string displayName => "Texture";

        protected override bool GetOldProp(Material mat, out Texture data)
        {
            try
            {
                data = mat.GetTexture(oldPropName);
            }
            catch (Exception e)
            {
                data = default;
                return false;
            }
            return true;
        }

        public override bool isLegal()
        {
            if(manualData == null)
                return false;

            if (deliverMode)
            {
                if (string.IsNullOrEmpty(oldPropName))
                    return false;
            }
            if(string.IsNullOrEmpty(targetPropName))
                return false;
            return true;
        }
        
        public override void SetData(Material mat)
        {
            if (!deliverMode)
            {
                if (mat.HasProperty(targetPropName))
                {
                    mat.SetTexture(targetPropName, manualData);
                }
            }
            else
            {
                if(GetOldProp(mat, out Texture oldData) && mat.HasProperty(targetPropName))
                {
                    mat.SetTexture(targetPropName, oldData);
                }
            }
        }

        public override void DisplayManualData()
        {
            manualData = EditorGUILayout.ObjectField("设置贴图", manualData, typeof(Texture), false) as Texture;
        }
    }
}