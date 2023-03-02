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
        BUILDING_DROP,
        CANON_SHOOT,
        ENEMY_SHOOT,
        ENEMY_SPAWN_DEFAULT,
        ENEMY_SPAWN_SMALL,
        ENEMY_SPAWN_BIG,
        PLAYER_SHOOT_WEAPON_DEFAULT
    }

    public enum WeaponType {
        DEFAULT,
        RIFLE
    }
}
