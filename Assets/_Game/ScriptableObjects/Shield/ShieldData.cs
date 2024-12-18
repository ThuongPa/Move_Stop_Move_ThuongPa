
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Scriptable
{
    public enum ShieldName
    {
        Khien = PoolType.ACC_Shield,
        Captain = PoolType.ACC_Captain
    }

    [CreateAssetMenu(fileName = "ShieldData", menuName = "ScriptableObjects/Data/ShieldData", order = 1)]

    public class ShieldData : ScriptableObject
    {

        [SerializeField] ShieldType[] shieldTypes;


        public GameObject GetShield(ShieldName shieldName)
        {
            return shieldTypes[(int)shieldName].model;
        }
        public Sprite GetIcon(ShieldName shieldName)
        {
            return shieldTypes[(int)shieldName].icon;
        }

    }
}