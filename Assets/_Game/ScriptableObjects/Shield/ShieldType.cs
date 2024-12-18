using Scriptable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "ShieldType", menuName = "ScriptableObjects/Type/ShieldType", order = 1)]
public class ShieldType : ScriptableObject  
{
    public ShieldName shieldName;
    public Sprite icon;
    public GameObject model;
}
