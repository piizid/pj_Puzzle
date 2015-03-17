using UnityEngine;
using System.Collections;

public class StateBehaviour : StateMachineBehaviour {
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CharacterModel model = animator.gameObject.GetComponent<CharacterModel>();
        if (model)
            model.onStateEnter(stateInfo.shortNameHash);
    }
}
