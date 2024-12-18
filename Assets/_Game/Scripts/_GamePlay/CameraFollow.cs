using UnityEngine;

public class CameraFollow : Singleton<CameraFollow>
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;

    [Header("Rotation")]
    [SerializeField] Vector3 playerRotate;
    [SerializeField] Vector3 gamePlayRotate;

    [Header("Offset")]
    [SerializeField] Vector3 scaleUpOffset;
    [SerializeField] Vector3 playerOffset;
    [SerializeField] Vector3 offsetMax;
    [SerializeField] Vector3 offsetMin;

    [SerializeField] float moveSpeed = 5f;
    private Vector3 targetOffset;
    private Quaternion targetRotate;

    [SerializeField] Transform[] offsets;
    public Camera Camera { get; private set; }
    private void Awake()
    {
        Camera = Camera.main;
    }

    void Start()
    {
    }

    void LateUpdate()
    {
        offset = Vector3.Lerp(offset, targetOffset, Time.deltaTime * moveSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotate, moveSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, player.position + targetOffset, moveSpeed*Time.deltaTime);
    }

    public void ScaleOffset(float size)
    {
        if (size > 0)
            targetOffset = targetOffset + scaleUpOffset;
    }

    public void ChangeState(GameState state)
    {
        targetOffset = offsets[(int)state].localPosition;
        targetRotate = offsets[(int)state].localRotation;
        return;
    }
}