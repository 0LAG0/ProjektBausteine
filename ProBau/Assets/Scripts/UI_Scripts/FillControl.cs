using UnityEngine;
using UnityEngine.UI;

public class FillControl : MonoBehaviour
{
    Text fillText;

    public void Start()
    {
        fillText = GameObject.Find("Text_Fill-Thickness").GetComponent<Text>();
        setText(GetComponent<Slider>().value);
    }

    public void changeFillValue()
    {
        setText(GetComponent<Slider>().value);
    }

    private void setText(float thickness)
    {
        fillText.text = "Wandstärke: " + (Mathf.Round(thickness) + 1) + " cm";
    }
}
