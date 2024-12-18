
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Scriptable
{
    public enum ColorType
    {
        Yellow = 0,
        Red = 1,
        Blue = 2,
        Green = 3,
        Orange = 4,
        Gray = 5
    }

    [CreateAssetMenu(fileName = "ColorData", menuName = "ScriptableObjects/Data/ColorData", order = 1)]
    public class ColorData : ScriptableObject
    {
        //theo tha material theo dung thu tu ColorType
        [SerializeField] Material[] materials;

        //lay material theo mau tuong ung
        public Material GetMat(ColorType colorType)
        {
            return materials[(int)colorType];
        }
    }
}