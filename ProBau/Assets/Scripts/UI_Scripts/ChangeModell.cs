using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Sources: 
 * https://www.youtube.com/watch?v=T-AbCUuLViA&feature=youtu.be 
 * Character Selection [Tutorial][C#] - Unity 3d 
 * https://docs.unity3d.com/ScriptReference/UI.Dropdown-value.html */

public class ChangeModell : MonoBehaviour
{
    private List<GameObject> models;
    // Default index of the model
    private int selectionIndex = 0;

    private void Start()
    {
        models = new List<GameObject>();

        foreach(Transform t in transform)
        {
            models.Add(t.gameObject); 
            t.gameObject.SetActive(false);
        }

        models[selectionIndex].SetActive(true);
    }

    private void Update() // ändern für Dropdown!
    {
        if (Input.GetKeyDown(KeyCode.A))
            Select(2);
        if (Input.GetKeyDown(KeyCode.S))
            Select(0);
        if (Input.GetKeyDown(KeyCode.D))
            Select(1);

    }

    // Select any model at any time
    public void Select(int index)
    {
        if (index == selectionIndex)
        {
            return;
        }

        if (index < 0 || index >= models.Count)
        {
            return;
        }

        models[selectionIndex].SetActive(false);
        selectionIndex = index;
        models[selectionIndex].SetActive(true);
    }

}
