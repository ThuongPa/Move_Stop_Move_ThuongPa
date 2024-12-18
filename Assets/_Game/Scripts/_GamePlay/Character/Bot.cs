using Scriptable;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character // Lớp Bot kế thừa từ lớp Character
{
    [SerializeField] private NavMeshAgent agent; // Đối tượng NavMeshAgent để điều khiển di chuyển
    [SerializeField] private IState currentState; // Trạng thái hiện tại của bot
    public Transform obj; // Đối tượng liên quan
    private Vector3 destination; // Điểm đến mà bot sẽ di chuyển tới
    private Vector3 direction; // Hướng di chuyển
    private Vector3 randomDirection3D; // Hướng ngẫu nhiên 3D
    private Vector2 randomDirection2D; // Hướng ngẫu nhiên 2D
    private int randomDistance; // Khoảng cách ngẫu nhiên

    // Kiểm tra xem bot có thể chạy hay không
    private bool IsCanRunning => (GameManager.Ins.IsState(GameState.GamePlay) || GameManager.Ins.IsState(GameState.Revive) || GameManager.Ins.IsState(GameState.Setting));

    // Phương thức khởi tạo
    public override void OnInit()
    {
        base.OnInit();
        SetBoxIndicator(false);
        ResetAnim();
    }

    // Phương thức cập nhật mỗi khung hình
    public void Update()
    {
        if (IsDead || !IsCanRunning) return; // Nếu bot chết hoặc không thể chạy, thoát khỏi phương thức
        if (currentState != null)
        {
            currentState.OnExecute(this); // Thực hiện hành động của trạng thái hiện tại
        }
    }

    // Thiết lập điểm đến cho bot
    public void SetDestination(Vector3 des)
    {
        this.destination = des; // Gán điểm đến
        agent.enabled = true; // Kích hoạt NavMeshAgent
        agent.SetDestination(destination); // Thiết lập điểm đến cho agent
    }

    // Phương thức di chuyển ngẫu nhiên
    public override void RandomMove()
    {
        if (IsDead) return; // Nếu bot chết, thoát khỏi phương thức
        SetDestination(LevelManager.Ins.RandomPoint()); // Thiết lập điểm đến ngẫu nhiên
        ChangeAnim(Const.ANIM_RUN); // Thay đổi hoạt ảnh thành chạy
    }

    // Dừng di chuyển của bot
    public override void OnMoveStop()
    {
        base.OnMoveStop(); // Gọi phương thức dừng di chuyển của lớp cha
        agent.enabled = false; // Vô hiệu hóa NavMeshAgent
    }

    // Dừng di chuyển ngẫu nhiên
    public void StopRandomMove()
    {
        // Kiểm tra khoảng cách đến điểm đến
        if (Vector3.Distance(TF.position, destination) - Mathf.Abs(TF.position.y - destination.y) < 0.1f)
        {
            agent.enabled = false; // Vô hiệu hóa NavMeshAgent
            ChangeAnim(Const.ANIM_IDLE); // Thay đổi hoạt ảnh thành đứng yên
            ChangeState(new IdleState()); // Chuyển sang trạng thái đứng yên
        }
    }

    // Thêm mục tiêu cho bot
    public override void AddTarget(Character crt)
    {
        base.AddTarget(crt); // Gọi phương thức thêm mục tiêu của lớp cha
        if (!crt.IsDead && IsCanAttack && !this.IsDead && IsCanRunning)
        {
            ChangeState(new AttackState()); // Nếu mục tiêu còn sống và bot có thể tấn công, chuyển sang trạng thái tấn công
        }
    }

    // Phương thức tấn công của bot
    public override void BotAttack()
    {
        SetDestination(transform.position); // Đặt điểm đến là vị trí hiện tại
    }

    // Đặt lại trạng thái tấn công
    public override void ResetAttack()
    {
        CancelInvoke(nameof(ResetAttack)); // Hủy bỏ lệnh gọi lại phương thức ResetAttack
        Invoke(nameof(StartReset), 2f); // Gọi phương thức StartReset sau 2 giây
    }

    // Bắt đầu đặt lại trạng thái
    public override void StartReset()
    {
        base.StartReset(); // Gọi phương thức khởi tạo lại của lớp cha
        IsCanAttack = true; // Cho phép bot tấn công
        ChangeState(new PatrolState()); // Chuyển sang trạng thái tuần tra
    }

    // Xử lý khi bot chết
    public override void OnDeath()
    {
        agent.enabled = false; // Vô hiệu hóa NavMeshAgent
        ChangeState(null); // Đặt trạng thái hiện tại thành null
        base.OnDeath(); // Gọi phương thức xử lý chết của lớp cha
        Invoke(nameof(OnDespawn), 2f); // Gọi phương thức OnDespawn sau 2 giây
    }

    // Xử lý khi bot bị loại bỏ
    public override void OnDespawn()
    {
        base.OnDespawn(); // Gọi phương thức loại bỏ của lớp cha
        SimplePool.Despawn(this); // Đưa bot về pool
    }

    // Tăng tốc độ di chuyển của bot
    public override void SpeedUp()
    {
        agent.speed = 5f; // Thiết lập tốc độ của agent
        base.SpeedUp(); // Gọi phương thức tăng tốc độ của lớp cha
    }

    // Đặt lại tốc độ di chuyển của bot
    public override void ResetSpeed()
    {
        base.ResetSpeed(); // Gọi phương thức đặt lại tốc độ của lớp cha
        agent.speed = Const.BOT_SPEED; // Đặt tốc độ về giá trị mặc định
    }

    // Thay đổi trạng thái của bot
    public virtual void ChangeState(IState state)
    {
        if (currentState != null)
        {
            currentState.OnExit(this); // Gọi phương thức thoát của trạng thái hiện tại
        }

        currentState = state; // Cập nhật trạng thái hiện tại

        if (currentState != null)
        {
            currentState.OnEnter(this); // Gọi phương thức vào của trạng thái mới
        }
    }

    // Khởi tạo các mục cho bot
    public override void OnInitItem()
    {
        base.OnInitItem(); // Gọi phương thức khởi tạo mục của lớp cha
        ChangeSkin(Utilities.RandomEnumValue<SkinType>()); // Thay đổi da ngẫu nhiên
        ChangeShield(Utilities.RandomEnumValue<ShieldName>()); // Thay đổi khiên ngẫu nhiên
        ChangeHat(Utilities.RandomEnumValue<HatName>()); // Thay đổi mũ ngẫu nhiên
        ChangePant(Utilities.RandomEnumValue<PantName>()); // Thay đổi quần ngẫu nhiên
        ChangeWeapon(Utilities.RandomEnumValue<WeaponName>()); // Thay đổi vũ khí ngẫu nhiên
    }
}
