using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleControl : MonoBehaviour
{
    Text scaleText;
    GameObject model;
    float sliderValue;

    public void Start()
    {
        scaleText = GameObject.Find("Text_ScalingSize").GetComponent<Text>();
    }

    public void changeHeight()
    {
        sliderValue = GetComponent<Slider>().value;
        model = GameObject.FindWithTag("model");

        if (model != null)
        {
            MeshFilter meshFilter = model.GetComponentInChildren<MeshFilter>();

            if(meshFilter)
            {
                Mesh mesh = meshFilter.mesh;
                //MeshUtils.OptimizeMesh(mesh, sliderValue);
                setText(mesh.bounds.size.x);

                Debug.Log("height: " + mesh.bounds.size.x);
                Debug.Log("slider value: " + sliderValue);
                Debug.Log("---");
            }
        }
        else
        {
            scaleText.text = "Höhe: ";
        }
    }

    private void setText(float height)
    {
        scaleText.text = "Höhe: " + Mathf.Round(height) + " cm";
    }
}
