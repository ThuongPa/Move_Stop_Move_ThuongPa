using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UIShop;
using static UnityEditor.LightingExplorerTableColumn;

public class ShopItem : MonoBehaviour
{
    public enum State { Buy, Bought, Equipped, Selecting }

    [SerializeField] GameObject[] stateObjects;

    private UIShop shop;

    [SerializeField] Color[] colorBg;
    [SerializeField] Image icon;
    [SerializeField] Image bgIcon;

    public int id;
    public State state;

    public Enum Type;
    internal ShopItemData data;

    public void SetShop(UIShop shop)
    {
        this.shop = shop;
    }

    public void SetData<T>(int id, ShopData<T> itemData, UIShop shop) where T : Enum
    {
        this.id = id;
        Type = itemData.type;
        this.data = itemData;
        this.shop = shop;
        icon.sprite = itemData.icon;
        colorBg[(int)shop.shopType].a = 255;
        bgIcon.color = colorBg[(int)shop.shopType];
    }

    public void SelectButton()
    {
        shop.SelectItem(this);
    }

    public void SetState(State state)
    {
        for (int i = 0; i < stateObjects.Length; i++)
        {
            stateObjects[i].SetActive(false);
        }

        stateObjects[(int)state].SetActive(true);

        this.state = state;
    }
}
