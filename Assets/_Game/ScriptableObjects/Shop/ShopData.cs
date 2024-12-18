using Scriptable;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopData", menuName = "ScriptableObjects/ShopData", order = 1)]
public class ShopData : ScriptableObject
{
    public ShopItemDatas<HatName> hats;
    public ShopItemDatas<PantName> pants;
    public ShopItemDatas<ShieldName> accessories;
    public ShopItemDatas<SkinType> skins;
}

[System.Serializable]
public class ShopItemDatas<T> where T : System.Enum
{
    [SerializeField] List<ShopData<T>> ts;
    public List<ShopData<T>> Ts => ts;

    public ShopData<T> GetHat(T t)
    {
        return ts.Single(q => q.type.Equals(t));
    }

}

[System.Serializable]
public class ShopData<T> : ShopItemData where T : System.Enum
{
    public T type;
}

public class ShopItemData
{
    public Sprite icon;
    public int cost;
    public int ads;
}