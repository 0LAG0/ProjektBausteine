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

    //public GameObject teapot;
    //public GameObject bunny;
    //public GameObject htwLogo;
    //public GameObject brickItLogo;

    public GameObject activeObject { get; private set; }

    private void Start()
    {
        // Fetch the DropDown component from the GameObject
        //m_Dropdown = GetComponent<Dropdown>();

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
        if (index < 0 || index >= models.Count)
        {
            return;
        }

        SetAllInactive();

        selectionIndex = index;
        activeObject = models[selectionIndex];
        activeObject.SetActive(true);
        if (index != 0)
        {
            textureController.ApplyTexture(activeObject);
        }

        // -- NEW APPROACH --
        // TODO: set teapot, bunny, ...
        // TODO: rotate by 180° on y
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
        selectionIndex = 0;
        SetAllInactive();
        models.Add(gameObject);
        activeObject = gameObject;
        m_Dropdown.value = 0;
        activeObject.SetActive(true);
        textureController.ApplyTexture(activeObject);
    }
}
