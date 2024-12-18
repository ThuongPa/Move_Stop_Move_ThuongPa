using Scriptable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : AbstractCharacter
{
    [SerializeField] private Animator anim; // Animator để điều khiển hoạt ảnh
    [SerializeField] protected Skin currentSkin; // Da nhân vật hiện tại
    [SerializeField] private string currentAnim; // Tên hoạt ảnh hiện tại
    [HideInInspector] public Character currentAttacker; // Kẻ tấn công hiện tại
    [HideInInspector] public string characterName; // Tên nhân vật
    [SerializeField] private WeaponData weaponData; // Dữ liệu vũ khí
    [SerializeField] private WeaponType currentWeaponType; // Loại vũ khí hiện tại
    [SerializeField] private GameObject boxIndicator; // Chỉ báo vị trí
    [SerializeField] private Transform characterSprite; // Biến chứa hình ảnh nhân vật

    public Booster.BoosterType BoosterType { get; set; } // Loại booster
    public bool IsHavingBooster { get; set; } // Kiểm tra có booster không
    public int scalePoint; // Điểm nâng cấp
    private int score; // Điểm số
    private float size = 1; // Kích thước nhân vật
    public float Size => size; // Thuộc tính để lấy kích thước
    public List<Character> targets = new List<Character>(); // Danh sách mục tiêu
    public Character currentTarget; // Mục tiêu hiện tại
    private Vector3 targetDirection; // Hướng mục tiêu
    private Vector3 localScale; // Tỉ lệ cục bộ

    public bool isWeaponActive; // Kiểm tra vũ khí có hoạt động không
    public bool IsDead { get; protected set; } // Kiểm tra nhân vật đã chết hay chưa
    public bool IsCanAttack { get; protected set; } // Kiểm tra có thể tấn công không

    public override void OnInit()
    {
        ClearTarget(); // Xóa mục tiêu hiện tại
        OnInitItem(); // Khởi tạo đồ vật
        localScale = currentSkin.transform.localScale; // Lưu tỉ lệ hiện tại
        isWeaponActive = true; // Đặt vũ khí hoạt động
        IsDead = false; // Đặt trạng thái sống
        IsCanAttack = true; // Đặt trạng thái có thể tấn công
        scalePoint = 0; // Đặt điểm nâng cấp về 0
        score = 0; // Đặt điểm số về 0
    }

    public override void OnDespawn()
    {
        OnDespawnItem(); // Hủy đồ vật
        CancelInvoke(); // Hủy các phương thức đã được gọi
    }

    public override void OnDeath()
    {
        IsDead = true; // Đánh dấu nhân vật đã chết
        ChangeAnim(Const.ANIM_DEAD); // Đổi hoạt ảnh thành chết
        LevelManager.Ins.OnDeadEvent(this); // Thông báo sự kiện chết
        if (currentAttacker.CheckTarget(this))
        {
            currentAttacker.RemoveTarget(this); // Xóa mục tiêu nếu có
        }
    }

    public void ChangeAnim(string animName)
    {
        if (currentAnim != animName) // Kiểm tra nếu hoạt ảnh khác
        {
            currentSkin.Anim.ResetTrigger(animName); // Đặt lại trigger
            currentAnim = animName; // Cập nhật tên hoạt ảnh
            currentSkin.Anim.SetTrigger(currentAnim); // Thiết lập trigger cho hoạt ảnh
        }
    }

    public virtual void AddTarget(Character crt)
    {
        targets.Add(crt); // Thêm mục tiêu vào danh sách
    }

    public bool CheckTarget(Character crt)
    {
        return targets.Contains(crt); // Kiểm tra mục tiêu có trong danh sách không
    }

    public virtual void RemoveTarget(Character crt)
    {
        targets.Remove(crt); // Xóa mục tiêu khỏi danh sách
    }

    public virtual void ClearTarget()
    {
        currentTarget = null; // Đặt mục tiêu hiện tại thành null
        targets.Clear(); // Xóa danh sách mục tiêu
    }

    private IEnumerator IAttack()
    {
        yield return new WaitForSeconds(Const.DELAY_ATTACK); // Đợi trước khi tấn công
        currentSkin.Weapon.Throw(this, targetDirection, currentWeaponType); // Thực hiện tấn công
        SetActiveCurrentWeapon(false); // Tắt vũ khí hiện tại
    }

    public void FocusTarget()
    {
        if (currentTarget != null) // Nếu có mục tiêu
        {
            targetDirection = (currentTarget.transform.position - transform.position);
            targetDirection = new Vector3(targetDirection.x, 0, targetDirection.z); // Chỉ lấy hướng x và z
            transform.rotation = Quaternion.LookRotation(targetDirection); // Quay về hướng mục tiêu
        }
    }

    public void SetActiveCurrentWeapon(bool wp)
    {
        currentSkin.Weapon.gameObject.SetActive(wp); // Kích hoạt/tắt vũ khí
        isWeaponActive = wp; // Cập nhật trạng thái vũ khí
    }

    public void SetAttacker(Character attacker)
    {
        this.currentAttacker = attacker; // Đặt kẻ tấn công
    }

    public void ChooseTarget()
    {
        float minDistance = float.PositiveInfinity; // Khởi tạo khoảng cách tối thiểu
        currentTarget = null; // Đặt mục tiêu hiện tại thành null

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] != null && !targets[i].IsDead && Vector3.Distance(TF.position, targets[i].TF.position) <= Const.ATT_RANGE * size + targets[i].Size)
            {
                float distance = Vector3.Distance(TF.position, targets[i].TF.position); // Tính khoảng cách đến mục tiêu
                if (distance < minDistance) // Nếu khoảng cách nhỏ hơn khoảng cách tối thiểu
                {
                    minDistance = distance; // Cập nhật khoảng cách tối thiểu
                    currentTarget = targets[i]; // Cập nhật mục tiêu hiện tại
                }
            }
        }
    }

    public override void OnAttack()
    {
        ChooseTarget(); // Chọn mục tiêu

        if (currentTarget != null) // Nếu có mục tiêu
        {
            IsCanAttack = false; // Không thể tấn công ngay lập tức
            FocusTarget(); // Tập trung vào mục tiêu
            ChangeAnim(Const.ANIM_ATTACK); // Chuyển sang hoạt ảnh tấn công
            StartCoroutine(IAttack()); // Bắt đầu tấn công
            ResetAttack(); // Đặt lại trạng thái tấn công
        }
        else
        {
            ResetAttack(); // Nếu không có mục tiêu, đặt lại trạng thái tấn công
        }
    }

    public virtual void ResetAttack() { } // Phương thức đặt lại tấn công (có thể được triển khai trong lớp con)

    public void OnHit()
    {
        NotiManager.Ins.PopUpWindow(this.characterName, currentAttacker.characterName); // Hiển thị thông báo khi bị trúng đòn
        if (currentAttacker.CheckTarget(this))
        {
            currentAttacker.RemoveTarget(this); // Xóa mục tiêu nếu có
        }
        OnDeath(); // Gọi phương thức chết
    }

    public void SetBoxIndicator(bool active)
    {
        boxIndicator.SetActive(active); // Kích hoạt/tắt chỉ báo
    }

    public virtual void ScaleUp(float size)
    {
        this.size = Mathf.Clamp(size, Const.MIN_SIZE, Const.MAX_SIZE); // Giới hạn kích thước
        TF.localScale = Vector3.one * size; // Đặt kích thước mới
        ParticlePool.Play(Utilities.RandomInMember(ParticleType.LevelUp_1, ParticleType.LevelUp_2, ParticleType.LevelUp_3), TF.position); // Hiển thị hiệu ứng khi nâng cấp
    }

    public virtual void SetPoint(int point)
    {
        scalePoint += Const.POINT_UNIT; // Cập nhật điểm nâng cấp
        score += Const.SCORE_UNIT; // Cập nhật điểm số
        if (scalePoint % 2 == 0) // Nếu điểm nâng cấp chia hết cho 2
        {
            ScaleUp(1 + scalePoint / 2 * 0.1f); // Nâng cấp kích thước
        }
    }

    public void SetScore(int score)
    {
        this.score = score; // Đặt điểm số
    }

    public int GetScore()
    {
        return score; // Lấy điểm số
    }

    public Vector3 getCharacterScale()
    {
        return localScale; // Lấy tỉ lệ cục bộ
    }

    public virtual void SpeedUp()
    {
        Invoke(nameof(ResetSpeed), 5f); // Tăng tốc độ và đặt lại sau 5 giây
    }

    public virtual void ResetSpeed() { } // Phương thức đặt lại tốc độ

    public void ChangePant(PantName pantName)
    {
        currentSkin.ChangePant(pantName); // Đổi quần
    }

    public void ChangeWeapon(WeaponName weaponName)
    {
        currentWeaponType = weaponData.GetWeaponType(weaponName); // Lấy loại vũ khí từ dữ liệu
        currentSkin.ChangeWeapon(weaponName); // Đổi vũ khí
    }

    public void ChangeHat(HatName hatName)
    {
        currentSkin.ChangeHat(hatName); // Đổi mũ
    }

    public void ChangeShield(ShieldName shieldName)
    {
        currentSkin.ChangeShield(shieldName); // Đổi khiên
    }

    public void ChangeSkin(SkinType skinType)
    {
        currentSkin = SimplePool.Spawn<Skin>((PoolType)skinType, TF); // Đổi da nhân vật
    }

    public virtual void OnInitItem() { } // Phương thức khởi tạo đồ vật (có thể được triển khai trong lớp con)

    public virtual void OnDespawnItem()
    {
        currentSkin?.OnDespawn(); // Hủy da nhân vật
        SimplePool.Despawn(currentSkin); // Hủy đối tượng da
    }

    public virtual void RandomMove() { } // Phương thức di chuyển ngẫu nhiên (có thể được triển khai trong lớp con)
    public virtual void BotAttack() { } // Phương thức tấn công của bot (có thể được triển khai trong lớp con)
    public virtual void StartReset() { } // Phương thức này có thể được triển khai trong lớp con
    public void ResetAnim()
    {
        currentAnim = "";
    }

    public override void OnMoveStop() { } // Phương thức dừng di chuyển (có thể được triển khai trong lớp con)
}
