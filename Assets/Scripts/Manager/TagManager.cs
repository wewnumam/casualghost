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
    public const string EXPLOSION = "Explosion";
    public const string COLLECTIBLE_ITEM = "CollectibleItem";
    public const string HEAL_AREA = "HealArea";
    public const string COLLECTIBLE_ITEM_BAR = "CollectibleItemBar";
    public const string PUDDLE = "Puddle";
    public const string TRAP_ROOT = "TrapRoot";
}

public class AnimationTags {
    public const string PLAYER_IDLE = "PlayerIdle";
    public const string PLAYER_WALK = "PlayerWalk";
    public const string PLAYER_WALK_TYPE_TWO = "PlayerWalkTypeTwo";
    public const string PLAYER_WALK_TYPE_THREE = "PlayerWalkTypeThree";
    public const string PLAYER_RELOAD = "PlayerReload";
    public const string PLAYER_SHOOT = "PlayerShoot";
    public const string PLAYER_HURT = "PlayerHurt";
    public const string PLAYER_DIE = "PlayerDie";
    public const string PLAYER_ECHO = "PlayerEcho";
    public const string PLAYER_AMMO_EMPTY = "PlayerAmmoEmpty";
    public const string PLAYER_POWER_UP = "PlayerPowerUp";
    public const string PLAYER_RELOAD_TIME = "reloadTime";
    public const string ENEMY_DEFAULT_IDLE = "EnemyDefaultIdle";
    public const string ENEMY_DEFAULT_WALK = "EnemyDefaultWalk";
    public const string ENEMY_DEFAULT_HURT = "EnemyDefaultHurt";
    public const string ENEMY_DEFAULT_DIE = "EnemyDefaultDie";
    public const string ENEMY_DEFAULT_ATTACK = "EnemyDefaultAttack";
    public const string ENEMY_BIG_IDLE = "EnemyBigIdle";
    public const string ENEMY_BIG_WALK = "EnemyBigWalk";
    public const string ENEMY_BIG_HURT = "EnemyBigHurt";
    public const string ENEMY_BIG_DIE = "EnemyBigDie";
    public const string ENEMY_BIG_ATTACK = "EnemyBigAttack";
    public const string ENEMY_SMALL_IDLE = "EnemySmallIdle";
    public const string ENEMY_SMALL_WALK = "EnemySmallWalk";
    public const string ENEMY_SMALL_HURT = "EnemySmallHurt";
    public const string ENEMY_SMALL_DIE = "EnemySmallDie";
    public const string ENEMY_SMALL_ATTACK = "EnemySmallAttack";
    public const string ENEMY_SHOOTER_IDLE = "EnemyShooterIdle";
    public const string ENEMY_SHOOTER_WALK = "EnemyShooterWalk";
    public const string ENEMY_SHOOTER_HURT = "EnemyShooterHurt";
    public const string ENEMY_SHOOTER_DIE = "EnemyShooterDie";
    public const string ENEMY_SHOOTER_AIM = "EnemyShooterAim";
    public const string ENEMY_SHOOTER_SHOOT = "EnemyShooterShoot";
    public const string ENEMY_BOSS_IDLE = "EnemyBossIdle";
    public const string ENEMY_BOSS_WALK = "EnemyBossWalk";
    public const string ENEMY_BOSS_HURT = "EnemyBossHurt";
    public const string ENEMY_BOSS_DIE = "EnemyBossDie";
    public const string ENEMY_BOSS_ATTACK = "EnemyBossAttack";
    public const string SWITCH_FIRST = "SwitchFirst";
    public const string SWITCH_SECOND = "SwitchSecond";
    public const string INVENTORY_OPEN = "InventoryOpen";
    public const string INVENTORY_CLOSE = "InventoryClose";
    public const string LOADING = "Loading";
    public const string BUILDING_HURT = "BuildingHurt";
    public const string DECOY_ATTACKED = "DecoyAttacked";
    public const string THORN_MINE_IDLE = "ThornMineIdle";
    public const string THORN_MINE_ATTACK = "ThornMineAttack";
}

public class PlayerPrefsKeys {
    public const string GEMS = "gems";
    public const string WEAPON = "weapon";
    public const string PLAYER = "player";
    public const string HIGH_SCORE = "highScore";
    public const string WIN_COUNTER = "winCounter";
    public const string TUTORIAL_COMPLETED_COUNTER = "tutorialCompletedCounter";
    public const string ENEMY_KILLED_COUNTER = "enemyKilledCounter";
    public const string GEMS_CLAIMED_COUNTER = "gemsClaimedCounter";
    public const string LEVEL_PLAYED_COUNTER = "levelPlayedCounter";
    public const string IS_INTRO_STORY_CUTSCENE_ALREADY_PLAYED = "isIntroStoryCutsceneAlreadyPlayed";
    public const string IS_INTRO_STORY_CUTSCENE_TO_GAMEPLAY_CALLED = "isIntroStoryCutsceneToGameplayCalled";
    public const string IS_THORNMINE_UNLOCKED = "isThornMineUnlocked";
    public const string IS_DECOY_UNLOCKED = "isDecoyUnlocked";
    public const string IS_CANNON_UNLOCKED = "isCannonUnlocked";
    public const string IS_SHOTGUN_UNLOCKED = "isShootgunUnlocked";
    public const string IS_RIFLE_UNLOCKED = "isRifleUnlocked";
    public const string IS_PLAYER_TYPE_TWO_UNLOCKED = "isPlayerTypeTwoUnlocked";
    public const string IS_PLAYER_TYPE_THREE_UNLOCKED = "isPlayerTypeThreeUnlocked";
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