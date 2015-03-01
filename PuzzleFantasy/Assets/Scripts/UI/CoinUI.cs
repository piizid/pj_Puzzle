using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoinUI : MonoBehaviour {
    CharacterState_Player _state;

    [SerializeField]
    Text _text;

    int _preCoin;

    public void SetState(CharacterState_Player state)
    {
        _state = state;
        setCoin();
        _preCoin = _state._Coin;
    }

    void setCoin()
    {
        if (_state != null)
        {
            _text.text = string.Format("{0:D6}", _state._Coin);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(_state != null &&
            _preCoin != _state._Coin )
        {
            setCoin();
            _preCoin = _state._Coin;
        }
	}
}
