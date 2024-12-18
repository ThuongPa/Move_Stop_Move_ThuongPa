using UnityEngine;

public class ForwardBullet : Bullet
{
    public override void OnInit(Character owner, Vector3 direction, WeaponType weaponType)
    {
        base.OnInit(owner, direction, weaponType);
    }

    private void Update()
    {
        if (!isCanRunning)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        if (Vector3.Distance(TF.position, owner.TF.position) > owner.Size * Const.ATT_RANGE && isCanRunning)
        {
            OnDespawn();
        }
    }

    protected override void OnMoveStop()
    {
        base.OnMoveStop();
        isCanRunning = false;
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        owner.SetActiveCurrentWeapon(true);
        SimplePool.Despawn(this);
    }
}
