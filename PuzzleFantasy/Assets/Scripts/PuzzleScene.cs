using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum NODETYPE
{
    NORMALATTACK,
    FIREATTACK,
    SNOWATTACK,
    HEAL,
    COIN,
    FIRESNOWATTACK,
}

[System.Serializable]
public class NodeInfo
{
    public string _Name;

    public GameObject _prefab;
    public int _Count = 1;

    public NODETYPE _NodeType;
}

[System.Serializable]
public class NodeIconSprite
{
    public NODETYPE _Type;
    public Sprite _Sprite;
}

public class PuzzleScene : Singleton< PuzzleScene > {

    [SerializeField]
    BattleManager _battleManager;

    public string _testStage;
    public CharacterState_Player _testPlayerState;
    public CharacterInfo _testInfo;

    List<PuzzleNode> _puzzleNodeList = new List<PuzzleNode>();

    [SerializeField]
    NodeInfo[] _NodeTypeList;

    public GameObject _LinePrefab;
	public GameObject _CirclePrefab;

    bool _puzzlePlaying = false;

    NodeInfo[] _randomNodeinfo = null;
    public NodeIconSprite[] _NodeIconSprite;
    void Start()
    {
        createRandomNodeInfo();
        _battleManager._phaseChangeEvent = this.battlePhaseChange;
        _battleManager.Initialize(_testStage, _testInfo, _testPlayerState);
    }

    void createRandomNodeInfo()
    {
        int count = 0;
        foreach (var info in _NodeTypeList)
            count += info._Count;

        _randomNodeinfo = new NodeInfo[count];

        int index = 0;
        foreach (var info in _NodeTypeList)
        {
            for (int i = 0; i < info._Count; i++, index++)
            {
                _randomNodeinfo[index] = info;
            }
        }
    }

    public NodeInfo GetRandomNode()
    {
        int type = Random.Range(0, _randomNodeinfo.Length);
        return _randomNodeinfo[type];
    }

    public void AddPuzzleNode( PuzzleNode node )
    {
        _puzzleNodeList.Add(node);
    }

    public void PuzzleStart()
    {
        if (_puzzlePlaying)
            return;

        foreach (var node in _puzzleNodeList)
        {
            node.NodeStart();
        }
        _puzzlePlaying = true;
    }

    public void PuzzleEnd()
    {
        if (_puzzlePlaying == false)
            return;

        foreach (var node in _puzzleNodeList)
        {
            node.NodeEnd();
        }
        _puzzlePlaying = false;
    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            _battleManager.StageStart();
            //PuzzleStart();
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            //PuzzleEnd();
        }
    }

    void battlePhaseChange(BATTLEPHASE newPhase)
    {
        switch (newPhase)
        {
            case BATTLEPHASE.BATTLE:
                PuzzleStart();
                break;
            default:
                PuzzleEnd();
                break;
        }
    }

    public void HitNodes(NODETYPE type, int count)
    {
        Debug.Log("Type : " + type.ToString() + ", Count : " + count.ToString());
        _battleManager.PlayerAction(type, count);
    }

    public void PuzzleReset()
    {
        StartCoroutine(puzzleReset());
    }

    IEnumerator puzzleReset()
    {
        PuzzleEnd();
        yield return new WaitForSeconds(0.1f);
        PuzzleStart();
    }

    public Sprite GetNodeIconSprite(NODETYPE type)
    {
        foreach (var info in _NodeIconSprite)
            if (info._Type == type) return info._Sprite;
        return null;
    }
      
}
