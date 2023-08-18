using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    private Vector3 offsetCamera = new Vector3(0, 17, -19);

    public GameObject objectToFollow;

    void LateUpdate()
    {
        transform.position = objectToFollow.transform.position + offsetCamera;
    }
}
