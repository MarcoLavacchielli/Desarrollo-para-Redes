using Fusion;
using UnityEngine;

public class PlayerAnimationController : NetworkBehaviour
{
    public NetworkMecanimAnimator networkAnimator;

    //private PlayerMovementAdvanced movementController;
    private PlayerAllInOne movementController;

    private void Start()
    {
        //movementController = GetComponent<PlayerMovementAdvanced>();
        movementController = GetComponent<PlayerAllInOne>();
    }

    private void Update()
    {
        //PlayerMovementAdvanced.MovementState currentState = movementController.state;
        PlayerAllInOne.MovementState currentState = movementController.state;

        networkAnimator.Animator.SetBool("slowRun", false);
        networkAnimator.Animator.SetBool("fastRun", false);
        networkAnimator.Animator.SetBool("crouchIdle", false);

        switch (currentState)
        {
            /*case PlayerMovementAdvanced.MovementState.walking:
                networkAnimator.Animator.SetBool("slowRun", true);
                break;
            case PlayerMovementAdvanced.MovementState.sprinting:
                networkAnimator.Animator.SetBool("fastRun", true);
                break;
            case PlayerMovementAdvanced.MovementState.crouching:
                networkAnimator.Animator.SetBool("crouchIdle", true);
                break;
            case PlayerMovementAdvanced.MovementState.sliding:
                break;
            case PlayerMovementAdvanced.MovementState.wallrunning:
                break;
            case PlayerMovementAdvanced.MovementState.air:
                break;
            default:
                break;*/
            case PlayerAllInOne.MovementState.walking:
                networkAnimator.Animator.SetBool("slowRun", true);
                break;
            case PlayerAllInOne.MovementState.sprinting:
                networkAnimator.Animator.SetBool("fastRun", true);
                break;
            case PlayerAllInOne.MovementState.crouching:
                networkAnimator.Animator.SetBool("crouchIdle", true);
                break;
            case PlayerAllInOne.MovementState.sliding:
                break;
            case PlayerAllInOne.MovementState.wallrunning:
                break;
            case PlayerAllInOne.MovementState.air:
                break;
            default:
                break;
        }
    }
}