using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleNode : MonoBehaviour
{
    static int _StateDead = Animator.StringToHash("Base Layer.Dead");
    static int _StateBorn = Animator.StringToHash("Base Layer.Born");

    static NODETYPE _selectType;
    static int _fireNodeCount;
    static int _iceNodeCount;
    static Stack<PuzzleNode> _SelectNodeStack = new Stack<PuzzleNode>();

    public static bool GetHitNodes( out NODETYPE type, out int count )
    {
        type = _selectType;
        count = _SelectNodeStack.Count;

        if (count > 0)
            return true;
        return false;
    }


    [SerializeField]
    Animator _CircleAnimator;

    [SerializeField]
    float _LearNodeDist = 0.5f;
   
    [SerializeField]
    List<PuzzleNode> _LearPuzzleNodeList = new List<PuzzleNode>();

    NodeInfo _nodeInfo;

    GameObject _IconObj;
    Animator _IconAnimator;

    int _preAnimatorState;

    bool _isSelect = false;
    bool _isLive = false;


    IEnumerator Start()
    {
        yield return null;
        _preAnimatorState = _CircleAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash;
        findNearNode();

        PuzzleScene._Instance.AddPuzzleNode(this);
    }

    void findNearNode()
    {
        Vector3 dir = Vector3.up;
        Quaternion quat = Quaternion.Euler(0.0f, 0.0f, 60.0f);
        RaycastHit2D[] hits;
        for (int i = 0; i < 6; i++)
        {
            hits = Physics2D.RaycastAll(transform.position, dir, _LearNodeDist);
            Debug.DrawLine(transform.position, transform.position + (dir * _LearNodeDist), Color.white, 4.0f);
            dir = quat * dir;
            if (hits == null)
                continue;

            foreach (var hit in hits)
            {
                if (hit.collider == GetComponent<Collider2D>())
                    continue;

                PuzzleNode node = hit.collider.gameObject.GetComponent<PuzzleNode>();
                if (node != null)
                    _LearPuzzleNodeList.Add(node);
            }
        }
    }

    bool IsNearNode(PuzzleNode node)
    {
        return (_LearPuzzleNodeList.Find(curnode => node == curnode) != null);
    }

    public void Select()
    {
        if (_isLive == false)
            return;

        if (_SelectNodeStack.Count <= 0)
        {
            _selectType = this._nodeInfo._NodeType;
            _fireNodeCount = 0;
            _iceNodeCount = 0;
			select ();
		}
        else if ( _isSelect )
        {
			if( _SelectNodeStack.Count == 1 && _SelectNodeStack.Peek() == this )
				return;

            PuzzleNode topNode = _SelectNodeStack.Pop();
            if (_SelectNodeStack.Peek() == this)
            {
                if (topNode._nodeInfo._NodeType == NODETYPE.FIREATTACK)
                    _fireNodeCount--;
                else if (topNode._nodeInfo._NodeType == NODETYPE.SNOWATTACK)
                    _iceNodeCount--;

                topNode.releaseNode();
            }
            else
                _SelectNodeStack.Push(topNode);
        }
        else if( _SelectNodeStack.Peek().IsNearNode(this) )
        {
            if( _nodeInfo._NodeType == _SelectNodeStack.Peek()._nodeInfo._NodeType  )
               select();
            else if (_selectType == NODETYPE.FIREATTACK ||
                     _selectType == NODETYPE.SNOWATTACK ||
                     _selectType == NODETYPE.FIRESNOWATTACK)
            {
                if (_nodeInfo._NodeType == NODETYPE.FIREATTACK ||
                    _nodeInfo._NodeType == NODETYPE.SNOWATTACK)
                {
                    select();
                }
            }
        }
        checkMagicElement();
    }

    static void checkMagicElement()
    {
        if (_selectType == NODETYPE.FIREATTACK || _selectType == NODETYPE.SNOWATTACK || _selectType == NODETYPE.FIRESNOWATTACK)
        {
            if (_fireNodeCount > 0 && _iceNodeCount > 0)
                _selectType = NODETYPE.FIRESNOWATTACK;
            else if (_fireNodeCount > 0)
                _selectType = NODETYPE.FIREATTACK;
            else
                _selectType = NODETYPE.SNOWATTACK;
        }
    }

    void select()
    {
        if (_nodeInfo._NodeType == NODETYPE.FIREATTACK)
            _fireNodeCount++;
        else if (_nodeInfo._NodeType == NODETYPE.SNOWATTACK)
            _iceNodeCount++;

        if (_fireNodeCount > 0 && _iceNodeCount > 0)
            _selectType = NODETYPE.FIRESNOWATTACK;

        _CircleAnimator.SetBool("Select", true);
        _SelectNodeStack.Push(this);
        _isSelect = true;
        _IconAnimator.SetBool("Select", true);

		//RectTransform rect = gameObject.GetComponent< RectTransform > ();
		LineManager._Instance.AddLine (transform.position);
    }

    public void ReleaseSelect()
    {
        if (_SelectNodeStack.Count < 3)
        {
            while (_SelectNodeStack.Count > 0)
            {
                _SelectNodeStack.Pop().releaseNode();
            }
        }
        else
        {
            Queue<PuzzleNode> queue = new Queue<PuzzleNode>();
            while (_SelectNodeStack.Count > 0)
            {
                PuzzleNode node = _SelectNodeStack.Pop();
                node._isLive = false;
                queue.Enqueue(node);
            }
            
            PuzzleScene._Instance.HitNodes( _selectType , queue.Count );
            StartCoroutine(hit(queue));
        }
    }

    IEnumerator hit(Queue<PuzzleNode> queue)
    {
        while (queue.Count > 0)
        {
            queue.Dequeue().Hit();

            yield return new WaitForSeconds(0.01f);            
        }
    }

    void releaseNode()
    {
        _CircleAnimator.SetBool("Select", false);
        _IconAnimator.SetBool("Select", false );
        _isSelect = false;
		LineManager._Instance.RemoveLine();
    }

    void Update()
    {
        int currentState = _CircleAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash;

        if (_preAnimatorState == _StateDead && currentState == _StateBorn)
        {
            IconInit();
            _isLive = true;
        }
        else if (currentState == _StateDead)
        {
            _isLive = false;
        }
        _preAnimatorState = currentState;
    }

    void IconInit()
    {
        if (_IconObj)
            Destroy(_IconObj);

        _nodeInfo = PuzzleScene._Instance.GetRandomNode();

        GameObject obj = Instantiate(_nodeInfo._prefab) as GameObject;
        _IconAnimator = obj.GetComponent<Animator>();

        obj.transform.SetParent(_CircleAnimator.gameObject.transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;

        _CircleAnimator.SetBool("Select", false);
        _IconObj = obj;
        _isSelect = false;
    }

    public void NodeStart()
    {
        _CircleAnimator.ResetTrigger("Dead");
        _CircleAnimator.SetTrigger("Born");
        _CircleAnimator.SetBool("Loop", true );
    }

    public void NodeEnd()
    {
        _CircleAnimator.ResetTrigger("Born");
        _CircleAnimator.SetTrigger("Dead");
        _CircleAnimator.SetBool("Loop", false);
    }

    public void Hit()
    {
        _CircleAnimator.SetTrigger("Dead");
		LineManager._Instance.RemoveLine();
    }
}
