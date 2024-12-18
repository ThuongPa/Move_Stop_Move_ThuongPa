using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public Player player;

    [SerializeField]private List<Bot> bots = new List<Bot>();
    [SerializeField]private List<Booster> boosters = new List<Booster>();

    [SerializeField] Level[] levels;
    public Level currentLevel;
    private const float BOOSTER_INTERVAL = 10f;
    private int totalBot;
    private bool isRevive;

    private int levelIndex;
    private float countBooster;

    public int TotalCharater => totalBot + bots.Count + 1;

    public void Start()
    {
        levelIndex = 0;
        OnLoadLevel(levelIndex);
        OnInit();
    }

    private void Update()
    {
        if (!GameManager.Ins.IsState(GameState.GamePlay)) return;
        countBooster += Time.deltaTime;
        if (countBooster > BOOSTER_INTERVAL)
        {
            DropBooster(RandomPoint() + new Vector3(0,10,0));
            countBooster = 0;
        }
    }

    public void OnInit()
    {
        player.OnInit();

        for (int i = 0; i < currentLevel.botReal; i++)
        {
            NewBot(null);
        }

        totalBot = currentLevel.botTotal - currentLevel.botReal - 1;

        isRevive = false;
    }

    public void OnReset()
    {
        player.OnDespawn();
        NotiManager.Ins.OnDespawn();
        for (int i = 0; i < bots.Count; i++)
        {
            bots[i].OnDespawn();
        }
        for(int i = 0; i < boosters.Count; i++)
        {
            SimplePool.Despawn(boosters[i]);
        }

        bots.Clear();
        SimplePool.CollectAll();
    }

    public void OnLoadLevel(int level)
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }

        currentLevel = Instantiate(levels[level]);
    }

    public Vector3 RandomPoint()
    {
        Vector3 randPoint = Vector3.zero;

        float size = Const.ATT_RANGE + Const.MAX_SIZE + 1f;

        for (int t = 0; t < 50; t++)
        {

            randPoint = currentLevel.RandomPoint();
            if (Vector3.Distance(randPoint, player.TF.position) < size)
            {
                continue;
            }

            for (int j = 0; j < 20; j++)
            {
                for (int i = 0; i < bots.Count; i++)
                {
                    if (Vector3.Distance(randPoint, bots[i].TF.position) < size)
                    {
                        break;
                    }
                }

                if (j == 19)
                {
                    return randPoint;
                }
            }
        }
        return randPoint;
    }

    private void NewBot(IState state)
    {
        Bot bot = SimplePool.Spawn<Bot>(PoolType.Bot, RandomPoint(), Quaternion.identity);
        bot.ScaleUp(player.Size>1?(Utilities.Chance(50,100)?(player.Size+0.1f):(player.Size-0.1f)):1);
        bot.OnInit();
        bot.ChangeState(state);
        bots.Add(bot);
    }

    public void OnDeadEvent(Character c)
    {
        if (c is Player)
        {
            UIManager.Ins.CloseAll();
            if (!isRevive)
            {
                isRevive = true;
                UIManager.Ins.OpenUI<UIRevive>();
            }
            else
            {
                Fail();
            }
        }
        else
        if (c is Bot)
        {
            bots.Remove(c as Bot);
            if (totalBot > 0)
            {
                totalBot--;
                NewBot(new IdleState());
            }
           if(bots.Count == 0)
            {
                Victory();
            }
        }
        UIManager.Ins.GetUI<UIGameplay>().UpdatePlayerRemain();
    }

    private void Victory()
    {
        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<UIVictory>().SetCoin(player.GetScore());
        player.ChangeAnim(Const.ANIM_DANCE);
    }

    public void Fail()
    {
        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<UIFail>().SetCoin(player.GetScore());
    }
   
    public void Home()
    {
        UIManager.Ins.CloseAll();
        OnReset();
        OnLoadLevel(levelIndex);
        OnInit();
        UIManager.Ins.OpenUI<UIMainMenu>();
    }
      
    public void NextLevel()
    {
        levelIndex++;
    }

    public void OnPlay()
    {
        for (int i = 0; i < bots.Count; i++)
        {
            bots[i].ChangeState(new PatrolState());
        }
    }

    public void OnRevive()
    {
        player.TF.position = RandomPoint();
        player.OnRevive();
    }
    public void DropBooster(Vector3 pos)
    {
        Booster newBooster = SimplePool.Spawn<Booster>(PoolType.Booster, pos, Quaternion.identity) ;
        boosters.Add(newBooster);
    }
}
