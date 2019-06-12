using UnityEngine;

// source: http://gyanendushekhar.com/2018/03/03/zoom-using-mouse-scroll-touch-unity-tutorial/

public class ZoomControl : MonoBehaviour
{
    const float MouseZoomSpeed = 15.0f;
    const float TouchZoomSpeed = 0.1f;
    const float ZoomMinBound = 0.1f;
    const float ZoomMaxBound = 180.0f;
    const float ZoomRatio = 0.2f;

    GameObject modelContainer;
    GameObject previewArea;
    Camera cam;
    float originalFieldOfView;

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();
        originalFieldOfView = cam.fieldOfView;
    }

    void Update()
    {
        previewArea = GameObject.Find("Preview_Area");
        RectTransform previewRectTransform = previewArea.GetComponent<RectTransform>();

        Vector3 mousePos = Input.mousePosition;
        Vector2 normalizedMousePos = new Vector2(mousePos.x / Screen.width, mousePos.y / Screen.height);

        if (normalizedMousePos.x > previewRectTransform.anchorMin.x
            && normalizedMousePos.x < previewRectTransform.anchorMax.x
            && normalizedMousePos.y > previewRectTransform.anchorMin.y
            && normalizedMousePos.y < previewRectTransform.anchorMax.y
            )
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
        modelContainer = GameObject.Find("ModelsContainer");
        MeshFilter meshFilter = modelContainer.GetComponentInChildren<MeshFilter>(false);

        //if (meshFilter)
        //{
        //    Debug.Log("mesh exists");

        //    Vector3 size = meshFilter.mesh.bounds.size;

        //    if (size.x > 0 && size.y > 0)
        //    {
        //        Debug.Log("-- size > 0");

        //        float newFoV = Mathf.Max(size.x, size.y) / ZoomRatio;

        //        if (newFoV < ZoomMinBound || newFoV > ZoomMaxBound)
        //        {
        //            // TODO scale
        //            Debug.Log("---- scaling needed");
        //        }

        //        cam.fieldOfView = Mathf.Clamp(newFoV, ZoomMinBound, ZoomMaxBound);

        //        Debug.Log("-- size: " + size);
        //        Debug.Log("-- new fov: " + newFoV);
        //        Debug.Log("-- cam.fieldOfView: " + cam.fieldOfView);
        //    }
        //}
        //else
        //{
        //    Debug.Log("no mesh...");
              cam.fieldOfView = originalFieldOfView;
        //}

        //Debug.Log("-----------------------------------------------------");

        //Vector3 size = modelContainer.GetComponentInChildren<MeshRenderer>(false).bounds.size;
        //Vector3 scale = modelContainer.GetComponentInChildren<MeshRenderer>(false).transform.lossyScale;
    }
}