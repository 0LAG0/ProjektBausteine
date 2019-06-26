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

    public void changeHeight()
    {
        setText(GetComponent<Slider>().value);
    }

    private void setText(float height)
    {
        fillText.text = "Wandstärke: " + (Mathf.Round(height) + 1) + " cm";
    }
}
