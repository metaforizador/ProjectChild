using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dashMovement : StateMachineBehaviour
{
    GameObject player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        player = animator.transform.parent.parent.gameObject;

        player.GetComponent<playerMovement>().dashing = true;
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        player.GetComponent<CharacterController>().Move(player.transform.forward * 200 * Time.deltaTime);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        player.GetComponent<playerMovement>().dashing = false;
    }
}
