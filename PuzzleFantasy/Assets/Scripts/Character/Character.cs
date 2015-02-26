using UnityEngine;
using System.Collections;


[System.Serializable]
public class CharacterInfo
{
    public string _Name;
    public Sprite _sprite;
    public RuntimeAnimatorController _controller;
}

public abstract class Character : MonoBehaviour 
{
    public delegate void DeadEvent();
    public DeadEvent _DeadEvent;

    public delegate void HitEndEvent();
    public HitEndEvent _HitEndEvent;

    [SerializeField]
    protected CharacterState_Life _life = new CharacterState_Life();
    
    [SerializeField]
    protected CharacterModel _Model = null;

    [SerializeField]
    protected int _PuzzlePoint = 0;

    [SerializeField]
    protected ELEMENTTYPE _AttackElement;

    public string _CharacterName
    {
        get;
        private set;
    }


    void Awake()
    {
        Transform model = transform.FindChild("Model");
        if (model)
            _Model = model.GetComponent<CharacterModel>();
        _Model._MotionEvent = this.modelEvent;
    }


    protected bool InitializeCharacter( CharacterInfo info, CharacterState_Life newState )
    {
        if (newState == null || info == null )
            return false;

        _life = newState;

        _CharacterName = info._Name;
        return _Model.Initialize( info._sprite, info._controller );
    }

    public void Action(NODETYPE type, int point)
    {
        _PuzzlePoint = point;
        switch (type)
        {
            case NODETYPE.NORMALATTACK:
                _Model.Start_Motion(MODELMOTION.NORMALATTACK);
                _AttackElement = ELEMENTTYPE.NONE;
                break;
            case NODETYPE.FIREATTACK:
                _AttackElement = ELEMENTTYPE.FIRE;
                _Model.Start_Motion(MODELMOTION.MAGICATTACK);
                break;
            case NODETYPE.SNOWATTACK:
                _AttackElement = ELEMENTTYPE.ICE;
                _Model.Start_Motion(MODELMOTION.MAGICATTACK);
                break;
            case NODETYPE.FIRESNOWATTACK:
                _AttackElement = ELEMENTTYPE.NONE;
                _Model.Start_Motion(MODELMOTION.MAGICATTACK);
                break;
            case NODETYPE.HEAL:
                _Model.Start_Motion(MODELMOTION.HEAL);
                break;
            case NODETYPE.COIN:
                _Model.Start_Motion(MODELMOTION.COIN);
                break;
        }
    }

    void modelEvent(MODELMOTION motion, MOTIONEVENT motionEvent)
    {
        switch( motion )
        {
            case MODELMOTION.DEAD:
                deadEvent( motionEvent );
                break;
            case MODELMOTION.HIT:
                hitEvent( motionEvent );
                break;
        }
        ModelEvent(motion, motionEvent);
    }

    abstract protected void ModelEvent(MODELMOTION motion, MOTIONEVENT motionEvent);

    void hitEvent(MOTIONEVENT motionEvent)
    {
        switch (motionEvent)
        {
            case MOTIONEVENT.END:
                {
                    if (_HitEndEvent != null)
                        _HitEndEvent();
                }
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

    protected void hit( int attackPoint )
    {
        _life._CurHP = Mathf.Max(0, _life._CurHP - attackPoint);

        if (_life._CurHP > 0)
            _Model.Start_Motion(MODELMOTION.HIT);
        else
            _Model.Start_Motion(MODELMOTION.DEAD);
    }
}

