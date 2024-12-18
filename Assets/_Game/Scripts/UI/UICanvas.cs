using UnityEngine;

public class UICanvas : MonoBehaviour
{
    public bool IsDestroyOnClose = false;
    protected RectTransform m_RectTransform;
    private Animator m_Animator;
    private float m_OffsetY = 0;

    private void Start()
    {
        OnInit();
    }

    //Khởi tạo giá trị canvas
    protected void OnInit()
    {
        m_RectTransform = GetComponent<RectTransform>();
        m_Animator = GetComponent<Animator>();

        // xu ly tai tho
        float ratio = (float)Screen.height / (float)Screen.width;
        if (ratio > 2.1f)
        {
            Vector2 leftBottom = m_RectTransform.offsetMin;
            Vector2 rightTop = m_RectTransform.offsetMax;
            rightTop.y = -100f;
            m_RectTransform.offsetMax = rightTop;
            leftBottom.y = 0f;
            m_RectTransform.offsetMin = leftBottom;
            m_OffsetY = 100f;
        }
    }

    //Set up mặc định cho UI 
    public virtual void Setup()
    {
        UIManager.Ins.AddBackUI(this);
        UIManager.Ins.PushBackAction(this, BackKey);
    }

    //Back key dành cho android
    public virtual void BackKey()
    {

    }

    //Mở canvas
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    //Đóng trực tiếp
    public virtual void CloseDirectly()
    {
        UIManager.Ins.RemoveBackUI(this);
        gameObject.SetActive(false);
        if (IsDestroyOnClose)
        {
            Destroy(gameObject);
        }
    }

    //Đóng canvas sau một khoảng thời gian delay
    public virtual void Close(float delayTime)
    {
        Invoke(nameof(CloseDirectly), delayTime);
    }
}
