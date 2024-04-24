using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator animator;

    private PlayerMovementAdvanced movementController;

    private void Start()
    {
        movementController = GetComponent<PlayerMovementAdvanced>();
    }

    private void Update()
    {
        PlayerMovementAdvanced.MovementState currentState = movementController.state;

        animator.SetBool("slowRun", false);
        animator.SetBool("fastRun", false);
        animator.SetBool("crouchIdle", false);

        switch (currentState)
        {
            case PlayerMovementAdvanced.MovementState.walking:
                animator.SetBool("slowRun", true);
                break;
            case PlayerMovementAdvanced.MovementState.sprinting:
                animator.SetBool("fastRun", true);
                break;
            case PlayerMovementAdvanced.MovementState.crouching:
                animator.SetBool("crouchIdle", true);
                break;
            case PlayerMovementAdvanced.MovementState.sliding:
                break;
            case PlayerMovementAdvanced.MovementState.wallrunning:
                break;
            case PlayerMovementAdvanced.MovementState.air:
                break;
            default:
                break;
        }
    }
}