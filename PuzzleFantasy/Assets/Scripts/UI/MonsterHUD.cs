using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MonsterHUD : HUD {

    CharacterState_Monster _state;

    [SerializeField]
    Text _AttackTime;

    [SerializeField]
    Text _AttackPoint;

    public void SetState(CharacterState_Monster state)
    {
        _state = state;
        setAttackPoint();
        setTime();
        base.SetLife(state);
    }

    void setAttackPoint()
    {
        _AttackPoint.text = _state._AttackPoint.ToString();
    }

    void setTime()
    {
        string text;
        if (_state._AItype == AITYPE.REALTIME)
        {
            float leftTIme = _state._NextAttackTime - Time.time;
            leftTIme = Mathf.Max(0, leftTIme);
            text = string.Format("{0:F1}", leftTIme );
        }
        else
        {
            text = _state._AttackTurnCount.ToString();
        }
        _AttackTime.text = text;
    }

    new void Update()
    {
        base.Update();
        setTime();
    }
}
