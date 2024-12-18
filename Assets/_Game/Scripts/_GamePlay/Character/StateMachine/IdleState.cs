using UnityEngine;

public class IdleState : IState
{
    private float timeInterval = 3f;
    private float countTime = 0f;

    public void OnEnter(Bot t)
    {
        countTime = 0f; // Đặt lại biến đếm thời gian
        t.ChangeAnim(Const.ANIM_IDLE); // Thay đổi hoạt ảnh thành đứng yên
    }

    public void OnExecute(Bot t)
    {
        countTime += Time.deltaTime; // Cộng dồn thời gian đã trôi qua
        // Kiểm tra nếu thời gian đã trôi qua lớn hơn thời gian quy định
        if (countTime > timeInterval)
            t.ChangeState(new PatrolState()); // Chuyển sang trạng thái tuần tra
    }

    public void OnExit(Bot t)
    {
        
    }
}
