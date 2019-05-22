﻿using UnityEngine;

// source: http://gyanendushekhar.com/2018/01/11/rotate-gameobject-using-mouse-drag-or-touch-unity-tutorial/

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

    private void Update()
    {
        if (!Input.GetMouseButton(0))
        {
            return;
        }
        else
        {
            OnMouseDrag();
        }
    }

    public void RotateX()
    {
        transform.Rotate(15, 0, 0);
    }

    public void RotateY()
    {
        transform.Rotate(0, 15, 0);
    }

    public void RotateZ()
    {
        transform.Rotate(0, 0, 15);
    }
}
