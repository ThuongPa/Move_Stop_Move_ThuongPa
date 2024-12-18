using UnityEngine;

public class Bullet : GameUnit
{
    [SerializeField] protected Rigidbody rb;
    public Character owner;
    protected bool isCanRunning;
    private int range;
    protected int speed;

    public virtual void OnInit(Character owner,Vector3 direction,WeaponType weaponType)
    { 
        this.range = weaponType.range;
        this.speed = weaponType.speed;
        this.owner = owner;
        isCanRunning = true;    
        rb.velocity = direction.normalized * speed;
        
    }

    public virtual void OnDespawn()
    {

    }

    protected virtual void OnMoveStop() { }

    private void OnTriggerEnter(Collider other)
    {
        
        Character crt = other.GetComponent<Character>();
        if (other.CompareTag(Const.CHARACTER_TAG) && crt!=owner && !crt.IsDead && isCanRunning)
        {
            owner.SetActiveCurrentWeapon(true);
            ParticlePool.Play(Utilities.RandomInMember(ParticleType.Hit_1, ParticleType.Hit_2, ParticleType.Hit_3), TF.position);
            crt.SetAttacker(owner);
            crt.OnHit();
            owner.SetPoint(1);
            Destroy(this.gameObject);
            
        }
        else if (other.CompareTag(Const.OBSTACLE_TAG))
        {
            OnMoveStop();
        }
    }
}
