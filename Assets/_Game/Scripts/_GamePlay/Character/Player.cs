using Scriptable;
using System;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private float moveSpeed = 300f; // Tốc độ di chuyển của nhân vật
    [SerializeField] private Rigidbody rb; // RigidBody để xử lý vật lý
    public VariableJoystick variableJoystick; // Joystick để điều khiển
    private Vector3 moveVector; // Vector lưu trữ hướng di chuyển
    private Vector3 direction; // Hướng di chuyển
    private float rotateSpeed = 10f; // Tốc độ quay của nhân vật

    // Các thuộc tính về trang phục và vũ khí
    SkinType skinType = SkinType.Normal;
    WeaponName weaponName = WeaponName.Boomerang;
    HatName hatName = HatName.Arrow;
    PantName pantName = PantName.Dabao;
    ShieldName shieldName = ShieldName.Khien;

    // Kiểm tra xem nhân vật có thể cập nhật trạng thái hay không
    private bool IsCanUpdate => GameManager.Ins.IsState(GameState.GamePlay) || GameManager.Ins.IsState(GameState.Setting);

    public void Update()
    {
        Move(); // Gọi phương thức di chuyển
    }

    private void Move()
    {
        // Kiểm tra joystick và trạng thái sống của nhân vật
        if (variableJoystick != null && !IsDead && IsCanUpdate)
        {
            moveVector.x = variableJoystick.Horizontal; // Lấy hướng ngang từ joystick
            moveVector.z = variableJoystick.Vertical; // Lấy hướng dọc từ joystick
            direction = Vector3.RotateTowards(transform.forward, moveVector, rotateSpeed * Time.deltaTime, 0.0f); // Tính toán hướng di chuyển

            // Nếu có di chuyển và nhấn chuột trái
            if (moveVector.magnitude > 0.1f && Input.GetMouseButton(0))
            {
                StopAllCoroutines(); // Dừng tất cả Coroutine
                IsCanAttack = true; // Cho phép tấn công
                ChangeAnim(Const.ANIM_RUN); // Đổi animation sang chạy
                transform.rotation = Quaternion.LookRotation(direction); // Quay nhân vật theo hướng di chuyển
                Vector3 targetPosition = transform.position + direction; // Tính vị trí mục tiêu

                // Di chuyển nhân vật
                rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
                TF.position = rb.position; // Cập nhật vị trí
            }
            else
            {
                OnAttack(); // Gọi phương thức tấn công
                ChangeAnim(Const.ANIM_IDLE); // Đổi animation sang đứng yên
                rb.velocity = Vector3.zero; // Đặt vận tốc về 0
            }
        }
        else
        {
            rb.velocity = Vector3.zero; // Nếu không có joystick, đặt vận tốc về 0
        }
    }

    public override void OnInit()
    {
        characterName = "You"; // Tên nhân vật
        OnTakeUserData(); // Lấy dữ liệu người chơi
        base.OnInit(); // Gọi phương thức khởi tạo của lớp cha
        ScaleUp(1); // Đặt kích thước mặc định của nhân vật
        variableJoystick = null; // Khởi tạo joystick
        IsDead = false; // Nhân vật sống
        ChangeAnim(Const.ANIM_IDLE); // Đổi animation sang đứng yên
    }

    public override void OnDeath()
    {
        base.OnDeath(); // Gọi phương thức chết của lớp cha
    }

    public override void OnAttack()
    {
        if (IsCanAttack) // Nếu có thể tấn công
        {
            base.OnAttack(); // Gọi phương thức tấn công của lớp cha
        }
    }

    public void SetVariableJoyStick(VariableJoystick Vj)
    {
        this.variableJoystick = Vj; // Gán joystick
    }

    public override void ResetAttack()
    {
        base.ResetAttack(); // Gọi phương thức đặt lại tấn công của lớp cha
        CancelInvoke(nameof(StartReset)); // Hủy lời gọi phương thức StartReset
        Invoke(nameof(StartReset), 3f); // Gọi lại phương thức StartReset sau 3 giây
    }

    public override void StartReset()
    {
        base.StartReset(); // Gọi phương thức bắt đầu lại của lớp cha
        IsCanAttack = true; // Cho phép tấn công
        SetActiveCurrentWeapon(true); // Kích hoạt vũ khí hiện tại
    }

    public override void ScaleUp(float size)
    {
        base.ScaleUp(size); // Gọi phương thức thay đổi kích thước của lớp cha
        CameraFollow.Ins.ScaleOffset(size); // Thay đổi kích thước camera theo nhân vật
    }

    public override void AddTarget(Character crt)
    {
        if (!crt.IsDead) // Nếu mục tiêu không chết
        {
            base.AddTarget(crt); // Gọi phương thức thêm mục tiêu của lớp cha
            crt.SetBoxIndicator(true); // Hiện chỉ báo cho mục tiêu
        }
    }

    // Phương thức hồi sinh
    internal void OnRevive()
    {
        ChangeAnim(Const.ANIM_IDLE); // Đổi animation sang đứng yên
        IsDead = false; // Đánh dấu nhân vật sống lại
        ClearTarget(); // Xóa mục tiêu
        //reviveVFX.Play(); // Chơi hiệu ứng hồi sinh (nếu cần)
    }

    // Xóa mục tiêu
    public override void RemoveTarget(Character crt)
    {
        base.RemoveTarget(crt); // Gọi phương thức xóa mục tiêu của lớp cha
        crt.SetBoxIndicator(false); // Ẩn chỉ báo cho mục tiêu
    }

    // Khởi tạo các vật phẩm cho nhân vật
    public override void OnInitItem()
    {
        base.OnInitItem(); // Gọi phương thức khởi tạo vật phẩm của lớp cha
        ChangeSkin(skinType); // Thay đổi trang phục
        ChangeHat(hatName); // Thay đổi mũ
        ChangePant(pantName); // Thay đổi quần
        ChangeWeapon(weaponName); // Thay đổi vũ khí
        ChangeShield(shieldName); // Thay đổi khiên
    }

    // Tăng tốc độ di chuyển
    public override void SpeedUp()
    {
        base.SpeedUp(); // Gọi phương thức tăng tốc độ của lớp cha
        this.moveSpeed = Const.PLAYER_SPEED + 5f; // Tăng tốc độ di chuyển lên 5
    }

    // Đặt lại tốc độ di chuyển
    public override void ResetSpeed()
    {
        base.ResetSpeed(); // Gọi phương thức đặt lại tốc độ của lớp cha
        moveSpeed = Const.PLAYER_SPEED; // Đặt tốc độ về giá trị mặc định
    }

    // Lấy dữ liệu người chơi
    public void OnTakeUserData()
    {
        weaponName = DataManager.Ins.playerData.playerWeapon; // Lấy vũ khí
        hatName = DataManager.Ins.playerData.playerHat; // Lấy mũ
        skinType = DataManager.Ins.playerData.playerSkin; // Lấy trang phục
        pantName = DataManager.Ins.playerData.playerPant; // Lấy quần
        shieldName = DataManager.Ins.playerData.playerShield; // Lấy khiên
    }

    // Thêm điểm cho nhân vật
    public override void SetPoint(int point)
    {
        base.SetPoint(point); // Gọi phương thức thêm điểm của lớp cha
        SimplePool.Spawn<FlyCoin>(PoolType.VFX_FlyCoin, transform.parent); // Hiện hiệu ứng điểm
    }

    // Thay đổi trang phục
    public void TryCloth(UIShop.ShopType shopType, Enum type)
    {
        switch (shopType) // Kiểm tra loại cửa hàng
        {
            case UIShop.ShopType.Hat:
                currentSkin.DespawnHat(); // Xóa mũ hiện tại
                ChangeHat((HatName)type); // Thay đổi mũ
                break;

            case UIShop.ShopType.Pant:
                ChangePant((PantName)type); // Thay đổi quần
                break;

            case UIShop.ShopType.Shield:
                currentSkin.DespawnShield(); // Xóa khiên hiện tại
                ChangeShield((ShieldName)type); // Thay đổi khiên
                break;

            case UIShop.ShopType.Skin:
                OnDespawnItem(); // Xóa vật phẩm hiện tại
                skinType = (SkinType)type; // Thay đổi trang phục
                OnInitItem(); // Khởi tạo lại vật phẩm
                break;

            case UIShop.ShopType.Weapon:
                currentSkin.DespawnWeapon(); // Xóa vũ khí hiện tại
                ChangeWeapon((WeaponName)type); // Thay đổi vũ khí
                break;

            default:
                break;
        }
    }
}
