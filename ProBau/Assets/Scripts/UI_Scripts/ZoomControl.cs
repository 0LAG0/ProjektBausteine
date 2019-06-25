using UnityEngine;
using UnityEngine.UI;

// source Zoom, ZoomIn, ZoomOut, Update: http://gyanendushekhar.com/2018/03/03/zoom-using-mouse-scroll-touch-unity-tutorial/
// source AjustZoom: https://forum.unity.com/threads/fit-object-exactly-into-perspective-cameras-field-of-view-focus-the-object.496472/

public class ZoomControl : MonoBehaviour
{
    const float MouseZoomSpeed = 15f;
    const float TouchZoomSpeed = 0.1f;
    const float ZoomMinBound = 0.1f;
    const float ZoomMaxBound = 180f;

    public Slider heightSlider;
    Camera cam;
    float originalFieldOfView;
    GameObject model;

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();
        originalFieldOfView = cam.fieldOfView;
    }

    void Update()
    {
        if (Input.touchSupported)
        {
            // Pinch to zoom
            if (Input.touchCount == 2)
            {
                // get current touch positions
                Touch tZero = Input.GetTouch(0);
                Touch tOne = Input.GetTouch(1);

                // get touch position from the previous frame
                Vector2 tZeroPrevious = tZero.position - tZero.deltaPosition;
                Vector2 tOnePrevious = tOne.position - tOne.deltaPosition;

                float oldTouchDistance = Vector2.Distance(tZeroPrevious, tOnePrevious);
                float currentTouchDistance = Vector2.Distance(tZero.position, tOne.position);

                // get offset value
                float deltaDistance = oldTouchDistance - currentTouchDistance;
                Zoom(deltaDistance, TouchZoomSpeed);
            }
        }
        else
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            Zoom(-scroll, MouseZoomSpeed);
        }
    }

    void Zoom(float deltaMagnitudeDiff, float speed)
    {
        cam.fieldOfView += deltaMagnitudeDiff * speed;
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, ZoomMinBound, ZoomMaxBound);
    }

    public void ZoomIn()
    {
        Zoom(-1.0f, 2.0f);
    }

    public void ZoomOut()
    {
        Zoom(1.0f, 2.0f);
    }

    /** Set position of model cam
     *  depending on size of the current 3D model
     *  to fit model into fov of cam
     *  i.e. to display model appropriately in preview area    
     */
    public void AdjustZoom()
    {
        model = GameObject.FindWithTag("model");
        if (model != null)
        {
            MeshFilter meshFilter = model.GetComponentInChildren<MeshFilter>();
            cam.fieldOfView = originalFieldOfView;

            if (meshFilter)
            {
                var height = (int)Mathf.Round(heightSlider.value);
                Mesh mesh = MeshUtils.RescaleAndCenterPivot(meshFilter.mesh, height);
                mesh.RecalculateBounds();
                meshFilter.mesh = mesh;

                Vector3 size = mesh.bounds.size;

                float objectSize = Mathf.Max(size.x, size.y, size.z);

                float cameraView = 2f * Mathf.Tan(0.5f * Mathf.Deg2Rad * cam.fieldOfView);  // Visible height 1 meter in front of cam
                float distance = objectSize / cameraView;    // Combined wanted distance from the object
                distance += 0.5f * objectSize;  // Estimated offset from the center to the outside of the object
                
                cam.transform.position = mesh.bounds.center + model.transform.position - distance * cam.transform.forward;
            }
        }
    }
}