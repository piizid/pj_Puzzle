using UnityEngine;
using System.Collections;

[System.Serializable]
public class CharacterState
{
    public int _MaxHP;
    public int _CurrentHP;

    public int _AttackPoint;
    public int _MagicPoint;

    public int _AttackDefence;
    public int _MagicDefence;
}

[System.Serializable]
public class CharacterInfo
{
    public string _Name;
    public Sprite _sprite;
    public RuntimeAnimatorController _controller;
}

public enum ATTACKTYPE
{
    NORMAL,
    MAGIC,
}

public enum SIDE
{
    LEFT,
    RIGHT,
}

public class Character : MonoBehaviour 
{
    public delegate void DeadEvent();
    public DeadEvent _DeadEvent;

    [SerializeField]
    SIDE _CharacterSide = SIDE.LEFT;

    [SerializeField]
    CharacterState _State = new CharacterState();
    
    Character _target = null;
    
    [SerializeField]
    CharacterModel _Model = null;

    [SerializeField]
    HUD _hud = null;

    int _PuzzlePoint;

    public Character _Target
    {
        set { _target = value; }
    }

    void Awake()
    {
        Transform model = transform.FindChild("Model");
        if (model)
            _Model = model.GetComponent<CharacterModel>();
        _Model._MotionEvent = this.ModelEvent;
    }

    void Start()
    {
    }

    public bool InitializeCharacter( CharacterState newState, CharacterInfo newInfo )
    {
        if (newState == null || newInfo == null)
            return false;

        _State = newState;

        if (_hud != null)
            _hud.SetState(newState);
        return _Model.Initialize(newInfo._sprite, newInfo._controller);
    }

    public void Action(NODETYPE type, int point)
    {
        _PuzzlePoint = point;
        switch (type)
        {
            case NODETYPE.NORMALATTACK:
                _Model.Start_Motion(MODELMOTION.NORMALATTACK);
                break;
            case NODETYPE.MAGICATTACK:
                _Model.Start_Motion(MODELMOTION.MAGICATTACK);
                break;
            case NODETYPE.SKILL_0:
                _Model.Start_Motion(MODELMOTION.SKILL_0);
                break;
            case NODETYPE.SKILL_1:
                _Model.Start_Motion(MODELMOTION.SKILL_1);
                break;
            case NODETYPE.SKILL_2:
                _Model.Start_Motion(MODELMOTION.SKILL_2);
                break;
        }
    }

    public void Attack( ATTACKTYPE type, int point )
    {
        if (_target == null) return;

        switch (type)
        {
            case ATTACKTYPE.NORMAL:
                point *= _State._AttackPoint;
                break;
            case ATTACKTYPE.MAGIC:
                point *= _State._MagicPoint;
                break;
        }
        _target.RecvAttack(type, point);
    }



    public void RecvAttack(ATTACKTYPE type, int point)
    {
        int defencePoint = 0;
        switch (type)
        {
            case ATTACKTYPE.NORMAL:
                defencePoint = _State._AttackDefence;
                break;
            case ATTACKTYPE.MAGIC:
                defencePoint = _State._MagicDefence;
                break;
        }

        point = Mathf.Max(0, point - defencePoint);
        _hud.CreateDamageFont(point);

        _State._CurrentHP = Mathf.Max(0, _State._CurrentHP - point);

        if (_State._CurrentHP > 0)
            _Model.Start_Motion(MODELMOTION.HIT);
        else
            _Model.Start_Motion(MODELMOTION.DEAD);
    }

    void ModelEvent(MODELMOTION motion, MOTIONEVENT motionEvent)
    {
        switch (motion)
        {
            case MODELMOTION.NORMALATTACK:
                AttackEvent(ATTACKTYPE.NORMAL, motionEvent);
                break;
            case MODELMOTION.MAGICATTACK:
                AttackEvent(ATTACKTYPE.MAGIC, motionEvent);
                break;
            case MODELMOTION.DEAD:
                deadEvent(motionEvent);
                break;
        }
    }

    void AttackEvent(ATTACKTYPE type, MOTIONEVENT motionEvent)
    {
        switch (motionEvent)
        {
            case MOTIONEVENT.EVENT:
                Attack(type, _PuzzlePoint);
                break;
        }
    }

    void deadEvent(MOTIONEVENT motionEvent)
    {
        switch (motionEvent)
        {
            case MOTIONEVENT.EVENT:
                {
                    if (_DeadEvent != null)
                        _DeadEvent();
                }
                break;
        }
    }
    
}
