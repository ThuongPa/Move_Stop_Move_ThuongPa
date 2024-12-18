
public class GameManager : Singleton<GameManager>
{
    private GameState gameState;

    public void ChangeState(GameState gameState)
    {
        this.gameState = gameState;
    }

    public bool IsState(GameState gameState) => this.gameState == gameState;

    private void Awake()
    {
        DataManager.Ins.InitUserData();
    }

    private void Start()
    {
        UIManager.Ins.OpenUI<UIMainMenu>();
    }
}
