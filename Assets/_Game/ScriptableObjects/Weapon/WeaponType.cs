using Scriptable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponType", menuName = "ScriptableObjects/Type/WeaponType", order = 1)]
public class WeaponType : ScriptableObject
{
    public WeaponName WeaponName;
    public int index;
    public string wpName;
    public Weapon model;
    public Sprite icon;

    public int price;
    public int range;
    public int speed;
}