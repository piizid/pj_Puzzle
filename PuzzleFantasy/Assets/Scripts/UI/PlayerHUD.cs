using UnityEngine;
using System.Collections;

public class PlayerHUD : HUD {

    CharacterState_Player _state;

    public void SetState(CharacterState_Player state)
    {
        _state = state;

        base.SetLife(state);
    }
}
