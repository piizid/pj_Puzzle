using UnityEngine;
using System.Collections;

[System.Serializable]
public struct StageCharacterInfo
{
    public float _minLevelrate;
    public float _maxLevelrate;

    public CharacterInfo[] _NormalCharacterInfoList;
    public CharacterInfo[] _MagicCharacterInfoList;
}

[System.Serializable]
public class StageInfo
{
    public string _StateName;
    public int _StageCount;

    public int _LevelInterval = 1;
    public int _StartLevel = 1;
    public int _MonsterLevelUp = 1;

    public float _GoldBonus = 1.0f;
    public float _ExpBonus = 1.0f;

    public GameObject _BackGround;

    public StageCharacterInfo[] _spriteInfos;
}

public class StageTable : MonoBehaviour  {
    static StageTable _instance;
    public static StageTable _Instance
    {
        get { return _instance;  }
    }

    public StageInfo[] _StageInfoList;

    void Awake()
    {
        _instance = this;
    }


    public StageInfo GetStageInfo(string stageName)
    {
        foreach (var info in _StageInfoList)
        {
            if (info._StateName == stageName)
                return info;
        }
        return null;
    }
}
