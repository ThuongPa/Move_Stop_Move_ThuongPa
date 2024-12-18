using System.Collections.Generic;
using UnityEngine;

public class NotiManager : Singleton<NotiManager>
{
    private string _popUpText;
    [SerializeField] private Transform content;
    [SerializeField] private NotiText notiPrefab;
    [SerializeField] private Queue<NotiText> _popUpQueue;
    
    MiniPool<NotiText> miniPool = new MiniPool<NotiText>();

    private void Awake()
    {
        miniPool.OnInit(notiPrefab, 4, content);
        _popUpQueue = new Queue<NotiText>();
    }

    public void PopUpWindow(string target,string killer)
    {
        _popUpText = target + " killed by " + killer;
        if(_popUpQueue.Count >=4)
        {
            miniPool.Despawn(_popUpQueue.Dequeue());
        }
        NotiText newNotiText = miniPool.Spawn();
        newNotiText.SetText(_popUpText);
        _popUpQueue.Enqueue(newNotiText);
    }

    public void OnDespawn()
    {
       foreach(NotiText noti in _popUpQueue)
       {
            miniPool.Despawn(noti);
       }
    }
}
