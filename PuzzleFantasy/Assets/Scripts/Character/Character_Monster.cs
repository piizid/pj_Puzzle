using UnityEngine;
using System.Collections;

public class Character_Monster : Character {

    [SerializeField]
    MonsterHUD _HUD;

    [SerializeField]
    CharacterState_Monster _state;

    public CharacterState_Monster _State
    {
        get { return _state; }
    }

    Character_Player _target;
    bool _attack = false;

    public bool InitializeCharacter(CharacterInfo info, CharacterState_Monster state)
    {
        _state = state;
        ResetAttackTime();

        if (_HUD != null)
            _HUD.SetState(state);

        return base.InitializeCharacter(info, state);
    }

    public void SetTarget(Character_Player player)
    {
        _target = player;
    }

    protected override void ModelEvent(MODELMOTION motion, MOTIONEVENT motionEvent)
    {
        switch (motion)
        {
            case MODELMOTION.NORMALATTACK:
            case MODELMOTION.MAGICATTACK:
                attackEvent(motionEvent);
                break;
        }
    }

    void attackEvent(MOTIONEVENT motionEvent)
    {
        switch (motionEvent)
        {
            case MOTIONEVENT.START:
                _attack = true;
                break;
            case MOTIONEVENT.EVENT:
                attack();
                break;
            case MOTIONEVENT.END:
                ResetAttackTime();
                _attack = false;
                break;
        }
    }

    public void AttackStart()
    {
        _Model.Start_Motion(MODELMOTION.NORMALATTACK);
 
    }

    void attack()
    {
        _target.hit(_state._AttackPoint);
    }

    public void ResetAttackTime()
    {
        _state._NextAttackTime = Time.time + _state._AttackTime;
        _state._AttackTurnCount = _state._AttackTurn;
    }

    public void hit(int attackPoint, ELEMENTTYPE type, bool critical)
    {
        _HUD.CreateFont(attackPoint, Color.yellow);

        base.hit(attackPoint);
    }

    void Update()
    {
        if( _State != null && _State._AItype == AITYPE.REALTIME)
        {
            if (Time.time >= _state._NextAttackTime && _attack == false)
            {
                AttackStart();
            }
        }
    }

    public void PlayerMotionEnd()
    {
        if (_state._AItype == AITYPE.TURN)
        {
            _state._AttackTurnCount--;
            if (_state._AttackTurnCount <= 0)
                AttackStart();
        }
    }
}
