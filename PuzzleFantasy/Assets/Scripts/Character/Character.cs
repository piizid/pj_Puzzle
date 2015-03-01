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
    public delegate void MotionEvent(MODELMOTION motion, MOTIONEVENT motionEvent);
    public MotionEvent _MotionEvent;

    [SerializeField]
    protected CharacterState_Life _life = new CharacterState_Life();

    public CharacterState_Life _Life
    {
        get { return _life; }
    }

    [SerializeField]
    protected CharacterModel _Model = null;

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
  

    void modelEvent(MODELMOTION motion, MOTIONEVENT motionEvent)
    {
        ModelEvent(motion, motionEvent);
        if (_MotionEvent != null)
            _MotionEvent(motion, motionEvent);
    }

    abstract protected void ModelEvent(MODELMOTION motion, MOTIONEVENT motionEvent);



    protected void hit( int attackPoint )
    {
        _life._CurHP = Mathf.Max(0, _life._CurHP - attackPoint);

        if (_life._CurHP > 0)
            _Model.Start_Motion(MODELMOTION.HIT);
        else
            _Model.Start_Motion(MODELMOTION.DEAD);
    }
}

