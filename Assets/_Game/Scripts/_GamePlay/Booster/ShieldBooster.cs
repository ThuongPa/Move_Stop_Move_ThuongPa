using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBooster : GameUnit
{
    public Character owner;

    private void OnEnable()
    {
        StartCoroutine(OnDespawn()); // Bắt đầu coroutine để tự động hủy sau 10 giây
    }

    private void OnTriggerEnter(Collider other)
    {
        Bullet bullet = other.GetComponent<Bullet>(); // Lấy thành phần Bullet từ collider
        // Kiểm tra xem collider có phải là viên đạn và không thuộc về chủ sở hữu của ShieldBooster
        if (other.CompareTag(Const.BULLET_TAG) && owner != bullet.owner)
        {
            SimplePool.Despawn(bullet); // Hủy viên đạn
            SimplePool.Despawn(this); // Hủy ShieldBooster
            bullet.owner.IsHavingBooster = false; // Đặt trạng thái IsHavingBooster của chủ sở hữu viên đạn thành false
        }
    }

    private IEnumerator OnDespawn()
    {
        yield return new WaitForSeconds(10f); // Đợi 10 giây
        SimplePool.Despawn(this); // Hủy ShieldBooster
    }
}
