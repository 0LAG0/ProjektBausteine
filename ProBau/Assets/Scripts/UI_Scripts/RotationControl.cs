using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationControl : MonoBehaviour
{
    float rotationSpeed = 2f;

    void OnMouseDrag()
    {
        float xAxisRotation = Input.GetAxis("Mouse X") * rotationSpeed;
        float yAxisRotation = Input.GetAxis("Mouse Y") * rotationSpeed;

        transform.Rotate(Vector3.down, xAxisRotation);
        transform.Rotate(Vector3.right, yAxisRotation);
    }
}
