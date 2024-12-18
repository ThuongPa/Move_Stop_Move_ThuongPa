
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Scriptable
{
    public enum WeaponName
    {
        Hammer = PoolType.W_Hammer_1,
        Knife = PoolType.W_Knife,
        Candy = PoolType.W_Candy_1,
        Boomerang = PoolType.W_Boomerang_1
    }

    [CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/Data/WeaponData", order = 1)]

    public class WeaponData : ScriptableObject
    {

       
        [SerializeField] List<WeaponType> weaponTypes;

        public Weapon GetWeapon(WeaponName weaponName)
        {
           
            switch (weaponName)
            {
                case WeaponName.Hammer:
                    return weaponTypes[0].model;

                case WeaponName.Knife:
                    return weaponTypes[1].model;

                case WeaponName.Candy:
                    return weaponTypes[2].model;

                case WeaponName.Boomerang:
                    return weaponTypes[3].model;
            }
            return weaponTypes[0].model;
        }
        public WeaponType GetWeaponType(WeaponName weaponName)
        {
            switch (weaponName)
            {
                case WeaponName.Hammer:
                    return weaponTypes[0];
                 
                case WeaponName.Knife:
                    return weaponTypes[1];
                   
                case WeaponName.Candy:
                    return weaponTypes[2];
                  
                case WeaponName.Boomerang:
                    return weaponTypes[3];     
            }
            return weaponTypes[0];
        }
        public WeaponName NextType(WeaponName weaponName)
        {
            int index = weaponTypes.FindIndex(q => q.WeaponName == weaponName);
            index = index + 1 >= weaponTypes.Count ? 0 : index + 1;
            return weaponTypes[index].WeaponName;
        }
        public WeaponName PrevType(WeaponName weaponName)
        {
            int index = weaponTypes.FindIndex(q => q.WeaponName == weaponName);
            index = index - 1 < 0 ? weaponTypes.Count - 1 : index - 1;
            return weaponTypes[index].WeaponName;
        }

        public Sprite GetIcon(PantName weaponName)
        {
            return weaponTypes[(int)weaponName].icon;
        }
     


    }
}