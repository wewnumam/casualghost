using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tags {
    public const string PLAYER = "Player";
    public const string BANYAN = "Banyan";
    public const string ENEMY = "Enemy";
    public const string BULLET_TYPE_ONE = "BulletTypeOne";
    public const string COIN = "Coin";
    public const string THORN_MINE = "ThornMine";
    public const string ROOT = "Root";
    public const string DECOY = "Decoy";
    public const string CANON = "Canon";
}

public class AnimationTags {
    public const string PLAYER_IDLE = "PlayerIdle";
    public const string PLAYER_RELOAD = "PlayerReload";
    public const string PLAYER_SHOOT = "PlayerShoot";
    public const string PLAYER_HURT = "PlayerHurt";
    public const string PLAYER_RELOAD_TIME = "reloadTime";
    public const string ENEMY_IDLE = "EnemyIdle";
    public const string ENEMY_HURT = "EnemyHurt";
    public const string SWITCH_FIRST = "SwitchFirst";
    public const string SWITCH_SECOND = "SwitchSecond";
}

public class PlayerPrefsKeys {
    public const string GEMS = "gems";
    public const string IS_INTRO_STORY_CUTSCENE_ALREADY_PLAYED = "isIntroStoryCutsceneAlreadyPlayed";
    public const string IS_THORNMINE_UNLOCKED = "isThornMineUnlocked";
    public const string IS_DECOY_UNLOCKED = "isDecoyUnlocked";
    public const string IS_CANON_UNLOCKED = "isCanonUnlocked";
}

public class PlayerPrefsValues {
    public const int FALSE = 0;
    public const int TRUE = 1;
}

public class SceneNames {
    public const string INTRO_STORY = "IntroStory";
    public const string GAMEPLAY = "Gameplay";
}