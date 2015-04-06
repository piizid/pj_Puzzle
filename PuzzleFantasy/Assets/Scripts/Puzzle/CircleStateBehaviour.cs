using UnityEngine;
using System.Collections;

public class CircleStateBehaviour : StateMachineBehaviour {
    public delegate void StateEnter(PuzzleCircle.CIRCLESTATE enterState);

    public StateEnter _StateEnterEvent = null;

    static int code_Idle = Animator.StringToHash("Idle");
    static int code_Born = Animator.StringToHash("Born");
    static int code_DestoryStart = Animator.StringToHash("Destory_Start");
    static int code_Destory = Animator.StringToHash("Destory");
    static int code_Select = Animator.StringToHash("SelectIdle");

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_StateEnterEvent != null)
            _StateEnterEvent(getState(stateInfo.shortNameHash));
        //else
        //    Debug.Log(getState(stateInfo.shortNameHash));
    }

    PuzzleCircle.CIRCLESTATE getState( int code )
    {
        if (code == code_Idle) return PuzzleCircle.CIRCLESTATE.Idle;
        if (code == code_Born) return PuzzleCircle.CIRCLESTATE.Create;
        if (code == code_DestoryStart) return PuzzleCircle.CIRCLESTATE.DestroyStart;
        if (code == code_Select) return PuzzleCircle.CIRCLESTATE.Select;
        if (code == code_Destory) return PuzzleCircle.CIRCLESTATE.DestroyEnd;
        return PuzzleCircle.CIRCLESTATE.DestroyEnd;
    }

}
