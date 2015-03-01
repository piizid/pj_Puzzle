using UnityEngine;
using System.Collections;

public enum ELEMENTTYPE
{
    NONE,
    FIRE,
    ICE,
}

public enum AITYPE
{
    REALTIME,
    TURN
}

[System.Serializable]
public class CharacterState_Life
{
    public int _MaxHP;
    public int _CurHP;
}

[System.Serializable]
public class CharacterState_Player : CharacterState_Life
{
    public int _MeleeAttack;
    public int _MagicAttack;

    public int _HealPoint;
    public int _CoinPoint;

    public int _MaxMP;
    public int _CurMP;

    public int _Luck;

    public int _Coin;
}

[System.Serializable]
public class CharacterState_Monster : CharacterState_Life
{
    public int _AttackPoint;

    public AITYPE _AItype;
    public ELEMENTTYPE _ElementType = ELEMENTTYPE.NONE;

    public int _AttackTurn;
    public float _AttackTime;

    public int _RewardClass;

    [HideInInspector]
    public float _NextAttackTime;

    [HideInInspector]
    public int _AttackTurnCount;
}
