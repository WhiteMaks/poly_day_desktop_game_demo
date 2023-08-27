using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    private int vertical;
    private int horizontal;

    public Animator animator;
    public bool canRotate;

    public void Initialize() {
        animator = GetComponent<Animator>();

        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement) {
        animator.SetFloat(
            vertical,
            GetMovementAnimatorValue(verticalMovement), 
            0.1f, 
            Time.deltaTime
        );

        animator.SetFloat(
            horizontal,
            GetMovementAnimatorValue(horizontalMovement),
            0.1f,
            Time.deltaTime
        );
    }

    public void CanRotate() {
        canRotate = true;
    }

    public void StopRotation() {
        canRotate = false;
    }

    private float GetMovementAnimatorValue(float verticalMovement) {
        if (verticalMovement > 0 && verticalMovement < 0.55f) {
            return 0.5f;
        }
        
        if (verticalMovement > 0.55f) {
            return 1;
        } 
        
        if (verticalMovement < 0 && verticalMovement > -0.55f) {
            return -0.5f;
        } 
        
        if (verticalMovement < -0.55f) {
            return -1;
        }

        return 0;
    }
}
