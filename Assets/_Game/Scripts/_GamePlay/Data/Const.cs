using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class Const
{
    public const string PLAYER_TAG = "Player";
    public const string BOT_TAG = "Bot";
    public const string OBSTACLE_TAG = "Obstacle";
    public const string CHARACTER_TAG = "Character";
    public const string BULLET_TAG = "Bullet";
    public const string ANIM_IDLE = "idle";
    public const string ANIM_DEAD = "dead";
    public const string ANIM_RUN = "run";
    public const string ANIM_ULTI = "ulti";
    public const string ANIM_ATTACK = "attack";
    public const string ANIM_DANCE = "dance";

    public const string NEXT_PLATFORM = "NPlatform";
    public const string WIN_PLATFORM = "WPlatform";
    public const string START_PLATFORM = "SPlatform";

    public const float BRICK_FORCE = 10f;
    public const float BRICK_HEIGHT = 0.35f;

    public const float DELAY_ATTACK = 0.2f; // Delay attack after Animation of character 
    public const float ATT_RANGE = 5f;
    public const float BOT_SPEED = 3.5f;
    public const float PLAYER_SPEED = 5f;

    public const int SCORE_UNIT = 50;
    public const int POINT_UNIT = 1;

    public const float MAX_SIZE = 4f;
    public const float MIN_SIZE = 1f;
}

public enum GameState { GamePlay, MainMenu , Finish, Revive, Setting }

public enum BulletType
{
    B_Hammer_1 = PoolType.B_Hammer_1,
    B_Hammer_2 = PoolType.B_Hammer_2,
    B_Hammer_3 = PoolType.B_Hammer_3,
    B_Candy_1 = PoolType.B_Candy_1,
    B_Candy_2 = PoolType.B_Candy_2,
    B_Candy_3 = PoolType.B_Candy_3,
    B_Boomerang_1 = PoolType.B_Boomerang_1,
    B_Boomerang_2 = PoolType.B_Boomerang_2,
    B_Boomerang_3 = PoolType.B_Boomerang_3,
    B_Knife_1 = PoolType.B_Knife_1,
}

public enum SkinType { 
    Normal = PoolType.SKIN_Normal,
    Devil = PoolType.SKIN_Devil,
    Angle = PoolType.SKIN_Angle,
    Thor = PoolType.SKIN_Thor,
    DeadPool = PoolType.SKIN_Deadpool,
    Witch = PoolType.SKIN_Witch
}
