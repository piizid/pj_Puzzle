using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LineManager : MonoBehaviour {
	static LineManager _instance;
	public static LineManager _Instance
	{
		get 
		{
			if( _instance == null )
			{
				_instance = GameObject.FindObjectOfType< LineManager >();
				if( _instance == null )
					Debug.LogError("Scene not have lineManager");
			}
			return _instance;
		}
	}

	Queue< GameObject > _circleQueue = new Queue<GameObject>();
    Queue<GameObject> _lineQueue = new Queue<GameObject>();

	Stack< GameObject > _acitiveCircleStack = new Stack<GameObject>();
    Stack<GameObject> _acitiveLineStack = new Stack<GameObject>();


	void Awake()
	{
		_instance = this;
	}

    void addCircle(Vector3 position)
    {
        GameObject newCircle = null;
        if (_circleQueue.Count > 0)
        {
            newCircle = _circleQueue.Dequeue();
            newCircle.SetActive(true);
        }
        else
            newCircle = Instantiate(PuzzleScene._Instance._CirclePrefab, transform.position, transform.rotation) as GameObject;

        newCircle.transform.SetParent(transform);
        newCircle.transform.position = Vector3.zero;
        newCircle.transform.rotation = Quaternion.identity;
        newCircle.transform.localScale = Vector3.one;

        newCircle.transform.position = position;

        _acitiveCircleStack.Push(newCircle);
    }

	public void AddLine( Vector3 position )
	{
        Vector3 prePosition = Vector3.zero;
        bool drawLine = false;
        if( _acitiveCircleStack.Count > 0 )
        {
            prePosition = _acitiveCircleStack.Peek().transform.position;
            drawLine = true;
        }
        addCircle(position);

        if (!drawLine)
            return;

        addLine(prePosition, position);
	}

    void addLine(Vector3 pointOne, Vector3 pointTwo)
    {
        GameObject line = null;
        if (_lineQueue.Count > 0)
        {
            line = _lineQueue.Dequeue();
            line.SetActive(true);
        }
        else
            line = Instantiate(PuzzleScene._Instance._LinePrefab) as GameObject;

        line.transform.SetParent(transform);
        line.transform.localScale = Vector3.one;
        line.transform.position = pointOne;

        float angle = Mathf.Rad2Deg * Mathf.Acos( Vector3.Dot( Vector3.right, (pointTwo - pointOne).normalized));
        if (pointTwo.y < pointOne.y)
            angle = -angle;

        Quaternion rot = Quaternion.Euler(0.0f, 0.0f, angle);
        line.transform.localRotation = rot;

        _acitiveLineStack.Push(line);
    }


	public void RemoveLine()
	{
        if (_acitiveCircleStack.Count > 0)
        {
            GameObject circle = _acitiveCircleStack.Pop();
            circle.SetActive(false);
            _circleQueue.Enqueue(circle);
        }
        if (_acitiveLineStack.Count > 0)
        {
            GameObject line = _acitiveLineStack.Pop();
            line.SetActive(false);
            _lineQueue.Enqueue(line);
        }
	}

}
