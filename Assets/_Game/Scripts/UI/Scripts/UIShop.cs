using Scriptable;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : UICanvas
{
    public enum ShopType { Hat, Pant, Shield, Skin, Weapon }
    [SerializeField] Text playerCoin;
    [SerializeField] ShopData data;
    [SerializeField] ShopItem prefab;
    [SerializeField] Transform content;
    [SerializeField] ShopBar[] shopBars;

    [SerializeField] ButtonState buttonState;
    [SerializeField] Text coinTxt;
    [SerializeField] Text adsTxt;

    MiniPool<ShopItem> miniPool = new MiniPool<ShopItem>();

    private ShopItem currentItem;
    private ShopBar currentBar;

    private ShopItem itemEquiped;

    public ShopType shopType => currentBar.Type;

    private void Awake()
    {
        miniPool.OnInit(prefab, 10, content);

        for (int i = 0; i < shopBars.Length; i++)
        {
            shopBars[i].SetShop(this);
        }
    }

    public override void Open()
    {
        base.Open();
        SelectBar(shopBars[0]);
        CameraFollow.Ins.ChangeState(GameState.MainMenu);
        playerCoin.text=(DataManager.Ins.playerData.coin.ToString());
    }

    public override void CloseDirectly()
    {
        base.CloseDirectly();
        UIManager.Ins.OpenUI<UIMainMenu>();

        LevelManager.Ins.player.OnDespawnItem();
        LevelManager.Ins.player.OnTakeUserData();
        LevelManager.Ins.player.OnInitItem();
    }

    internal void SelectBar(ShopBar shopBar)
    {
        if (currentBar != null)
        {
            currentBar.Active(false);
        }

        currentBar = shopBar;
        currentBar.Active(true);

        miniPool.Collect();
        itemEquiped = null;

        switch (currentBar.Type)
        {
            case ShopType.Hat:
                InitShipItems(data.hats.Ts, ref itemEquiped);
                break;
            case ShopType.Pant:
                InitShipItems(data.pants.Ts, ref itemEquiped);
                break;
            case ShopType.Shield:
                InitShipItems(data.accessories.Ts, ref itemEquiped);
                break;
            case ShopType.Skin:
                InitShipItems(data.skins.Ts, ref itemEquiped);
                break;
            default:
                break;
        }

    }
    public List<ShopData<T>> GetListItems<T>(ShopType shopType) where T : Enum
    {
        switch (shopType)
        {
            case ShopType.Hat: return data.hats.Ts as List<ShopData<T>>;
            case ShopType.Pant: return data.pants.Ts as List<ShopData<T>>;
            case ShopType.Shield: return data.accessories.Ts as List<ShopData<T>>;
            case ShopType.Skin: return data.skins.Ts as List<ShopData<T>>;
            case ShopType.Weapon: return null;
            default: return null;
        }
    }

    private void InitShipItems<T>(List<ShopData<T>> items, ref ShopItem itemEquiped) where T : Enum
    {
        for (int i = 0; i < items.Count; i++)
        {
            ShopItem.State state = (ShopItem.State)DataManager.Ins.GetItemState(items[i].type);
            ShopItem item = miniPool.Spawn();
            item.SetData(i, items[i], this);
            item.SetState(state);

            if (state == ShopItem.State.Equipped)
            {
                SelectItem(item);
                itemEquiped = item;
            }
        }
    }

    public ShopItem.State GetState(Enum t) => (ShopItem.State)DataManager.Ins.GetItemState(t);

    internal void SelectItem(ShopItem item)
    {
        if (currentItem != null)
        {
            
            currentItem.SetState(GetState(currentItem.Type));
        }

        currentItem = item;
     

        switch (currentItem.state)
        {
            case ShopItem.State.Buy:
                buttonState.SetState(ButtonState.State.Buy);
                break;
            case ShopItem.State.Bought:
                buttonState.SetState(ButtonState.State.Equip);
                break;
            case ShopItem.State.Equipped:
                buttonState.SetState(ButtonState.State.Equiped);
                break;
            case ShopItem.State.Selecting:
                break;
            default:
                break;
        }
        LevelManager.Ins.player.TryCloth(shopType, currentItem.Type);
        currentItem.SetState(ShopItem.State.Selecting);
        coinTxt.text = item.data.cost.ToString();
    }

    public void BuyButton()
    {
        if (DataManager.Ins.playerData.coin >= currentItem.data.cost)
        {
            DataManager.Ins.SetItemState(currentItem.Type, ItemState.Bought);
            DataManager.Ins.SetData(ref DataManager.Ins.playerData.coin,DataManager.Ins.playerData.coin-currentItem.data.cost);
            playerCoin.text = DataManager.Ins.playerData.coin.ToString();
        }
            SelectItem(currentItem);
    }

    public void AdsButton()
    {

    }

    public void EquipButton()
    {
        if (currentItem != null)
        {
            DataManager.Ins.SetItemState(currentItem.Type, ItemState.Equipped);
          
            switch (shopType)
            {
                case ShopType.Hat:                  
                    DataManager.Ins.SetItemState(DataManager.Ins.playerData.playerHat,ItemState.Bought);
                    DataManager.Ins.SetEnumData(ref DataManager.Ins.playerData.playerHat,(HatName) currentItem.Type);
                    break;
                case ShopType.Pant:
                    DataManager.Ins.SetItemState(DataManager.Ins.playerData.playerPant, ItemState.Bought);
                    DataManager.Ins.SetEnumData(ref DataManager.Ins.playerData.playerPant,(PantName) currentItem.Type);
                    break;
                case ShopType.Shield:
                    DataManager.Ins.SetItemState(DataManager.Ins.playerData.playerShield, ItemState.Bought);
                    DataManager.Ins.SetEnumData(ref DataManager.Ins.playerData.playerShield, (ShieldName)currentItem.Type);
                    break;
                case ShopType.Skin:                   
                    DataManager.Ins.SetItemState(DataManager.Ins.playerData.playerSkin, ItemState.Bought);
                    DataManager.Ins.SetEnumData(ref DataManager.Ins.playerData.playerSkin, (SkinType)currentItem.Type);
                    break;
                case ShopType.Weapon:
                    break;
                default:
                    break;
            }
        }

        if (itemEquiped != null)
        {
            itemEquiped.SetState(ShopItem.State.Bought);
        }

        currentItem.SetState(ShopItem.State.Equipped);
        SelectItem(currentItem);
    }
}
