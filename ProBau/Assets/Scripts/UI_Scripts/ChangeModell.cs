using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;

/* Sources: 
 * https://www.youtube.com/watch?v=T-AbCUuLViA&feature=youtu.be 
 * Character Selection [Tutorial][C#] - Unity 3d 
 * https://docs.unity3d.com/ScriptReference/UI.Dropdown-value.html */

public class ChangeModell : MonoBehaviour
{
    // Attach this script to a DropDown GameObject
    Dropdown m_Dropdown;

    private List<GameObject> models;
    // Default index of the model
    private int selectionIndex = 0;

    private void Start()
    {
        // Fetch the DropDown component from the GameObject
        m_Dropdown = GetComponent<Dropdown>();

        models = new List<GameObject>();

        foreach(Transform t in transform)
        {
            models.Add(t.gameObject); 
            t.gameObject.SetActive(false);
        }

        models[selectionIndex].SetActive(true);
    }

    // Select any model at any time
    public void Select(int index)
    {
        if (index == selectionIndex || index < 0 || index >= models.Count)
        {
            return;
        }

        models[selectionIndex].SetActive(false);
        selectionIndex = index;
        models[selectionIndex].SetActive(true);
    }


    // source: https://docs.unity3d.com/ScriptReference/EditorUtility.OpenFilePanel.html
    // found these, but so far couldn't use them: http://wiki.unity3d.com/index.php?title=ObjImporter & http://wiki.unity3d.com/index.php/FastObjImporter
    /// <summary>
    /// Open file system dialog to import own 3D modell
    /// </summary>
    public void OpenFilePanel()
    {
        GameObject modell = GameObject.Find("ModelsContainer");

        string path = EditorUtility.OpenFilePanel("Importiere dein 3D Modell", "", "obj");

        if (path.Length != 0)
        {
            var newModell = File.ReadAllBytes(path);
            // TODO convert byte[] to modell/game object
            //models.Add(newModell);
            //Select(models.IndexOf(newModell));
        }
    }
}
