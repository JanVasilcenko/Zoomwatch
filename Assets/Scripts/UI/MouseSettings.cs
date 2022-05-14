
using System;
using UnityEngine;

namespace UnityTemplateProjects.UI

{
    [CreateAssetMenu(fileName = "MouseSensitivity", menuName = "ScriptableObject/MouseSensitivityObject", order = 1)]
    public class MouseSettings : ScriptableObject
    {
        public float sensitivityX, sensitivityY;
        
    }
}