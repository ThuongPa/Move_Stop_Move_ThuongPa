using Scriptable;
using UnityEngine;

public class Skin : GameUnit
{
    // properties for weapon 
    [SerializeField] private GameObject weaponHolder;
    [SerializeField] private Weapon currentWeapon;
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private WeaponType currentWeaponType;
    // properties for pant
    [SerializeField] private Renderer pantRenderer;
    [SerializeField] private PantData pantData;
    // properties for hat
    [SerializeField] private GameObject hatHolder;
    [SerializeField] private Hat currentHat;
    [SerializeField] private HatData hatData;
    // properties for shield
    [SerializeField] private GameObject shieldHolder;
    [SerializeField] private Shield currentShield;
    [SerializeField] private ShieldData shieldData;
    // properties for Animation
    [SerializeField] private Animator anim;
    public Animator Anim => anim;
    public Weapon Weapon => currentWeapon;

    public void ChangePant(PantName pantName)
    {
        pantRenderer.material = pantData.GetMat(pantName);
    }

    public void ChangeWeapon(WeaponName weaponName)
    {
        currentWeapon = SimplePool.Spawn<Weapon>((PoolType)weaponName, weaponHolder.transform);
    }

    public void ChangeHat(HatName hatName)
    {
        currentHat = SimplePool.Spawn<Hat>((PoolType)hatName, hatHolder.transform);
    }

    public void ChangeShield(ShieldName shieldName)
    {
        currentShield = SimplePool.Spawn<Shield>((PoolType)shieldName, shieldHolder.transform);
    }

    public void OnDespawn()
    {
        SimplePool.Despawn(currentWeapon);
        if (currentShield) SimplePool.Despawn(currentShield);
        if (currentHat) SimplePool.Despawn(currentHat);
    }

    public void DespawnHat()
    {
        if (currentHat) SimplePool.Despawn(currentHat);
    }

    public void DespawnShield()
    {
        if (currentShield) SimplePool.Despawn(currentShield);
    }

    internal void DespawnWeapon()
    {
        if (currentWeapon) SimplePool.Despawn(currentWeapon);
    }
}
