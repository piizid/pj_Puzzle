using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class StateInfo
{
    public float _MinAttackPerLevel = 1.0f;
    public float _MaxAttackPerLevel = 1.0f;
    
    public float _MinHPPerLevel = 5.0f;
    public float _MaxHPPerLevel = 10.0f;

    public float _MinRewardClassPerLevel = 0.3f;
    public float _MaxRewardClassPerLevel = 0.5f;

    public float _HaveElementRate = 0.5f;
    public float _FireElementRate = 0.5f;

    public float _MinTime;
    public float _MaxTime;

    public int _MinTurn;
    public int _MaxTurn;
}

public class CharacterCreater :MonoBehaviour {
    StageCharacterInfo _characterInfo;

    [SerializeField]
    StateInfo _NormalCharacterState;

    [SerializeField]
    StateInfo _BossCharacterState;

    static CharacterCreater _instance = null;
    public static  CharacterCreater _Instance
    {
        get
        {
            return _instance;
        }
    }

    public void SetStageCharacterInfo(StageCharacterInfo info)
    {
        _characterInfo = info;
    }

    void Awake()
    {
        _instance = this;
    }

    public CharacterState_Monster GetNewState(int level, bool boss )
    {
        CharacterState_Monster newState = new CharacterState_Monster();

        StateInfo createInfo = ( boss ) ? _BossCharacterState : _NormalCharacterState;

        newState._AItype = (boss) ? AITYPE.REALTIME : AITYPE.TURN;
        newState._AttackTime = Random.Range( createInfo._MinTime, createInfo._MaxTime );
        newState._AttackTurn = Random.Range(createInfo._MinTurn, createInfo._MaxTurn );

        newState._AttackPoint = (int)(Random.Range(createInfo._MinAttackPerLevel, createInfo._MaxAttackPerLevel) * level);
        newState._MaxHP = (int)(Random.Range(createInfo._MinHPPerLevel, createInfo._MaxHPPerLevel) * level);
        newState._CurHP = newState._MaxHP;
        newState._RewardClass = (int)(Random.Range(createInfo._MinRewardClassPerLevel, createInfo._MaxRewardClassPerLevel) * level);

        if (Random.Range(0.0f, 1.0f) < createInfo._HaveElementRate)
        {
            if (Random.Range(0.0f, 1.0f) < createInfo._FireElementRate)
                newState._ElementType = ELEMENTTYPE.FIRE;
            else
                newState._ElementType = ELEMENTTYPE.ICE;
        }
        else
            newState._ElementType = ELEMENTTYPE.NONE;

        return newState;
    }

    public CharacterInfo GetNewInfo( ELEMENTTYPE type, bool boss )
    {
        if (_characterInfo == null)
            return null;

        CharacterInfo[] info = null;

        switch (type)
        {
            case ELEMENTTYPE.NONE:
                if (boss) info = _characterInfo._None_Element_Boss_CharacterInfoList;
                else info = _characterInfo._None_Element_CharacterInfoList;
                break;
            case ELEMENTTYPE.FIRE:
                if (boss) info = _characterInfo._Fire_Element_Boss_CharacterInfoList;
                else info = _characterInfo._Fire_Element_CharacterInfoList;
                break;
            case ELEMENTTYPE.ICE:
                if (boss) info = _characterInfo._Ice_Element_Boss_CharacterInfoList;
                else info = _characterInfo._Ice_Element_CharacterInfoList;
                break;
        }

        if (info == null)
            return null;
        if (info.Length <= 0)
            return null;

        int index = Random.Range(0, info.Length);

        return info[index];
    }


}
