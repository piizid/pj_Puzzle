using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleNode : MonoBehaviour
{
    static int _StateDead = Animator.StringToHash("Base Layer.Dead");
    static int _StateBorn = Animator.StringToHash("Base Layer.Born");

    [SerializeField]
    Animator _CircleAnimator;

    [SerializeField]
    float _LearNodeDist = 0.5f;
   
    [SerializeField]
    List<PuzzleNode> _LearPuzzleNodeList = new List<PuzzleNode>();

    static Stack<PuzzleNode> _SelectNodeStack = new Stack<PuzzleNode>();

    NodeInfo _nodeInfo;

    GameObject _IconObj;
    Animator _IconAnimator;

    int _preAnimatorState;

    bool _isSelect = false;
    bool _isLive = false;
    IEnumerator Start()
    {
        yield return null;
        _preAnimatorState = _CircleAnimator.GetCurrentAnimatorStateInfo(0).nameHash;
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
                if (hit.collider == collider2D)
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

        if ( _SelectNodeStack.Count <= 0)
            select();
        else if ( _isSelect )
        {
            PuzzleNode topNode = _SelectNodeStack.Pop();
            if (_SelectNodeStack.Peek() == this)
                topNode.releaseNode();
            else
                _SelectNodeStack.Push(topNode);
        }
        else if( _SelectNodeStack.Peek().IsNearNode(this) &&
                 _nodeInfo._type == _SelectNodeStack.Peek()._nodeInfo._type )
        {
            select();
        }      
    }

    void select()
    {
        _CircleAnimator.SetBool("Select", true);
        _SelectNodeStack.Push(this);
        _isSelect = true;
        _IconAnimator.SetBool("Select", true);
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
    }

    void Update()
    {
        int currentState = _CircleAnimator.GetCurrentAnimatorStateInfo(0).nameHash;

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
        _CircleAnimator.SetTrigger("Born");
        _CircleAnimator.SetBool("Loop", true );
    }

    public void NodeEnd()
    {
        _CircleAnimator.SetTrigger("Dead");
        _CircleAnimator.SetBool("Loop", false);
    }

    public void Hit()
    {
        _CircleAnimator.SetTrigger("Dead");
    }
}
