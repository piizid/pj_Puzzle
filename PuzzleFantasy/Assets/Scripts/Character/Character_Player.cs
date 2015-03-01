using UnityEngine;
using System.Collections;

public class Character_Player : Character
{
    [SerializeField]
    PlayerHUD _hud;

    [SerializeField]
    CoinUI _CoinUI;

    [SerializeField]
    CharacterState_Player _state;

    public CharacterState_Player _State
    {
        get { return _state; }
    }
    

    Character_Monster _target;

    [SerializeField]
    int _PuzzlePoint = 0;

    [SerializeField]
    ELEMENTTYPE _AttackElement;

    [SerializeField]
    bool _MagicAttack;

    public bool InitializeCharacter(CharacterInfo info, CharacterState_Player state)
    {
        _state = state;

        if (_hud != null)
            _hud.SetState(state);

        if (_CoinUI != null)
            _CoinUI.SetState(state);

        return base.InitializeCharacter(info, state);
    }

    public void SetTarget(Character_Monster monster)
    {
        _target = monster;
    }

    public void Action(NODETYPE type, int point)
    {
        _PuzzlePoint = point;
        switch (type)
        {
            case NODETYPE.NORMALATTACK:
                _Model.Start_Motion(MODELMOTION.NORMALATTACK);
                _AttackElement = ELEMENTTYPE.NONE;
                _MagicAttack = false;
                break;
            case NODETYPE.FIREATTACK:
                _AttackElement = ELEMENTTYPE.FIRE;
                _Model.Start_Motion(MODELMOTION.MAGICATTACK);
                _MagicAttack = true;
                break;
            case NODETYPE.SNOWATTACK:
                _AttackElement = ELEMENTTYPE.ICE;
                _Model.Start_Motion(MODELMOTION.MAGICATTACK);
                _MagicAttack = true;
                break;
            case NODETYPE.FIRESNOWATTACK:
                _AttackElement = ELEMENTTYPE.NONE;
                _Model.Start_Motion(MODELMOTION.MAGICATTACK);
                break;
            case NODETYPE.HEAL:
                _Model.Start_Motion(MODELMOTION.HEAL);
                break;
            case NODETYPE.COIN:
                _Model.Start_Motion(MODELMOTION.COIN);
                break;
        }
    }

    protected override void ModelEvent(MODELMOTION motion, MOTIONEVENT motionEvent)
    {
        switch (motion)
        {
            case MODELMOTION.MAGICATTACK:
            case MODELMOTION.NORMALATTACK:
                {
                    attackEvent(motionEvent);
                }
                break;
            case MODELMOTION.HEAL:
                healEvent(motionEvent);
                break;
            case MODELMOTION.COIN:
                coinEvent(motionEvent);
                break;
        }
    }

    void attackEvent(MOTIONEVENT motionEvent)
    {
        switch (motionEvent)
        {
            case MOTIONEVENT.EVENT:
                attack();
                break;
        }
    }

    void healEvent(MOTIONEVENT motionEvent)
    {
        switch (motionEvent)
        {
            case MOTIONEVENT.EVENT:
                heal();
                break;
        }
    }

    void coinEvent(MOTIONEVENT motionEvent)
    {
        switch (motionEvent)
        {
            case MOTIONEVENT.EVENT:
                coin();
                break;
        }
    }

    void heal()
    {
        int healPoint;
        Character_Player.GetHealPoint(_State, _PuzzlePoint, out healPoint);
        _State._CurHP += healPoint;
        _State._CurHP = Mathf.Clamp(_State._CurHP, 0, _State._MaxHP);

        _hud.CreateFont(healPoint, Color.green);
    }

    void coin()
    {
        int coinPoint;
        Character_Player.GetCoinPoint( _State, _PuzzlePoint, out coinPoint );

        _hud.CreateFont(coinPoint, Color.blue);
        AddCoin(coinPoint);
    }

    public void AddCoin(int coin)
    {
        _state._Coin += coin;
        _state._Coin = Mathf.Max(0, _state._Coin);
    }

    void attack()
    {
        int attackPoint;
        int criticalPoint;
        Character_Player.GetAttackPoint(_State, _target._State, _PuzzlePoint, _MagicAttack, _AttackElement, out attackPoint, out criticalPoint);

        bool critical = false;
        if (Random.Range(0, 101) <= criticalPoint)
        {
            attackPoint *= 2;
            critical = true;
        }

        _target.hit(attackPoint, _AttackElement, critical);
    }



    public void hit(int attackPoint)
    {
        _hud.CreateFont(attackPoint, Color.yellow);

        base.hit(attackPoint);
    }


    static public bool GetAttackPoint(CharacterState_Player player,
                                      CharacterState_Monster monster,
                                      int puzzlePoint, bool magicAttack, ELEMENTTYPE attackType,
                                      out int outAttackPoint, out int outCritcalPoint)
    {
        outAttackPoint = 0;
        outCritcalPoint = 0;

        if (player == null || monster == null)
            return false;

        float attackPoint = puzzlePoint;
        if (magicAttack)
        {
            attackPoint *= player._MagicAttack;
            attackPoint *= ElementRate(monster._ElementType, attackType);
        }
        else
        {
            attackPoint *= player._MeleeAttack;
        }
        outAttackPoint = (int)(attackPoint + 0.5f);
        outCritcalPoint = puzzlePoint * (player._Luck * 2);
        return true;
    }

    static public float ElementRate(ELEMENTTYPE dest, ELEMENTTYPE sour)
    {
        if( dest == ELEMENTTYPE.NONE || sour == ELEMENTTYPE.NONE )
            return 1.0f;

        if ( dest != sour )
            return 2.0f;

        return 0.5f;
    }

    static public bool GetHealPoint(CharacterState_Player player, int puzzlePoint,
                                     out int outHealPoint)
    {
        outHealPoint = 0;
        if (player == null)
            return false;

        outHealPoint = (int)((player._MaxHP * 0.1f) * puzzlePoint + 0.5f);

        return true;
    }

    static public bool GetCoinPoint(CharacterState_Player player, int puzzlePoint,
                                    out int outCointPoint)
    {
        outCointPoint = 0;
        if (player == null)
            return false;

        outCointPoint = puzzlePoint * 10;

        return true;
    }
   
}
