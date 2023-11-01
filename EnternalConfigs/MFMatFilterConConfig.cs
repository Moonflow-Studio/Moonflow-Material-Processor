using System.Collections.Generic;
using UnityEngine;

namespace Moonflow.MFAssetTools.MFMatProcessor.EnternalConfigs
{
    [CreateAssetMenu(fileName = "NewMFMatFilterConConfig", menuName = "Moonflow/MFMatProcessor/新建条件筛选配置")]
    public class MFMatFilterConConfig : ScriptableObject
    {
        [SerializeField]public MFMatFilterCon[] filters;
    }
}