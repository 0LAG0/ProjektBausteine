using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Sources
 * Unity Tutorial: Open Panel on Button Click
 * https://www.youtube.com/watch?v=LziIlLB2Kt4&t=120s */


public class OpenManual : MonoBehaviour
{
    public GameObject Panel;

    public void OpenPanel()
    {
        if (Panel != null)
        {
            bool isActive = Panel.activeSelf;
            Panel.SetActive(!isActive);
        }
    }
}
