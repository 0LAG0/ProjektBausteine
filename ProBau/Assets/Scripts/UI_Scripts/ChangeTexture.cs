using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Source: 
 * Unity3D Tutorial - Texture Swap
 * https://www.youtube.com/watch?v=aLOru6QQShM 
 * Unity 5 C# Using Dropdown menu to change background color with switch Statement
 * https://www.youtube.com/watch?v=6qQ80N3vRQU */

public class ChangeTexture : MonoBehaviour

{
    
    public Texture[] textures;
    public Dropdown myDropdown;

    public void Update()
    {
        switch(myDropdown.value)
        {
            case 1:
                GetComponent<Renderer>().material.mainTexture = textures[1];
                break;
            case 2:
                GetComponent<Renderer>().material.mainTexture = textures[2];
                break;
            case 3:
                GetComponent<Renderer>().material.mainTexture = textures[3];
                break;
            case 4:
                GetComponent<Renderer>().material.mainTexture = textures[4];
                break;
            case 5:
                GetComponent<Renderer>().material.mainTexture = textures[5];
                break;
            case 6:
                GetComponent<Renderer>().material.mainTexture = textures[6];
                break;
            case 7:
                GetComponent<Renderer>().material.mainTexture = textures[7];
                break;
        }
    }

}
