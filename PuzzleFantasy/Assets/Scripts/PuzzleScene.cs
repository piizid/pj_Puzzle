using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum NODETYPE
{
    NORMALATTACK,
    MAGICATTACK,
    SKILL_0,
    SKILL_1,
    SKILL_2
}

[System.Serializable]
public class NodeInfo
{
    public string _Name;
    [HideInInspector]
    public int _type;

    public GameObject _prefab;


    public NODETYPE _NodeType;
}

public class PuzzleScene : MonoBehaviour {

    [SerializeField]
    BattleManager _battleManager;

    public string _testStage;
    public CharacterState _testState;
    public CharacterInfo _testInfo;

    static PuzzleScene _instance = null;
    public static PuzzleScene _Instance { get { return _instance; } }

    List<PuzzleNode> _puzzleNodeList = new List<PuzzleNode>();

    [SerializeField]
    NodeInfo[] _NodeTypeList;

    public GameObject _LinePrefab;
	public GameObject _CirclePrefab;

    bool _puzzlePlaying = false;

    void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);
        else
            _instance = this;

        for (int i = 0; i < _NodeTypeList.Length; i++)
            _NodeTypeList[i]._type = i;

        _battleManager._phaseChangeEvent = this.battlePhaseChange;
    }

    void Start()
    {
        _battleManager.Initialize(_testStage, _testState, _testInfo);
    }

    public NodeInfo GetRandomNode()
    {
        int type = Random.Range(0, _NodeTypeList.Length);
        return _NodeTypeList[type];
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
    }

    public void PuzzleEnd()
    {
        if (_puzzlePlaying == false)
            return;

        foreach (var node in _puzzleNodeList)
        {
            node.NodeEnd();
        }
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


  
}
