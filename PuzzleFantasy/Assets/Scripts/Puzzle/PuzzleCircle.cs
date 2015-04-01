using UnityEngine;
using System.Collections;

public class PuzzleCircle : MonoBehaviour {

    public enum CIRCLESTATE
    {
        Create,
        Idle,
        Select,
        DestroyStart,
        DestroyEnd,
    };

    Animator _animator = null;
    Animator _IconAnimator = null;

    CIRCLESTATE _state = CIRCLESTATE.Create;
    public CIRCLESTATE _State { get { return _state; } }
    GameObject _iconObj = null;
    
    void Awake()
    {
        _animator = GetComponent<Animator>();
        CircleStateBehaviour stateBehaviour = _animator.GetBehaviour<CircleStateBehaviour>();
        if (stateBehaviour)
            stateBehaviour._StateEnterEvent = this.stateAnimatorStateEnter;
    }

    void stateAnimatorStateEnter(CIRCLESTATE enterState)
    {
        _state = enterState;

        if (_state == CIRCLESTATE.DestroyEnd && _iconObj != null )
        {
            Destroy(_iconObj);
            _iconObj = null;
        }
    }

    public bool CreateCircle( GameObject IconPrefab )
    {
        if (IconPrefab == null)
            return false;

        GameObject newObj = Instantiate(IconPrefab, transform.position, transform.rotation) as GameObject;
        newObj.transform.parent = transform;
        _IconAnimator = newObj.GetComponent<Animator>();
        
        iconSetSelect(false);
        circleSelect(false);
        circleLife(true);

        _iconObj = newObj;

        return true;
    }

    public void SetSelect(bool select)
    {
        circleSelect(select);
        iconSetSelect(select);
    }

    public void Destroy()
    {
        iconSetSelect(false);
        circleSelect(false);
        circleLife(false);
    }

    void iconSetSelect(bool select)
    {
        if (_IconAnimator == null)
            return;
        _IconAnimator.SetBool("Select", select);
    }

    void circleSelect(bool select)
    {
        if (_animator == null)
            return;
        _animator.SetBool("Select", select);
    }

    void circleLife(bool select)
    {
        if (_animator == null)
            return;
        _animator.SetBool("Life", select);
    }
}
