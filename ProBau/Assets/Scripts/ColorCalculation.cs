using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Globalization;
using System;

public class ColorCalculation : MonoBehaviour
{
    static List<string> hexColorsList2 = new List<string>()
    {"#FFEC6C", "#FAC80A", "#FCAC00", "#D67923", "#B40000","#720012", "#901F76", "#D3359D", "#FF9ECD",
    "#CDA4DE", "#A06EB9", "#441A91", "#19325A", "#1E5AA8", "#7396C8", "#9DC3F7", "#70819A", "#68C3E2", "#469BC3",
    "#D3F2EA", "#E2F99A", "#A5CA18", "#58AB41", "#00852B", "#00451A", "#3E3C39", "#1B2A34", "#646464", "#8C8C8C",
    "#969696", "#F4F4F4", "#708E7C", "#77774E", "#897D62", "#B0A06F", "#FFC995", "#BB805A", "#AA7D55", "#91501C",
    "#372100"};

    List<Color> colorList = getRgbList(hexColorsList2);

    public GameObject myObject;
    public Texture2D myTexture;

    //void Start()
    //{
    //    myObject = GameObject.FindWithTag("object");
    //    myTexture = myObject.GetComponent<Renderer>().material.mainTexture as Texture2D;
    //    Texture2D newTexture = colorCalculate(myTexture);
    //    newTexture.Apply();
    //    myObject.GetComponent<Renderer>().material.mainTexture = newTexture as Texture;
    //}

    private Texture2D colorCalculate(Texture2D colorTexture)
    {
        int defaultColorNumber = colorList.Count;
        int width = colorTexture.width;
        int height = colorTexture.height;
        Color[] myObjColor = getColorMap(colorTexture);

        for (int j = 0; j < myObjColor.Length; j++)
        {
            Dictionary<int, float> distanceList = new Dictionary<int, float>();
            Color currentColor = myObjColor[j];
            for (int i = 0; i < defaultColorNumber; i++)
            {
                Color indexColor = colorList.ElementAt(i);
                float distance = Mathf.Pow(currentColor.r - indexColor.r, 2) + Mathf.Pow(currentColor.g - indexColor.g, 2) +
                    Mathf.Pow(currentColor.b - indexColor.b, 2) + Mathf.Pow(currentColor.a - indexColor.a, 2);
                distanceList.Add(i, distance);
            }

            if (distanceList.Count >= 0)
            {
                float minDistance = distanceList.Values.Min();
                int indexMinDistance = distanceList.First(x => x.Value == minDistance).Key;
                Color nearestColor = colorList.ElementAt(indexMinDistance);
                myObjColor[j] = nearestColor;
            }
        }

        Texture2D newTexture = setColorMap(myObjColor, width, height);
        return newTexture;
    }

    public Color[] getColorMap(Texture2D colorMap)
    {
        int width = colorMap.width;
        int height = colorMap.height;
        int size = width * height;
        Color[] argb = new Color[size];
        argb = colorMap.GetPixels(0, 0, width, height);
        Debug.Log(argb[0]);
        return argb;
    }

    public Texture2D setColorMap(Color[] argb, int width, int height)
    {
        Texture2D newColorMap = new Texture2D(width, height);
        Debug.Log(argb[0]);
        newColorMap.SetPixels(argb);
        return newColorMap;
    }


    //Transform source color to lego color
    public Color convertColor(Color srcColor)
    {
        Color nearestColor = new Color();
        int defaultColorNumber = colorList.Count;
        Dictionary<int, float> distanceList = new Dictionary<int, float>();
        for (int i = 0; i < defaultColorNumber; i++)
        {
            Color indexColor = colorList.ElementAt(i);
            float distance = Mathf.Pow(srcColor.r - indexColor.r, 2) + Mathf.Pow(srcColor.g - indexColor.g, 2) +
                Mathf.Pow(srcColor.b - indexColor.b, 2) + Mathf.Pow(srcColor.a - indexColor.a, 2);
            distanceList.Add(i, distance);
        }

        if (distanceList.Count >= 0)
        {
            float minDistance = distanceList.Values.Min();
            int indexMinDistance = distanceList.First(x => x.Value == minDistance).Key;
            nearestColor = colorList.ElementAt(indexMinDistance);
        }

        return nearestColor;
    }

        private static List<Color> getRgbList(List<string> hexColors)
    {
        List<Color> colorVectors = new List<Color>();
        for (int i = 0; i < hexColors.Count; i++)
        {
            string hexColor = hexColors.ElementAt(i);
            if (hexColor.IndexOf('#') != -1)
                hexColor = hexColor.Replace("#", "");

            float r = 0;
            float g = 0;
            float b = 0;
            float a = 1;

            r = int.Parse(hexColor.Substring(0, 2), NumberStyles.AllowHexSpecifier) / 255.3f;
            g = int.Parse(hexColor.Substring(2, 2), NumberStyles.AllowHexSpecifier) / 255.3f;
            b = int.Parse(hexColor.Substring(4, 2), NumberStyles.AllowHexSpecifier) / 255.3f;

            Color colorVector = new Color(r, g, b, a);
            colorVectors.Add(colorVector);
        }

        return colorVectors;
    }
}
