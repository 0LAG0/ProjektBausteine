using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomControl : MonoBehaviour
{
    float MouseZoomSpeed = 15.0f;
    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse Scrollwheel");
        cam.fieldOfView -= scroll * MouseZoomSpeed;
    }
}
