using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TRAPTYPE
{
    NONE,
    DART,
    NET
}

public enum BASETILETYPE
{
    EMPTY,
    STONEFLOOR = 2,
    STONEWALL,
    STAIR_DOWN,
    STAIR_UP,
    OUTOFRANGE
}

public enum TILE_RESTRICTION
{
    FORBIDDEN,
    MOVEABLE,
    FLYONLY,
    OCCUPIED
}

public enum STAIRTYPE
{
    NONE,
    BASE_DOWN_STAIR,
    BASE_UP_STAIR
}

public enum ENEMYTYPE
{
    JELLY,
    RAT,
    ELEPHANTSLUG
}

public enum DEBUFFTYPE
{
    NONE,
    ENTANGLE
}

public enum PLAYERSTATE
{
    NONE,
    INPUT,
    OUTPUT,
    DELAY
}

public enum ENEMYSTATE
{
    IDLE,
    TRACKING,
    ATTACK,
    RUNNINGAWAY
}

public enum TURN_STATE
{
    NONE,
    PLAYER_TURN,
    INTERACTIVE,
    ENEMY_TURN,
}

