using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    [SerializeField] private Character owner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Const.CHARACTER_TAG))
        { 
            Character target = Cache.GetCharacter(other);
            if (target != owner)
            {    
                Character character = other.GetComponent<Character>();             
                owner.AddTarget(target); 
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Const.CHARACTER_TAG))
        {
            Character target = Cache.GetCharacter(other);

            if(target != owner)
                if(owner.CheckTarget(target))
                {
                    owner.RemoveTarget(target);
                }
        }
    }
}
