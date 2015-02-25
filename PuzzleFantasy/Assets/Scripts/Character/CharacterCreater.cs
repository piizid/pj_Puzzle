using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MONSTERTYPE
{
    NORMAL,
    MAGIC
}

[System.Serializable]
public class StateInfo
{
    public float _NormalAttackPerLevel = 1.0f;
    public float _MagicAttackPerLevel = 1.0f;
    public float _NormalDefencePerLevel = 1.0f;
    public float _MagicDefencePerLevel = 1.0f;
    public float _HPPerLevel = 5.0f;
}

public class CharacterCreater :MonoBehaviour {
    StageCharacterInfo[] _characterInfo;

    [SerializeField]
    StateInfo _NormalCharacterState;

    [SerializeField]
    StateInfo _MagicCharacterState;

    static CharacterCreater _instance = null;
    public static  CharacterCreater _Instance
    {
        get
        {
            return _instance;
        }
    }

    public void SetStageCharacterInfo(StageCharacterInfo[] info)
    {
        _characterInfo = info;
    }

    void Awake()
    {
        _instance = this;
    }

    public CharacterState GetNewState(int level, MONSTERTYPE type)
    {
        CharacterState newState = new CharacterState();

        StateInfo stateInfo = (type == MONSTERTYPE.NORMAL) ?
            _NormalCharacterState :
            _MagicCharacterState;

        newState._AttackPoint = (int)(level * stateInfo._NormalAttackPerLevel);
        newState._MagicPoint = (int)(level * stateInfo._MagicAttackPerLevel);
        newState._AttackDefence = (int)(level * stateInfo._NormalDefencePerLevel);
        newState._MagicDefence = (int)(level * stateInfo._MagicDefencePerLevel);
        newState._MaxHP = (int)(level * stateInfo._HPPerLevel);
        newState._CurrentHP = newState._MaxHP;

        return newState;
    }

    public CharacterInfo GetNewInfo( float rate, MONSTERTYPE type )
    {
        if (_characterInfo == null)
            return null;

        List<StageCharacterInfo> list = new List<StageCharacterInfo>();

        foreach (var info in _characterInfo)
        {
            if (info._minLevelrate <= rate &&
                info._maxLevelrate >= rate)
            {
                list.Add(info);
            }
        }

        if( list.Count <= 0 )
            return null;

        int listIndex = Random.Range(0, list.Count);

        CharacterInfo[] characterInfo = (type == MONSTERTYPE.NORMAL) ?
            list[listIndex]._NormalCharacterInfoList :
            list[listIndex]._MagicCharacterInfoList;

        int index = Random.Range(0, characterInfo.Length);
        return characterInfo[index];
    }


}
