using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    [SerializeField]
    Text _HP;

    [SerializeField]
    Transform _DamageFontPos;

    [SerializeField]
    GameObject _damageFontPrefab;

    int _preHP;

    CharacterState_Life _life = null;
   

    public void SetLife(CharacterState_Life life)
    {
        _life = life;
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
        if (_life == null)
        {
            _HP.text = "";
        }
        else
        {
            _HP.text = string.Format("{0} / {1}", _life._CurHP, _life._MaxHP);
            _preHP = _life._CurHP;
        }
    }

    public void CreateFont(int point, Color color)
    {
        GameObject newObj = Instantiate(_damageFontPrefab) as GameObject;
        newObj.transform.SetParent(_DamageFontPos);

        RectTransform rectTF = newObj.GetComponent<RectTransform>();

        rectTF.anchoredPosition3D = Vector3.zero;
        rectTF.rotation = Quaternion.identity;
        rectTF.localScale = Vector3.one;

        newObj.GetComponent<DamageFont>().SetText(point, color);
    }
}
