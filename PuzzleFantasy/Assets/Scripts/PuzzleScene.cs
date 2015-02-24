using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class NodeInfo
{
    public string _Name;
    [HideInInspector]
    public int _type;

    public GameObject _prefab;
}

public class PuzzleScene : MonoBehaviour {
    static PuzzleScene _instance = null;
    public static PuzzleScene _Instance { get { return _instance; } }

    List<PuzzleNode> _puzzleNodeList = new List<PuzzleNode>();

    [SerializeField]
    NodeInfo[] _NodeTypeList;

    public GameObject _LinePrefab;
	public GameObject _CirclePrefab;

    void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);
        else
            _instance = this;

        for (int i = 0; i < _NodeTypeList.Length; i++)
            _NodeTypeList[i]._type = i;
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
        foreach (var node in _puzzleNodeList)
        {
            node.NodeStart();
        }
    }

    public void PuzzleEnd()
    {
        foreach (var node in _puzzleNodeList)
        {
            node.NodeEnd();
        }
    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            PuzzleStart();
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            PuzzleEnd();
        }

    }

}
