using UnityEngine;
using System.Collections;


public enum BATTLEPHASE
{
    BATTLESTART,
    MOVE,
    BATTLE,
    BATTLEEND,
}



public class BattleManager : MonoBehaviour
{
    public delegate void PhaseChange(BATTLEPHASE pase);
    public PhaseChange _phaseChangeEvent;

    const float _moveTime = 1.0f;

    float _moveStartTime;
    int _currentStage;
    int _nextMonsterLevel;

    [SerializeField]
    Character_Player _PlayerCharacter;

    [SerializeField]
    Character_Monster _MonsterCharacter;

    [SerializeField]
    GameObject _backGroundParent;

    BackGround _backGround;

    BATTLEPHASE _Phase = BATTLEPHASE.BATTLESTART;

    StageInfo _stageInfo;

    public bool Initialize(string stageName, CharacterInfo playerInfo, CharacterState_Player playerState)
    {
        StageInfo info = StageTable._Instance.GetStageInfo(stageName);
        if (info == null)
            return false;

        _stageInfo = info;
        CharacterCreater._Instance.SetStageCharacterInfo(info._spriteInfos);

        _PlayerCharacter.InitializeCharacter(playerInfo, playerState);
        _MonsterCharacter.gameObject.SetActive(false);

        GameObject backGround = Instantiate(_stageInfo._BackGround) as GameObject;
        backGround.transform.SetParent(_backGroundParent.transform);
        RectTransform backGroundTF = backGround.GetComponent<RectTransform>();
        backGroundTF.position = Vector3.zero;
        backGroundTF.anchoredPosition3D = Vector3.zero;
        backGroundTF.rotation = Quaternion.identity;
        backGroundTF.localScale = Vector3.one;

        _backGround = backGround.GetComponent<BackGround>();
        _backGround.MoveStop();

        _PlayerCharacter.SetTarget(_MonsterCharacter);
        _MonsterCharacter.SetTarget(_PlayerCharacter);

        _PlayerCharacter._DeadEvent = this.playerDead;
        _MonsterCharacter._DeadEvent = this.monsterDead;

        _nextMonsterLevel = _stageInfo._StartLevel;

        return true;
    }

    public void StageStart()
    {
        set_Phase(BATTLEPHASE.MOVE);

        _backGround.MoveStart();

        _PlayerCharacter.gameObject.SetActive(true);
        _moveStartTime = Time.time;
        _currentStage = 1;
    }

    void Update()
    {
        switch (_Phase)
        {
            case BATTLEPHASE.MOVE:
                update_Move();
                break;
            case BATTLEPHASE.BATTLE:
                update_Battle();
                break;
        }
    }

    void set_Phase( BATTLEPHASE newPhase )
    {
        if (_Phase == newPhase)
            return;

        if (_phaseChangeEvent != null)
            _phaseChangeEvent(newPhase);
        _Phase = newPhase;
        if (newPhase == BATTLEPHASE.MOVE)
            _moveStartTime = Time.time;
    }

    void update_Move()
    {
        if (Time.time >= _moveStartTime + _moveTime)
        {
            _backGround.MoveStop();
            createNewMonster();

            set_Phase(BATTLEPHASE.BATTLE);
        }
    }

    void update_Battle()
    {

    }

    void createNewMonster()
    {
        bool boss = (_currentStage % _stageInfo._BossLevel == 0);

        CharacterState_Monster monster = CharacterCreater._Instance.GetNewState(_nextMonsterLevel, boss);
        CharacterInfo info = CharacterCreater._Instance.GetNewInfo(monster._ElementType, boss);

        _MonsterCharacter.InitializeCharacter(info, monster);
        _MonsterCharacter.gameObject.SetActive(true);

        _nextMonsterLevel = Random.Range(_stageInfo._MinMonsterLevelUp, _stageInfo._MaxMonsterLevelUp + 1);
    }

    void playerDead()
    {

    }

    void StageClear()
    {

    }

    void monsterDead()
    {
        if (_currentStage >= _stageInfo._StageCount)
            StageClear();
        else
        {
            _MonsterCharacter.gameObject.SetActive(false);
            _currentStage++;
            set_Phase(BATTLEPHASE.MOVE);
            _backGround.MoveStart();
        }
    }

    public void PlayerAction(NODETYPE type, int count)
    {
        _PlayerCharacter.Action(type, count);
    }
}
