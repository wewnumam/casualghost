using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tags {
    public const string PLAYER = "Player";
    public const string BANYAN = "Banyan";
    public const string ENEMY = "Enemy";
    public const string BULLET_TYPE_ONE = "BulletTypeOne";
    public const string BULLET_TYPE_TWO = "BulletTypeTwo";
    public const string COIN = "Coin";
    public const string THORN_MINE = "ThornMine";
    public const string ROOT = "Root";
    public const string DECOY = "Decoy";
    public const string CANNON = "Cannon";
    public const string ENEMY_BULLET = "EnemyBullet";
    public const string BUILDING_BUILDER = "BuildingBuilder";
    public const string TREE = "Tree";
    public const string MIST = "Mist";
}

public class AnimationTags {
    public const string PLAYER_IDLE = "PlayerIdle";
    public const string PLAYER_WALK = "PlayerWalk";
    public const string PLAYER_RELOAD = "PlayerReload";
    public const string PLAYER_SHOOT = "PlayerShoot";
    public const string PLAYER_HURT = "PlayerHurt";
    public const string PLAYER_DIE = "PlayerDie";
    public const string PLAYER_AMMO_EMPTY = "PlayerAmmoEmpty";
    public const string PLAYER_POWER_UP = "PlayerPowerUp";
    public const string PLAYER_RELOAD_TIME = "reloadTime";
    public const string ENEMY_DEFAULT_IDLE = "EnemyDefaultIdle";
    public const string ENEMY_DEFAULT_WALK = "EnemyDefaultWalk";
    public const string ENEMY_DEFAULT_HURT = "EnemyDefaultHurt";
    public const string ENEMY_DEFAULT_DIE = "EnemyDefaultDie";
    public const string ENEMY_DEFAULT_ATTACK = "EnemyDefaultAttack";
    public const string SWITCH_FIRST = "SwitchFirst";
    public const string SWITCH_SECOND = "SwitchSecond";
    public const string INVENTORY_OPEN = "InventoryOpen";
    public const string INVENTORY_CLOSE = "InventoryClose";
}

public class PlayerPrefsKeys {
    public const string GEMS = "gems";
    public const string WEAPON = "weapon";
    public const string IS_INTRO_STORY_CUTSCENE_ALREADY_PLAYED = "isIntroStoryCutsceneAlreadyPlayed";
    public const string IS_THORNMINE_UNLOCKED = "isThornMineUnlocked";
    public const string IS_DECOY_UNLOCKED = "isDecoyUnlocked";
    public const string IS_CANNON_UNLOCKED = "isCannonUnlocked";
    public const string IS_SHOTGUN_UNLOCKED = "isShootgunUnlocked";
    public const string IS_RIFLE_UNLOCKED = "isRifleUnlocked";
    public const string IS_TUTORIAL_MOVE_DONE = "isTutorialMoveDone";
    public const string IS_TUTORIAL_SHOOT_DONE = "isTutorialShootDone";
    public const string IS_TUTORIAL_COIN_DONE = "isTutorialCoinDone";
    public const string IS_TUTORIAL_BUILD_DONE = "isTutorialBuildDone";
    public const string IS_TUTORIAL_UNLOCK_DONE = "isTutorialUnlockDone";
    public const string BGM_SLIDER = "BGMSlider";
    public const string SFX_SLIDER = "SFXSlider";
}

public class PlayerPrefsValues {
    public const int FALSE = 0;
    public const int TRUE = 1;
    public const int WEAPON_DEFAULT = 0;
    public const int WEAPON_SHOTGUN = 1;
    public const int WEAPON_RIFLE = 2;
}

public class SceneNames {
    public const string INTRO_STORY = "IntroStory";
    public const string GAMEPLAY = "Gameplay";
}