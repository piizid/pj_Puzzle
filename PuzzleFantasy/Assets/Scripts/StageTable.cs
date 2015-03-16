using UnityEngine;
using System.Collections;

[System.Serializable]
public class StageCharacterInfo
{
    public CharacterInfo[] _None_Element_CharacterInfoList;
    public CharacterInfo[] _Fire_Element_CharacterInfoList;
    public CharacterInfo[] _Ice_Element_CharacterInfoList;

    public CharacterInfo[] _None_Element_Boss_CharacterInfoList;
    public CharacterInfo[] _Fire_Element_Boss_CharacterInfoList;
    public CharacterInfo[] _Ice_Element_Boss_CharacterInfoList;
}

[System.Serializable]
public class StageInfo
{
    public string _StateName;
    public int _StageCount;

    public int _StartLevel = 1;
    public int _BossLevel = 5;

    public int _MinMonsterLevelUp = 1;
    public int _MaxMonsterLevelUp = 1;

    public GameObject _BackGround;

    public StageCharacterInfo _spriteInfos;
}

public class StageTable : Singleton< StageTable >  {
    public StageInfo[] _StageInfoList;

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
