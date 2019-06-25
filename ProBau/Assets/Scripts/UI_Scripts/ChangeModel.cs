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
    Dropdown m_Dropdown;

    private List<GameObject> models;
    // Default index of the model
    private int selectionIndex = 0;

    public GameObject teapot;
    public GameObject bunny;
    public GameObject htwLogo;
    public GameObject brickItLogo;

    public GameObject activeObject { get; private set; }

    private void Start()
    {
        // Fetch the DropDown component from the GameObject
        m_Dropdown = GetComponent<Dropdown>();

        models = new List<GameObject>();

        foreach (Transform t in transform)
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

        foreach (Transform t in transform)
        {
            // to ensure that any other model (i.e., also imported models) are set to inactive
            t.gameObject.SetActive(false);
        }

        selectionIndex = index;
        activeObject = models[selectionIndex];
        activeObject.SetActive(true);

        // -- NEW APPROACH --
        //GameObject model = GameObject.FindGameObjectWithTag("model");

        //if (model)
        //{
        //    Destroy(model);
        //}

        //switch (index)
        //{
        //    // option: Waehle ein Modell
        //    case 0:
        //        break;

        //    // option: Utah Teapot
        //    case 1:
        //        Instantiate(teapot, GameObject.Find("ModelsContainer").transform);
        //        teapot.tag = "model";
        //        break;

        //    // option: Stanford Bunny
        //    case 2:
        //        Instantiate(bunny, GameObject.Find("ModelsContainer").transform);
        //        bunny.tag = "model";
        //        break;

        //    // option: HTW Logo
        //    case 3:
        //        Instantiate(htwLogo, GameObject.Find("ModelsContainer").transform);
        //        htwLogo.tag = "model";
        //        break;

        //    // option: BrickIT Logo
        //    case 4:
        //        Instantiate(brickItLogo, GameObject.Find("ModelsContainer").transform);
        //        brickItLogo.tag = "model";
        //        break;
        //}
    }
}
