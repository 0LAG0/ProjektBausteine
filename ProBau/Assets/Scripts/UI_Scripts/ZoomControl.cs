using UnityEngine;

// source Zoom, ZoomIn, ZoomOut, Update: http://gyanendushekhar.com/2018/03/03/zoom-using-mouse-scroll-touch-unity-tutorial/
// source AjustZoom: https://forum.unity.com/threads/fit-object-exactly-into-perspective-cameras-field-of-view-focus-the-object.496472/

public class ZoomControl : MonoBehaviour
{
    const float MouseZoomSpeed = 15f;
    const float TouchZoomSpeed = 0.1f;
    const float ZoomMinBound = 0.1f;
    const float ZoomMaxBound = 180.0f;
    const float ZoomRatio = 0.2f;

    GameObject model;
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

    /** Set position of model cam
     *  depending on size of the current 3D model
     *  to fit model into fov of cam
     *  i.e. to display model appropriately in preview area    
     */
    public void AdjustZoom()
    {
        model = GameObject.FindWithTag("model");
        MeshFilter meshFilter = model.GetComponent<MeshFilter>();

        //if (meshFilter)
        //{
        //    Vector3 size = meshFilter.mesh.bounds.size;

        //    if (size.x > 0 && size.y > 0)
        //    {
        //        float newFoV = Mathf.Max(size.x, size.y) / ZoomRatio;

        //        if (newFoV < ZoomMinBound || newFoV > ZoomMaxBound)
        //        {
        //            // TODO scale
        //            Debug.Log("---- scaling needed");
        //        }

        //        cam.fieldOfView = Mathf.Clamp(newFoV, ZoomMinBound, ZoomMaxBound);
        //    }
        //}
        //else
        //{
              cam.fieldOfView = originalFieldOfView;
        //}

        //Vector3 size = model.GetComponent<MeshRenderer>().bounds.size;
        //Vector3 scale = model.GetComponent<MeshRenderer>().transform.lossyScale;
    }
}