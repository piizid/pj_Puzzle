using UnityEngine;
using System.Collections;

public class MonsterHUD : HUD {

    CharacterState_Monster _state;

    public void SetState(CharacterState_Monster state)
    {
        _state = state;

        base.SetLife(state);
    }

	
}
