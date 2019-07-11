using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Sources: 
 * https://www.youtube.com/watch?v=T-AbCUuLViA&feature=youtu.be 
 * Character Selection [Tutorial][C#] - Unity 3d 
 * https://docs.unity3d.com/ScriptReference/UI.Dropdown-value.html */

public class ChangeModel : MonoBehaviour
{
    // Attach this script to a DropDown GameObject
    public Dropdown m_Dropdown;
    public TextureController textureController;
    private List<GameObject> models;
    // Default index of the model
    private int selectionIndex = 0;
    private int initialLength;
    public GameObject activeObject { get; private set; }

    private void Start()
    {

        models = new List<GameObject>();

        foreach (Transform t in transform)
        {
            models.Add(t.gameObject);
            t.gameObject.SetActive(false);
        }
        initialLength = models.Count;
        activeObject = models[selectionIndex];
        activeObject.SetActive(true);
    }

    // Select any model at any time
    public void Select(int index)
    {
        if (index < 0 || index >= models.Count)
        {
            return;
        }

        SetAllInactive();

        selectionIndex = index;
        activeObject = models[selectionIndex];
        activeObject.SetActive(true);
        textureController.ApplyTexture(activeObject);
    }

    public void Reselect()
    {
        var objectsToDestroy = GameObject.FindGameObjectsWithTag(GlobalConstants.containerTag);
        foreach(GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }
        Select(selectionIndex);
    }

    private void SetAllInactive()
    {
        foreach (Transform t in transform)
        {
            // to ensure that any other model (i.e., also imported models) are set to inactive
            t.gameObject.SetActive(false);
        }
    }

    public void SetImportedActive(GameObject gameObject)
    {
        SetAllInactive();
        if (models.Count == initialLength)
        {
            m_Dropdown.AddOptions(new List<string> { "Last Custom Model" });
        }
        else
        {
            models.Remove(models[models.Count - 1]);
        }
        models.Add(gameObject);
        selectionIndex = models.Count - 1;
        activeObject = gameObject;
        m_Dropdown.value = selectionIndex;
        activeObject.SetActive(true);
        textureController.ApplyTexture(activeObject);
    }
}
