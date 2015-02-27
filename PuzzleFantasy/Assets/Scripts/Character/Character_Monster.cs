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

    float _attackTime;
    int _attackTurnCount;

    public bool InitializeCharacter(CharacterInfo info, CharacterState_Monster state)
    {
        _state = state;

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

    }


    public void AttackStart()
    {

    }

    void attack()
    {
    }

    public void ResetAttackTime()
    {

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
            if (Time.time >= _attackTime)
            {
                AttackStart();
            }
        }
    }
}
