
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.LightingExplorerTableColumn;


namespace Scriptable
{
    public enum HatName
    {
        Arrow = PoolType.HAT_Arrow,
        Cowboy = PoolType.HAT_Cowboy,
        Crown = PoolType.HAT_Crown,
        Ear = PoolType.HAT_Ear,
        Hat = PoolType.HAT_Police,
        Hat_Cap = PoolType.HAT_Cap,
        Hat_Yellow = PoolType.HAT_StrawHat,
        Headphone = PoolType.HAT_Headphone,
    }

    [CreateAssetMenu(fileName = "HatData", menuName = "ScriptableObjects/Data/HatData", order = 1)]
    public class HatData : ScriptableObject
    {

        [SerializeField] HatType[] hatTypes;

        public GameObject GetHat(HatName hatName)
        {
            return hatTypes[(int)hatName].model;
        }

        public Sprite GetIcon(HatName hatName)
        {
            return hatTypes[(int)hatName].icon;
        }

    }
}