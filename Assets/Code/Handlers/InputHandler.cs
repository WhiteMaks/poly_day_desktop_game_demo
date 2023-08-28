using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private PlayerControls playerControls;
    private CameraHandler cameraHandler;
    private Vector2 movementInput;
    private Vector2 cameraInput;

    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    private void Awake() {
        cameraHandler = CameraHandler.cameraHandler;
    }

    void FixedUpdate() {
        var delta = Time.fixedDeltaTime;

        cameraHandler.FollowTarget(delta);
        cameraHandler.HandleCameraRotation(
            delta, 
            mouseX, 
            mouseY
        );
    }

    public void OnEnable() {
        if (playerControls == null) {
            playerControls = new PlayerControls();
            playerControls.PlayerMovement.Movement.performed += playerControls => movementInput = playerControls.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += playerControls => cameraInput = playerControls.ReadValue<Vector2>();
        }

        playerControls.Enable();
    }

    public void TickInput(float delta) {
        MoveInput(delta);
    }

    private void MoveInput(float delta) {
        horizontal = movementInput.x;
        vertical = movementInput.y;

        moveAmount = Mathf.Clamp01(
            Mathf.Abs(horizontal) + Mathf.Abs(vertical)
        );

        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }

    private void OnDisable() {
        playerControls.Disable();
    }
}
