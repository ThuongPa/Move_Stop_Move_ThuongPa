using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class AttackState : IState
{
    public void OnEnter(Bot t)
    {
        t.OnMoveStop(); // Dừng di chuyển của bot

        // Kiểm tra xác suất tấn công (60% thành công)
        if (Utilities.Chance(60, 100))
        {
            t.OnAttack(); // Nếu thành công, bot sẽ tấn công
        }
        else
        {
            t.ChangeState(new PatrolState()); // Nếu không, chuyển sang trạng thái tuần tra
        }
    }

    public void OnExecute(Bot t)
    {
       
    }

    public void OnExit(Bot t)
    {
        
    }
}
