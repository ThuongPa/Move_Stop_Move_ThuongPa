using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCoin : GameUnit
{
    public void OnEnable()
    {
        StartCoroutine(OnDespawn());
    }
    private IEnumerator OnDespawn()
    {
        yield return new  WaitForSeconds(1f);
        SimplePool.Despawn(this);
    }
    

}
