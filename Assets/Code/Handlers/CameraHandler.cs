using System.Collections;
using System.Collections.Generic;
using Unity.XR.OpenVR;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public static CameraHandler cameraHandler;

    private Transform myTransform;
    private Vector3 cameraTransformPosition;
    private LayerMask ignoreLayers;
    private Vector3 cameraFollowVelocity = Vector3.zero;

    private float targetPosition;
    private float defaultPosition;
    private float lookAngle;
    private float pivotAngle;

    public Transform targetTransform;
    public Transform cameraTransform;
    public Transform cameraPivotTransform;

    public float lookSpeed = 0.1f;
    public float followSpeed = 0.1f;
    public float pivotSpeed = 0.03f;
    public float minPivot = -35;
    public float maxPivot = 35;
    public float cameraSphereRadius = 0.2f;
    public float cameraCollisionOffset = 0.2f;
    public float minCollisionOffset = 0.2f;

    private void Awake() {
        cameraHandler = this;
        myTransform = transform;
        defaultPosition = cameraTransform.localPosition.z;
        ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
    }

    public void FollowTarget(float delta) {
        var targetPosition = Vector3.SmoothDamp(
            myTransform.position, 
            targetTransform.position,
            ref cameraFollowVelocity,
            delta / followSpeed
        );

        myTransform.position = targetPosition;

        HandleCameraCollisions(delta);
    }

    public void HandleCameraRotation(float delta, float mouseX, float mouseY) {
        lookAngle += (mouseX * lookSpeed) / delta;
        pivotAngle -= (mouseY * pivotSpeed) / delta;

        pivotAngle = Mathf.Clamp(
            pivotAngle, 
            minPivot, 
            maxPivot
        );

        var rotation = Vector3.zero;
        rotation.y = lookAngle;

        var targetRotation = Quaternion.Euler(rotation);

        myTransform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;

        targetRotation = Quaternion.Euler(rotation);

        cameraPivotTransform.localRotation = targetRotation;
    }

    private void HandleCameraCollisions(float delta) {
        targetPosition = defaultPosition;
        var direction = cameraTransform.position - cameraPivotTransform.position;
        direction.Normalize();

        RaycastHit hit;

        var isCollized = Physics.SphereCast(
            cameraPivotTransform.position,
            cameraSphereRadius,
            direction,
            out hit,
            Mathf.Abs(targetPosition),
            ignoreLayers
        );

        if (isCollized) {
            var distance = Vector3.Distance(
                cameraPivotTransform.position,
                hit.point
            );

            targetPosition = -(distance - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minCollisionOffset) {
            targetPosition = -minCollisionOffset;
        }

        cameraTransformPosition.z = Mathf.Lerp(
            cameraTransform.localPosition.z, 
            targetPosition, 
            delta / 0.2f
        );

        cameraTransform.localPosition = cameraTransformPosition;
    }
}
