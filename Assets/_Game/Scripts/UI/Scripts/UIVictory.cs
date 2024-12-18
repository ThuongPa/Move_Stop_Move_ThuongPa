using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVictory : UICanvas
{
    [SerializeField] Text playerCoin;
    private int coin;

    public override void Setup()
    {
        base.Setup();
    }

    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeState(GameState.Finish);
    }

    public void HomeButton()
    {
        DataManager.Ins.playerData.coin += coin;
        DataManager.Ins.SetData(ref DataManager.Ins.playerData.coin, DataManager.Ins.playerData.coin);
        LevelManager.Ins.Home();
    }

    public void SetCoin(int coin)
    {
        this.coin = coin;
        playerCoin.text = coin.ToString();
    }

    public void X3Button()
    {
        LevelManager.Ins.Home();
        DataManager.Ins.playerData.coin += 3*coin;
        DataManager.Ins.SetData(ref DataManager.Ins.playerData.coin, DataManager.Ins.playerData.coin);
    }
}
