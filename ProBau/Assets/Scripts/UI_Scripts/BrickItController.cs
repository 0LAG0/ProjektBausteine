using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrickItController : MonoBehaviour
{
    public MultiToggle bricks;
    public MultiToggle colors;
    public Slider depth;
    public Slider scaling;
    public ChangeModel modelSelection;
    public ConversionController conversionController;
    public TextureController textureController;

    private List<Vector3Int> getBricks()
    {
        if (bricks._allToggle.isOn)
        {
            return GlobalConstants.BlockTypes;
        }
        var output = new List<Vector3Int>();
        var boolBricks = new List<bool>();
        bricks._toggleGroup.ForEach(e => boolBricks.Add(e.isOn));
        boolBricks.Reverse();
        output.Add(GlobalConstants.BlockTypes[GlobalConstants.BlockTypes.Count-1]);
        for (int i = 1; i < boolBricks.Count; i++)
        {
            if (boolBricks[i])
            {
                output.Add(GlobalConstants.BlockTypes[i]);
            }
        }
        return output;
    }

    private List<Color> getColors()
    {
        if (colors._allToggle.isOn)
        {
            return GlobalConstants.LegoColors;
        }
        var output = new List<Color>();
        var boolColors = new List<bool>();
        colors._toggleGroup.ForEach(e => boolColors.Add(e.isOn));
        for (int i = 0; i < boolColors.Count; i++)
        {
            
            if (boolColors[i])
            {
                Debug.Log(i);
                output.Add(GlobalConstants.LegoColors[i]);
            }
        }
       
        return output;
    }

    private BrickItConfiguration getBrickItConfig()
    {
        var output = new BrickItConfiguration();
        output.brickExtends = getBricks();
        Debug.Log(output.brickExtends.Count);
        output.colors = getColors();
        Debug.Log(output.colors.Count);
        output.height = (int)Mathf.Round(scaling.value);
        output.depth = (int)Mathf.Round(depth.value);
        output.mesh = modelSelection.activeObject.GetComponentInChildren<MeshFilter>().mesh;
        output.transform = modelSelection.activeObject.transform;
        output.tex = (Texture2D) textureController.activeTexture;

        modelSelection.activeObject.SetActive(false);
        return output;
    }


    public void BrickIt()
    {
        conversionController.InstantiateBricks(getBrickItConfig());
    }

    public void StartAnimation()
    {
        conversionController.TriggerAnimation(getBrickItConfig());
    }
}
