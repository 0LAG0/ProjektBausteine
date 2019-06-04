using UnityEngine;

// source: http://gyanendushekhar.com/2018/01/11/rotate-gameobject-using-mouse-drag-or-touch-unity-tutorial/

public class RotationControl : MonoBehaviour
{
    float rotationSpeed = 2f;
    GameObject modelsContainer;

    void OnMouseDrag()
    {
        modelsContainer = GameObject.Find("ModelsContainer");

        float xAxisRotation = Input.GetAxis("Mouse X") * rotationSpeed;
        float yAxisRotation = Input.GetAxis("Mouse Y") * rotationSpeed;

        modelsContainer.transform.Rotate(Vector3.down, xAxisRotation);
        modelsContainer.transform.Rotate(Vector3.right, yAxisRotation);
    }

    private void Update()
    {
        if (!Input.GetMouseButton(0))  // || out of preview area
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
        //transform.rotation = new Quaternion(0, 0, 0, 0);
        transform.rotation = Quaternion.identity;
    }
}
