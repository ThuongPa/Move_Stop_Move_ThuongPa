using UnityEngine;

public class BomerangBullet : Bullet
{
    public enum BulletState { Forward, Backward, Stop }
    private BulletState state;
    [SerializeField] Transform sprite;
    [SerializeField] float rotateSpeed = 800f;

    public override void OnInit(Character owner, Vector3 direction, WeaponType weaponType)
    {
        base.OnInit(owner, direction, weaponType);
        state = BulletState.Forward;
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        SimplePool.Despawn(this);
    }

    private void Update()
    {
        if (!isCanRunning)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        else {
            sprite.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed, Space.Self);
            switch (state)
            {
                case BulletState.Forward:
                    if (Vector3.Distance(TF.position, this.owner.TF.position) > owner.Size*Const.ATT_RANGE)
                    {
                        state = BulletState.Backward;
                    }
                    break;
                case BulletState.Backward:
                    //TF.position = Vector3.MoveTowards(TF.position, this.owner.TF.position, speed * Time.deltaTime);
                    rb.velocity = (owner.TF.position - this.TF.position).normalized * speed;
                    if(Vector3.Distance(TF.position, this.owner.TF.position) < 0.1f)
                    {
                        OnDespawn();
                    }
                    break;
                    
            }
        }
    }

    protected override void OnMoveStop()
    {
        base.OnMoveStop();
        isCanRunning = false;
        Invoke(nameof(OnDespawn), 5f);
    }
}
