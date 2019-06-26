using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[RequireComponent(typeof(Dropdown))]
public class TextureController : MonoBehaviour
{
    public ChangeModel modelSelector;

    public List<NamedTexture> namedTextures;
    private Dropdown dropdown;

    public Texture activeTexture;
    private int currentIndex = -1;

    private void Awake()
    {
        dropdown = this.GetComponent<Dropdown>();
        dropdown.ClearOptions();
        List<string> optionNames = new List<string>();
        namedTextures.ForEach(nt => optionNames.Add(nt.name));
        dropdown.AddOptions(optionNames);
        Select(0);
    }

    public void Select(int index)
    {
        currentIndex = index;
        activeTexture = namedTextures[currentIndex].texture;

        if (modelSelector.activeObject != null && modelSelector.activeObject.GetComponentInChildren<Renderer>() != null)
        {
            modelSelector.activeObject.GetComponentInChildren<Renderer>().material.mainTexture = activeTexture;
        }
    }

    public void ApplyTexture(GameObject activeObject)
    {
        if (activeObject != null)
        {
            activeObject.GetComponentInChildren<Renderer>().material.mainTexture = activeTexture;
        }
    }
}
