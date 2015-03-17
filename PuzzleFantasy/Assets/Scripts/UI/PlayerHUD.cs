using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHUD : HUD {

    [SerializeField]
    Text _MPText;

    CharacterState_Player _state;

    int _preMp;

    public void SetState(CharacterState_Player state)
    {
        _state = state;

        _preMp = _state._CurMP;
        setMP();
        base.SetLife(state);
    }

    new public void Update()
    {
        base.Update();

        if (_preMp != _state._CurMP)
        {
            setMP();
            _preMp = _state._CurMP;
        }
    }

    void setMP()
    {
        _MPText.text = string.Format("{0} / {1}", _state._CurMP, _state._MaxMP);
    }
        
}
