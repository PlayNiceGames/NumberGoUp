using System.Collections.Generic;
using UnityEngine;

namespace ServiceLocator
{
    public class GlobalServicesData : ScriptableObject
    {
        [field: SerializeField] public List<MonoBehaviour> Services { get; private set; }
    }
}