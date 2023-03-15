using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumsManager {
    public enum BuildingType {
        ROOT,
        CLEAR_BUILDING,
        THORN_MINE,
        DECOY,
        CANON
    }

    public enum GameState {
        MAINMENU,
        GAMEPLAY,
        PAUSE,
        LEVELTRANSITION,
        GAMEOVER,
        REWARD
    }

    public enum LevelState {
        LEVEL_1,
        LEVEL_2,
        LEVEL_3,
        LEVEL_4,
        LEVEL_5,
        LEVEL_6,
        LEVEL_7,
        LEVEL_8,
        LEVEL_9,
        LEVEL_10
    }

    public enum PlayerState {
        SHOOT,
        BUILD
    }

    public enum SoundEffect {
        _BGM_CREDIT_PANEL,
        _BGM_GAME_OVER,
        _BGM_GAMEPLAY_1,
        _BGM_GAMEPLAY_2,
        _BGM_GAMEPLAY_3,
        _BGM_INTRO_STORY_MAGICAL,
        _BGM_INTRO_STORY_TENSE,
        _BGM_MAINMENU,
        BUILDING_DROP,
        BUTTON_CLICK,
        BUTTON_DISABLED,
        BUY_BUILDING,
        CANON_SHOOT,
        COLLECT_COIN,
        ENEMY_BLOOD_1,
        ENEMY_BLOOD_2,
        ENEMY_BLOOD_3,
        ENEMY_DIE_BIG,
        ENEMY_DIE_DEFAULT,
        ENEMY_DIE_SMALL,
        ENEMY_SHOOT,
        ENEMY_SPAWN_BIG,
        ENEMY_SPAWN_DEFAULT,
        ENEMY_SPAWN_SMALL,
        PLAYER_DIE,
        PLAYER_HURT_1,
        PLAYER_HURT_2,
        PLAYER_HURT_3,
        PLAYER_SHOOT_WEAPON_DEFAULT,
        PLAYER_SHOOT_WEAPON_RIFLE,
        PLAYER_SHOOT_WEAPON_SHOTGUN,
        REWARD_PANEL,
        TUTORIAL_DONE
    }

    public enum WeaponType {
        DEFAULT,
        SHOTGUN,
        RIFLE
    }
}
