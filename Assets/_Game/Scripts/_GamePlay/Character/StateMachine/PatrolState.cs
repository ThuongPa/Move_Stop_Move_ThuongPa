using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    public void OnEnter(Bot t)
    {
        t.RandomMove(); // Bắt đầu di chuyển ngẫu nhiên
    }

    public void OnExecute(Bot t)
    {
        t.StopRandomMove(); // Dừng di chuyển ngẫu nhiên nếu đã đến điểm đến
    }

    public void OnExit(Bot t)
    {
       
    }
}

