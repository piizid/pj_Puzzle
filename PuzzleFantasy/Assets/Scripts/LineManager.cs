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

	Stack< GameObject > _acitiveCircleList = new Stack<GameObject>();

	void Awake()
	{
		_instance = this;
	}

	public void AddLine( Vector3 position )
	{
		GameObject newCircle = Instantiate (PuzzleScene._Instance._CirclePrefab, transform.position, transform.rotation) as GameObject;

		newCircle.transform.parent = this.transform;
		newCircle.transform.position = Vector3.zero;
		newCircle.transform.rotation = Quaternion.identity;
		newCircle.transform.localScale = Vector3.one;

		newCircle.transform.position = position;

	}

	public void RemoveLine()
	{
	}

}
