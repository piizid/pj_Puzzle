using UnityEngine;
using System.Collections;

public class Character_Monster : Character {

    [SerializeField]
    CharacterState_Monster _state;

    Character_Player _target;

    public bool InitializeCharacter(CharacterInfo info, CharacterState_Monster state)
    {
        _state = state;
        return base.InitializeCharacter(info, state);
    }

    public void SetTarget(Character_Player player)
    {
        _target = player;
    }

    protected override void ModelEvent(MODELMOTION motion, MOTIONEVENT motionEvent)
    {
    }

    void attack()
    {
    }

    void hit(int attackPoint, ELEMENTTYPE type, bool critical)
    {
    }
}
