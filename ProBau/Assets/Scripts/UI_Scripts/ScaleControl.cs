using UnityEngine;
using UnityEngine.UI;

public class ScaleControl : MonoBehaviour
{
    Text scaleText;

    public void Start()
    {
        scaleText = GameObject.Find("Text_ScalingSize").GetComponent<Text>();
        setText(GetComponent<Slider>().value);
    }

    public void changeHeight()
    {
        setText(GetComponent<Slider>().value);
    }

    private void setText(float height)
    {
        scaleText.text = "Höhe: " + Mathf.Round(height) + " cm";
    }
}
