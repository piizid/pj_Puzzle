using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum MODELMOTION
{
    IDLE,
    NORMALATTACK,
    MAGICATTACK,
    HEAL,
    COIN,
    HIT,
    DEAD,
}

public enum MOTIONEVENT
{
    START,
    EVENT,
    END
}


public class CharacterModel : MonoBehaviour {
    static int CODE_IDLE = Animator.StringToHash("Base Layer.IDLE");
    static int CODE_NORMALATTACK = Animator.StringToHash("Base Layer.NormalAttack");
    static int CODE_MAGICATTACK = Animator.StringToHash("Base Layer.MagicAttack");
    static int CODE_HEAL = Animator.StringToHash("Base Layer.Heal");
    static int CODE_COIN = Animator.StringToHash("Base Layer.Coin");
    static int CODE_HIT = Animator.StringToHash("Base Layer.Hit");
    static int CODE_DEAD = Animator.StringToHash("Base Layer.Dead");

    const string TRIGGER_NORMALATTACK = "NormalAttack";
    const string TRIGGER_MAGICATTACK = "MagicAttack";
    const string TRIGGER_Heal = "Heal";
    const string TRIGGER_Coin = "Coin";
    const string TRIGGER_Hit = "Hit";
    const string TRIGGER_Dead = "Dead";

    public delegate void MotionnEvent( MODELMOTION motion, MOTIONEVENT motionEvent );
    public MotionnEvent _MotionEvent;

    [SerializeField]
    Animator _anim;

    int _preAnimatorCode;

    void Awake()
    {
        _anim = GetComponent<Animator>();

        if( _MotionEvent == null )
            _MotionEvent = this.testLog;
    }


    void Start()
    {
        _preAnimatorCode = _anim.GetCurrentAnimatorStateInfo(0).nameHash;
    }

    public bool Initialize( Sprite sprite , RuntimeAnimatorController controller )
    {
        if (sprite == null || controller == null)
            return false;

        Image image = GetComponent<Image>();
        image.sprite = sprite;
        image.color = Color.white;
        image.SetNativeSize();

        _anim.runtimeAnimatorController = controller;
        return true;
    }

    void Update()
    {
        int currentCode = _anim.GetCurrentAnimatorStateInfo(0).nameHash;

        if (_preAnimatorCode != currentCode)
        {
            stateChange(_preAnimatorCode, currentCode);
            _preAnimatorCode = currentCode;
        }
    }

    void stateChange(int preCode, int curCode)
    {
        if (_MotionEvent == null)
            return;

        _MotionEvent(getMotion(preCode), MOTIONEVENT.END);
        _MotionEvent(getMotion(curCode), MOTIONEVENT.START);
    }

    MODELMOTION getMotion(int code)
    {
        if (code == CODE_DEAD) return MODELMOTION.DEAD;
        if (code == CODE_HIT) return MODELMOTION.HIT;
        if (code == CODE_IDLE) return MODELMOTION.IDLE;
        if (code == CODE_MAGICATTACK) return MODELMOTION.MAGICATTACK;
        if( code == CODE_NORMALATTACK ) return MODELMOTION.NORMALATTACK;
        if (code == CODE_HEAL) return MODELMOTION.HEAL;
        if (code == CODE_COIN) return MODELMOTION.COIN;
        return MODELMOTION.IDLE;
    }

    public void Start_Motion(MODELMOTION motion)
    {
        string triggerName = "";
        switch (motion)
        {
            case MODELMOTION.NORMALATTACK: triggerName = TRIGGER_NORMALATTACK; break;
            case MODELMOTION.MAGICATTACK:  triggerName = TRIGGER_MAGICATTACK;  break;
            case MODELMOTION.HEAL:         triggerName = TRIGGER_Heal;      break;
            case MODELMOTION.COIN:         triggerName = TRIGGER_Coin;      break;
            case MODELMOTION.HIT:          triggerName = TRIGGER_Hit;          break;
            case MODELMOTION.DEAD:         triggerName = TRIGGER_Dead;         break;
        }
        _anim.SetTrigger(triggerName);
    }
  
    public void MotionEvent()
    {
//        Debug.Log("event");
        _MotionEvent(getMotion(_anim.GetCurrentAnimatorStateInfo(0).nameHash), MOTIONEVENT.EVENT);
    }

    void testLog(MODELMOTION motion, MOTIONEVENT motionEvent)
    {
        Debug.Log(motion.ToString() + "Motion, " + motionEvent.ToString());
    }
}
