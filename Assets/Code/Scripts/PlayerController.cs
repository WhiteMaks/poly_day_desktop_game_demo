using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private float smoothVelocity;
    private float targetAngle;
    private float smoothTargetAngle;

    public float speed = 10;
    public float smoothTime = 0.1f;

    // Update is called once per frame
    void Update()
    {
        updateInputs();

        var direction = getDirectionFromInputs();
        if (direction.magnitude >= 0.1f) {
            updateRotation(direction);
            updatePosition(direction);
        }
    }

    private Vector3 getDirectionFromInputs() {
        return new Vector3(
            horizontalInput,
            0f,
            verticalInput
        ).normalized;
    }

    private void updateInputs() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void updatePosition(Vector3 direction) {
        transform.Translate(
            direction * Time.deltaTime * speed, 
            Space.World
        );
    }

    private void updateRotation(Vector3 direction) {
        targetAngle = Mathf.Atan2(
            direction.x, 
            direction.z
        ) * Mathf.Rad2Deg;

        smoothTargetAngle = Mathf.SmoothDampAngle(
            transform.eulerAngles.y,
            targetAngle,
            ref smoothVelocity,
            smoothTime
        );

        transform.rotation = Quaternion.Euler(
            0f,
            smoothTargetAngle,
            0f
        );
    }

}

