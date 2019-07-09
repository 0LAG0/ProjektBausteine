using UnityEngine;

// source: http://gyanendushekhar.com/2018/01/11/rotate-gameobject-using-mouse-drag-or-touch-unity-tutorial/

public class RotationControl : MonoBehaviour
{
    float rotationSpeed = 2f;
    public GameObject modelsContainer;
    public GameObject previewArea;

    private void Start()
    {
        //modelsContainer = GameObject.Find("ModelsContainer");
        //previewArea = GameObject.Find("Preview_Area");
    }
    void OnMouseDrag()
    {

        float xAxisRotation = Input.GetAxis("Mouse X") * rotationSpeed;
        float yAxisRotation = Input.GetAxis("Mouse Y") * rotationSpeed;

        modelsContainer.transform.Rotate(Vector3.down, xAxisRotation);
        modelsContainer.transform.Rotate(Vector3.right, yAxisRotation);
    }

    private void Update()
    {
        RectTransform previewRectTransform = previewArea.GetComponent<RectTransform>();

        Vector3 mousePos = Input.mousePosition;
        Vector2 normalizedMousePos = new Vector2(mousePos.x / Screen.width, mousePos.y / Screen.height);

        // check if there is a left mouse button as input and if mouse is within preview area
        if (!Input.GetMouseButton(0)
            || normalizedMousePos.x < previewRectTransform.anchorMin.x
            || normalizedMousePos.x > previewRectTransform.anchorMax.x
            || normalizedMousePos.y < previewRectTransform.anchorMin.y
            || normalizedMousePos.y > previewRectTransform.anchorMax.y
            )
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

    public void RotateReset()
    {
        transform.rotation = Quaternion.identity;
    }
}
