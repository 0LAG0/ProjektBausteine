using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Source: 
 * Unity3D Tutorial - Texture Swap
 * https://www.youtube.com/watch?v=aLOru6QQShM 
 * How to Assign Materials Using C# Code
 * https://www.youtube.com/watch?v=AAc_R9bp6zI */

public class ChangeTexture : MonoBehaviour

{
    // Attach this script to a DropDown GameObject
    /*Dropdown m_Dropdown;
    // Fetch the DropDown component from the GameObject
    m_Dropdown = GetComponent<Dropdown>();
    */
    public Texture[] textures;
    public int currentTexture;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentTexture++;
            currentTexture %= textures.Length;
            GetComponent<Renderer>().material.mainTexture = textures[currentTexture];
        }

    }

}
