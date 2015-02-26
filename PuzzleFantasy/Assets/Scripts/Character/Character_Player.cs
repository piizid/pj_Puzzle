using UnityEngine;
using System.Collections;

public class Character_Player : Character
{

    [SerializeField]
    CharacterState_Player _State;

    Character_Monster _target;


    public bool InitializeCharacter(CharacterInfo info, CharacterState_Player state)
    {
        _State = state;
        return base.InitializeCharacter(info, state);
    }

    public void SetTarget(Character_Monster monster)
    {
        _target = monster;
    }

    protected override void ModelEvent(MODELMOTION motion, MOTIONEVENT motionEvent)
    {

    }

    void attack()
    {
    }

    void hit(int attackPoint)
    {
    }
}
