using UnityEngine;
using ProjectChild.Characters;
using ProjectChild.Inputs;

public class dashMovement : StateMachineBehaviour
{
    GameObject player;
    Character character;
    float dashSpeed;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        //player = animator.transform.parent.parent.gameObject;
        //player.GetComponent<playerMovement>().dashing = true;
        dashSpeed = 200;

        character = animator.GetComponent<Character>();
        player = character.gameObject;
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (animatorStateInfo.IsName("Armature_Dash"))
        {
            var input = new MovementInput()
            {
                direction = character.transform.forward,
                dashing = true
            };
            character.Move(input);

            // player.GetComponent<CharacterController>().Move(player.transform.forward * dashSpeed * Time.deltaTime);
        }

        if (animatorStateInfo.IsName("Armature_DashEnd"))
        {
            //dashSpeed -= (500 * Time.deltaTime);
            //player.GetComponent<CharacterController>().Move(player.transform.forward * dashSpeed * Time.deltaTime);

            var input = new MovementInput()
            {
                direction = character.transform.forward,
                exitingDash = true
            };
            character.Move(input);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // player.GetComponent<playerMovement>().dashing = false;
    }
}
