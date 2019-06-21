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
            // keine Textur, Standard
            case 0:
                GetComponent<Renderer>().material.mainTexture = textures[0];
                break;
            // Textur Zement
            case 1:
                GetComponent<Renderer>().material.mainTexture = textures[1];
                break;
            // Textur Keramik
            case 2:
                GetComponent<Renderer>().material.mainTexture = textures[2];
                break;
            // Textur HTW Gruen
            case 3:
                GetComponent<Renderer>().material.mainTexture = textures[3];
                break;
            // Textur Leopard
            case 4:
                GetComponent<Renderer>().material.mainTexture = textures[4];
                break;
            // Textur Gemaelde
            case 5:
                GetComponent<Renderer>().material.mainTexture = textures[5];
                break;
            // Textur Himmel
            case 6:
                GetComponent<Renderer>().material.mainTexture = textures[6];
                break;
            // Textur Holz
            case 7:
                GetComponent<Renderer>().material.mainTexture = textures[7];
                break;
        }
    }

}
