using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Transform cameraObject;
    private Transform playerTransform;

    private InputHandler inputHandler;
    private AnimatorHandler animatorHandler;

    private new Rigidbody rigidbody;
    
    private Vector3 moveDirection;
    private Vector3 normal;
    private Vector3 targetPosition;

    public GameObject normalCamera;
    
    public float movementSpeed = 5;
    public float rotationSpeed = 10;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        cameraObject = Camera.main.transform;
        playerTransform = transform;

        animatorHandler.Initialize();
    }

    // Update is called once per frame
    void Update() {
        var delta = Time.deltaTime;

        inputHandler.TickInput(delta);

        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        
        moveDirection.Normalize();

        moveDirection.y = 0;

        var speed = movementSpeed;

        moveDirection *= speed;

        var projectedVelocity = Vector3.ProjectOnPlane(
            moveDirection, 
            normal
        );

        rigidbody.velocity = projectedVelocity;

        animatorHandler.UpdateAnimatorValues(
            inputHandler.moveAmount,
            0
        );

        if (animatorHandler.canRotate) {
            HandleRotation(delta);
        }
    }

    private void HandleRotation(float delta) {
        var targetDirection = Vector3.zero;
        var moveOverride = inputHandler.moveAmount;

        targetDirection = cameraObject.forward * inputHandler.vertical;
        targetDirection += cameraObject.right * inputHandler.horizontal;

        targetDirection.Normalize();

        targetDirection.y = 0;

        if (targetDirection == Vector3.zero) {
            targetDirection = playerTransform.forward;
        }

        var rotationSpeedTemp = rotationSpeed;
        var targetRotationTemp = Quaternion.LookRotation(targetDirection);

        var targetRotation = Quaternion.Slerp(
            playerTransform.rotation, 
            targetRotationTemp,
            rotationSpeedTemp * delta
        );

        playerTransform.rotation = targetRotation;
    }
}
