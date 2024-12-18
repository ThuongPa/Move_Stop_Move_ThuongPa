using Scriptable;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ItemState { Buy = 0, Bought = 1, Equipped = 2 }

public class DataManager : Singleton<DataManager>
{
    public PlayerData playerData;

    public void InitUserData()
    {
        playerData = DataUtilities.LoadData<PlayerData>();
    }

    public void SaveUserData(PlayerData playerData)
    {
        DataUtilities.SaveData(playerData);
    }

    // Lấy state của Item theo Enum của Item    
    public ItemState GetItemState<T>(List<Item<T>> itemList, T key) where T : System.Enum
    {
        foreach (Item<T> item in itemList)
        {
            int itemValue = Convert.ToInt32(item.type);
            int keyConvert = Convert.ToInt32(key);
            if (itemValue == keyConvert)
            {
                return (ItemState)Enum.ToObject(typeof(ItemState), item.state);
            }
        }

        T newEnumValue = (T)Enum.ToObject(typeof(T), key);// Nếu chưa có Item trong List thì tạo thêm 1 item mới 
        Item<T> newItem = new Item<T> { type = newEnumValue, state = ItemState.Buy };
        itemList.Add(newItem);
        DataUtilities.UpdateData(playerData);
        return newItem.state;
    }

    public ItemState GetItemState<T>(T key) where T : System.Enum
    {
        switch (key)
        {
            case HatName hatName: return GetItemState(playerData.hats, hatName);
            case PantName pantName: return GetItemState(playerData.pants, pantName);
            case ShieldName shieldName: return GetItemState(playerData.shields, shieldName);
            case SkinType skinType: return GetItemState(playerData.skins, skinType);
            case WeaponName weaponName: return GetItemState(playerData.weapons, weaponName);
            default: return default(ItemState);
        }
    }

    public void SetItemState<T>(T key, ItemState value) where T : System.Enum
    {
        switch (key)
        {
            case HatName hatName:
                SetItemState(playerData.hats, hatName, value, ref playerData.playerHat);
                break;
            case PantName pantName:
                SetItemState(playerData.pants, pantName, value, ref playerData.playerPant);
                break;
            case ShieldName shieldName:
                SetItemState(playerData.shields, shieldName, value, ref playerData.playerShield);
                break;
            case SkinType skinType:
                SetItemState(playerData.skins, skinType, value, ref playerData.playerSkin);
                break;
             case WeaponName weaponName:
                 SetItemState(playerData.weapons,weaponName,value, ref playerData.playerWeapon);
                break;
            default:

                break;
        }
    }
    public void SetData(ref int variable,int value)
    {
        variable = value;
        DataUtilities.UpdateData(playerData);
    }
   
    public void SetEnumData<T>(ref T key,T value)
    {
        key = value;
        DataUtilities.UpdateData(playerData);
    }

    // Set trạng thái của Item theo Enum của Item 
    public void SetItemState<T>(List<Item<T>> itemList, T key, ItemState value, ref T variable) where T : System.Enum
    {
        if(key is ItemState.Equipped)
            variable = key;
        foreach (Item<T> item in itemList)
        {
            int itemValue = Convert.ToInt32(item.type);
            if (EqualityComparer<T>.Default.Equals(item.type, (T)(object)key))
            {
                item.state = value;
            }
        }
        DataUtilities.UpdateData(playerData);
    }

    public void SetItemState<T>(List<Item<T>> itemList, T key, ItemState value) where T : System.Enum
    {

        foreach (Item<T> item in itemList)
        {
            int itemValue = Convert.ToInt32(item.type);
            if (EqualityComparer<T>.Default.Equals(item.type, (T)(object)key))
            {
                item.state = value;
            }
        }
        DataUtilities.UpdateData(playerData);

    }

    [MenuItem("UserDataManager/ResetData")]
    public static void ResetData()
    {
        DataUtilities.UpdateData(new PlayerData());
        Debug.Log("Reset thành công Data người chơi");
    }
}

[Serializable]
public class PlayerData
{
    public int coin = 0;
    public int level = 0;
    public WeaponName playerWeapon = WeaponName.Knife;
    public HatName playerHat = HatName.Arrow;
    public PantName playerPant = PantName.Dabao;
    public SkinType playerSkin = SkinType.Normal;
    public ShieldName playerShield = ShieldName.Khien;
    public List<Item<HatName>> hats;
    public List<Item<PantName>> pants;
    public List<Item<SkinType>> skins;
    public List<Item<ShieldName>> shields;
    public List<Item<WeaponName>> weapons;
}

[Serializable]
public class Item<T> where T : System.Enum
{
    public T type;
    public ItemState state;
}



