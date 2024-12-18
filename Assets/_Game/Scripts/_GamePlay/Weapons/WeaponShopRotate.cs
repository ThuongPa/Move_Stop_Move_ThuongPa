using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShopRotate : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 800f;
    [SerializeField] Vector3 direction;
    private void Update()
    {
        transform.Rotate(direction * Time.deltaTime * rotateSpeed);
    }
}
