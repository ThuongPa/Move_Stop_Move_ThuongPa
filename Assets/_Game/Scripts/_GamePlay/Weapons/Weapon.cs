using Scriptable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Weapon : GameUnit
{
    [SerializeField] BulletType bulletType;
    public void Throw(Character owner,Vector3 direction,WeaponType weaponType)
    {
        //Bullet bullet = Instantiate(bulletPrefab, owner.transform.position + new Vector3(0,1,0), owner.transform.rotation);
        Bullet bullet = SimplePool.Spawn<Bullet>((PoolType)bulletType, owner.transform.position + new Vector3(0, 1, 0), owner.transform.rotation);
        bullet.OnInit(owner, direction, weaponType);
    }
    
   
}
