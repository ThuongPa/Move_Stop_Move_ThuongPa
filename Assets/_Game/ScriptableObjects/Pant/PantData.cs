
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Scriptable
{
    public enum PantName
    {
        Batman = 0,
        Chambi = 1,
        Comy = 2,
        Dabao = 3,
        Rainbow = 4,
        Pokemon = 5
    }


    [CreateAssetMenu(fileName = "PantData", menuName = "ScriptableObjects/Data/PantData", order = 1)]
    public class PantData : ScriptableObject
    {
        
        [SerializeField] PantType[] pantTypes;

       
        public Material GetMat(PantName pantName)
        {
            return pantTypes[(int)pantName].material;
        }
        public Sprite GetIcon(PantName pantName)
        {
            return pantTypes[(int)pantName].icon;
        }
        
    }
}