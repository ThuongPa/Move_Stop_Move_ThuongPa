using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameplay : UICanvas
{
    [SerializeField] private VariableJoystick variableJoystick;
    [SerializeField] private Text playerRemain;
    [SerializeField] private Transform content;

    // Start is called before the first frame update
    public override void Setup()
    {
        base.Setup();
        UpdatePlayerRemain();
    }
    public override void Open()
    {

        SetVariableJoyStick();
        GameManager.Ins.ChangeState(GameState.GamePlay);
        base.Open();
        
    
    }
    public void OpenSettingButton()
    {
        UIManager.Ins.OpenUI<UISetting>();
    }
    private void SetVariableJoyStick()
    {
        Player player = FindObjectOfType<Player>();
        player.SetVariableJoyStick(variableJoystick);
    }
    public void UpdatePlayerRemain()
    {
        playerRemain.text = "Alive : " + LevelManager.Ins.TotalCharater.ToString();
    }
}
