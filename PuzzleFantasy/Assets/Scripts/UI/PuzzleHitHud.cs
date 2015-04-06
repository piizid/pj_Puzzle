using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PuzzleHitHud : Singleton_Scene< PuzzleHitHud > {

    [SerializeField]
    Text _text;

    [SerializeField]
    Image _Image;

    NODETYPE _preType;
    int _preCount = 0;

	// Use this for initialization
	void Start () {

        PuzzleNode.GetHitNodes(out _preType, out _preCount);
        Hide();
	}
	
	// Update is called once per frame
	void Update () {

        NODETYPE hitType;
        int hitCount;

        PuzzleNode.GetHitNodes(out hitType, out hitCount);
        if( _preType != hitType || _preCount != hitCount )
        {
            if( hitCount > 0 )
                Show(hitType, hitCount);
            else
                Hide();

            _preCount = hitCount;
            _preType = hitType;
        }
	}
    
    public void Hide()
    {
        _Image.gameObject.SetActive(false);
        _text.gameObject.SetActive(false);
    }

    public void Show(NODETYPE type, int count)
    {
        Sprite sprite = PuzzleScene._Instance.GetNodeIconSprite(type);
        _Image.sprite = sprite;
        _Image.gameObject.SetActive(true);

        _text.text = getPointString(type, count);
        _text.gameObject.SetActive(true);        
    }

    string getPointString(NODETYPE type, int count)
    {
        string returnString = "";
        switch (type)
        {
            case NODETYPE.NORMALATTACK:
            case NODETYPE.FIREATTACK:
            case NODETYPE.SNOWATTACK:
            case NODETYPE.FIRESNOWATTACK:
                {
                    int outAttackPoint;
                    int outCritcalPoint;
                    float outElementRate;
                    bool magicAttack = (type == NODETYPE.NORMALATTACK) ? false : true;
                    ELEMENTTYPE elementType = (type == NODETYPE.FIREATTACK) ? ELEMENTTYPE.FIRE : (type == NODETYPE.SNOWATTACK) ? ELEMENTTYPE.ICE : ELEMENTTYPE.NONE;
                    BattleManager._Instance.GetAttackPoint(count, magicAttack, elementType, out outAttackPoint,out outCritcalPoint,out outElementRate);

                    returnString = string.Format("AP : {0}\nCR : {1}%", outAttackPoint, outCritcalPoint);                  
                }
                break;
            case NODETYPE.HEAL:
                {
                    int outHealPoint;
                    BattleManager._Instance.GetHealPoint(count, out outHealPoint);
                    returnString = string.Format("Heal : {0}", outHealPoint);
                }
                break;
            case NODETYPE.COIN:
                {
                    int outCoinPoint;
                    BattleManager._Instance.GetCoinPoint(count, out outCoinPoint);
                    returnString = string.Format("Coin : {0}", outCoinPoint);
                }
                break;
        }
        return returnString;
    }



    
}
