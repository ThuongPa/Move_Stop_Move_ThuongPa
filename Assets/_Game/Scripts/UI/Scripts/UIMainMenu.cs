using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : UICanvas
{
    private const string ANIM_OPEN = "Open";
    private const string ANIM_CLOSE = "Close";
    [SerializeField] Text playerCoinTxt;
    [SerializeField] RectTransform coinPoint;
    [SerializeField] Animation anim;
    public RectTransform CoinPoint => coinPoint;

    public override void Setup()
    {
        base.Setup();
        playerCoinTxt.text = DataManager.Ins.playerData.coin.ToString();
    }
    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeState(GameState.MainMenu);
        CameraFollow.Ins.ChangeState(GameState.MainMenu);



        //anim.Play(ANIM_OPEN);
    }



    public void ShopButton()
    {
        UIManager.Ins.OpenUI<UIShop>();
        Close(0);
    }


    public void WeaponButton()
    {
        UIManager.Ins.OpenUI<UIWeapon>();
        Close(0);
    }

    public void PlayButton()
    {
        LevelManager.Ins.OnPlay();
        UIManager.Ins.OpenUI<UIGameplay>();
        CameraFollow.Ins.ChangeState(GameState.GamePlay);

        Close(0.5f);
        //anim.Play(ANIM_CLOSE);
    }
}

