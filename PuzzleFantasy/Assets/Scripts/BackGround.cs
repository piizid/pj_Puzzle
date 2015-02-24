using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BackGround : MonoBehaviour {

    [System.Serializable]
    public class BackGroundNode
    {
        public GameObject _obj;
        public float _xMoveSpeed;
    }

    public BackGroundNode[] _BackGroundNode;

    [SerializeField]
    bool _Play = true;

    RawImage[] _BackGroundImage = null;

    void Start()
    {
        int count = _BackGroundNode.Length;
        _BackGroundImage = new RawImage[count];

        for( int i = 0 ;i < _BackGroundNode.Length ; i++ )
        {
            _BackGroundImage[i] = _BackGroundNode[i]._obj.GetComponent<RawImage>();
        }
    }

	void Update () {
        if (_Play)
            backGroundMove();	
    }

    void backGroundMove()
    {
        if (_BackGroundImage == null)
            return;
        
        for (int i = 0; i < _BackGroundImage.Length; i++)
        {
            if (_BackGroundImage[i] == null)
                return;

            float moveSpeed = _BackGroundNode[i]._xMoveSpeed * Time.deltaTime;
            Rect uvRect = _BackGroundImage[i].uvRect;
            uvRect.x += moveSpeed;
            _BackGroundImage[i].uvRect = uvRect;
        }

    }

    public void MoveStart()
    {
        _Play = true;
    }

    public void MoveStop()
    {
        _Play = false;
    }

}
