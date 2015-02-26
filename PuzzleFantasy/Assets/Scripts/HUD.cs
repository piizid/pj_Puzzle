using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    [SerializeField]
    Text _HP;

    [SerializeField]
    Text _AP;

    [SerializeField]
    Text _DP;

    [SerializeField]
    Transform _DamageFontPos;

    [SerializeField]
    GameObject _damageFontPrefab;

    int _preHP;

    CharacterState_Life _life = null;

    public void SetState(CharacterState_Life state)
    {
        _life = state;
        setText();
        gameObject.SetActive(true);
    }

    void OnDisable()
    {
        _DamageFontPos.DetachChildren();
    }

    void Update()
    {
        if (_life == null)
        {
            gameObject.SetActive(false);
            return;
        }

        if (_preHP != _life._CurHP)
        {
            _HP.text = string.Format("{0} / {1}", _life._CurHP, _life._MaxHP);
            _preHP = _life._CurHP;
        }
    }

    void setText()
    {
        //if (_state == null)
        //{
        //    _HP.text = "";
        //    _AP.text = "";
        //    _DP.text = "";
        //}
        //else
        //{
        //    _HP.text = string.Format("{0} / {1}", _state._CurrentHP, _state._MaxHP);
        //    _AP.text = string.Format("{0} / {1}", _state._AttackPoint, _state._MagicPoint);
        //    _DP.text = string.Format("{0} / {1}", _state._AttackDefence, _state._MagicDefence);
        //    _preHP = _state._CurrentHP;
        //}
    }

    public void CreateDamageFont(int damage)
    {
        GameObject newObj = Instantiate(_damageFontPrefab) as GameObject;
        newObj.transform.SetParent(_DamageFontPos);

        RectTransform rectTF = newObj.GetComponent<RectTransform>();

        rectTF.anchoredPosition3D = Vector3.zero;
        rectTF.rotation = Quaternion.identity;
        rectTF.localScale = Vector3.one;

        newObj.GetComponent<DamageFont>().SetText(damage);
    }
}
