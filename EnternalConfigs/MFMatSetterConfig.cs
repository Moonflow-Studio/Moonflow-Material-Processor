using UnityEngine;

namespace Moonflow.MFAssetTools.MFMatProcessor.EnternalConfigs
{
    [CreateAssetMenu(fileName = "NewMFMatSetterConfig", menuName = "Moonflow/MFMatProcessor/新建替换配置")]
    public class MFMatSetterConfig : ScriptableObject
    {
        [SerializeField] public MFMatDataSetter[] setters;
    }
}