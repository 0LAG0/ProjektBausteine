using UnityEngine;

// source: http://gyanendushekhar.com/2018/03/03/zoom-using-mouse-scroll-touch-unity-tutorial/

public class ZoomControl : MonoBehaviour
{
    const float MouseZoomSpeed = 15.0f;
    const float TouchZoomSpeed = 0.1f;
    const float ZoomMinBound = 0.1f;
    const float ZoomMaxBound = 180.0f;
    const float ZoomRatio = 5f;
    //const float ZoomRatioHeight = 1;

    Camera cam;
    float originalFieldOfView;
    GameObject modelContainer;

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

    // Set zoom (fov of cam) to an appropriate value depending on the ratio (fov to model size)
    public void AdjustZoom()
    {
        // OLD ZOOM RESET
        cam.fieldOfView = originalFieldOfView;

        /* NEW IDEA: cam fov depending on width/height of model 
         * TODO: force to get data from child object && how to get data?
         */
        //modellContainer = GameObject.Find("ModelsContainer");
        //Vector3 size = modelContainer.GetComponentInChildren<MeshRenderer>(false).bounds.size;
        //Vector3 scale = modelContainer.GetComponentInChildren<MeshRenderer>(false).transform.lossyScale;

        //float newFoV = Mathf.Max(scale.x, scale.y) / ZoomRatio;

        //cam.fieldOfView = Mathf.Clamp(newFoV, ZoomMinBound, ZoomMaxBound);
    }
}