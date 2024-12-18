using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : GameUnit
{
    private BoosterType _type;

    private void OnEnable()
    {
        // Chọn ngẫu nhiên loại booster khi đối tượng được kích hoạt
        _type = Utilities.RandomEnumValue<BoosterType>();
    }

    public enum BoosterType
    {
        Booster_Shield = PoolType.Booster_Shield, // Booster bảo vệ
        Booster_Speed = PoolType.Booster_Speed // Booster tăng tốc
    }

    public void DropBooster(Vector3 pos)
    {
        // Chọn ngẫu nhiên loại booster trước khi thả
        _type = Utilities.RandomEnumValue<BoosterType>();
        // Sinh ra một booster mới tại vị trí được chỉ định
        SimplePool.Spawn<Booster>(PoolType.Booster, pos, Quaternion.identity);
    }

    // Phương thức xử lý va chạm với các collider khác
    private void OnCollisionEnter(Collision collision)
    {
        Character crt = collision.gameObject.GetComponent<Character>(); // Lấy thành phần Character từ collider
        // Kiểm tra xem collider có phải là một nhân vật
        if (collision.gameObject.CompareTag(Const.CHARACTER_TAG))
        {
            crt.BoosterType = _type; // Gán loại booster cho nhân vật
            crt.IsHavingBooster = true; // Đặt trạng thái là đang có booster

            // Kiểm tra loại booster và thực hiện hành động tương ứng
            if (_type == BoosterType.Booster_Shield)
            {
                InitShield(crt); // Khởi tạo shield cho nhân vật
            }
            if (_type == BoosterType.Booster_Speed)
            {
                crt.SpeedUp(); // Tăng tốc cho nhân vật
            }

            // Hủy booster sau khi nó đã được sử dụng
            SimplePool.Despawn(this);
        }
    }

    //Khởi tạo shield cho nhân vật
    public void InitShield(Character crt)
    {
        // Sinh ra một ShieldBooster mới và gán chủ sở hữu là nhân vật
        ShieldBooster shieldBooster = SimplePool.Spawn<ShieldBooster>((PoolType)_type, crt.transform);
        if (shieldBooster != null)
        {
            shieldBooster.owner = crt; // Gán chủ sở hữu cho shield booster
        }
    }
}
