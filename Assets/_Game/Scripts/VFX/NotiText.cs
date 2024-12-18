using Scriptable;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.UI;

public class NotiText : GameUnit
{
    [SerializeField] private Text notiText;

    private void Start()
    {
            notiText.color = new Color(Random.value, Random.value, Random.value);  
    }
    public void SetText(string text)
    {
        notiText.text = text;
    }
   
    private IEnumerator OnDespawn()
    {
        yield return new WaitForSeconds(2f);
        SimplePool.Despawn(this);
    }
    

}
