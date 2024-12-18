using Scriptable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "PantType", menuName = "ScriptableObjects/Type/PantType", order = 1)]
public class PantType : ScriptableObject
{
    public PantName pantName;
    public int index;
    public Sprite icon;
    public Material material;
    public int speedBuff;
    public int rangeBuff;
}