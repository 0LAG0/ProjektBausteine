using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class GlobalConstants
{
    public const string cubeContainerName = "cube container";

    public const float VoxelWidth = 1f;
    public const float VoxelHeight = 1.2f;

    public static readonly List<Vector3Int> BlockTypes;
    public static readonly List<Vector3Int> BlockDirections;
    public static readonly List<Color> LegoColors;

    static GlobalConstants()
    {
        BlockTypes = new List<Vector3Int>()
        {
            new Vector3Int(2,1,8),
            new Vector3Int(2,1,6),
            new Vector3Int(2,1,4),
            new Vector3Int(2,1,3),
            new Vector3Int(2,1,2),
            new Vector3Int(1,1,8),
            new Vector3Int(1,1,6),
            new Vector3Int(1,1,4),
            new Vector3Int(1,1,3),
            new Vector3Int(1,1,2),
            new Vector3Int(1,1,1)
        };

        BlockDirections = new List<Vector3Int>()
        {
            new Vector3Int(1,1,1),
            new Vector3Int(-1,1,1),
            new Vector3Int(1,1,-1),
            new Vector3Int(-1,1,-1)
        };

        LegoColors = new List<Color>()
        {
            new Color32(255,236,108,255),
            new Color32(250,200,10,255),
            new Color32(252,172,0,255),
            new Color32(214,121,35,255),
            new Color32(180,0,0,255),
            new Color32(114,0,18,255),
            new Color32(144,31,118,255),
            new Color32(211,53,157,255),
            new Color32(255,158,205,255),
            new Color32(205,164,222,255),
            new Color32(160,110,185,255),
            new Color32(68,26,145,255),
            new Color32(25,50,90,255),
            new Color32(30,90,168,255),
            new Color32(115,150,200,255),
            new Color32(157,195,247,255),
            new Color32(112,129,154,255),
            new Color32(104,195,226,255),
            new Color32(70,155,195,255),
            new Color32(211,242,234,255),
            new Color32(226,249,154,255),
            new Color32(165,202,24,255),
            new Color32(88,171,65,255),
            new Color32(0,133,43,255),
            new Color32(0,69,26,255),
            new Color32(62,60,57,255),
            new Color32(27,42,52,255),
            new Color32(100,100,100,255),
            new Color32(140,140,140,255),
            new Color32(150,150,150,255),
            new Color32(244,244,244,255),
            new Color32(112,142,124,255),
            new Color32(119,119,78,255),
            new Color32(137,125,98,255),
            new Color32(176,160,111,255),
            new Color32(255,201,149,255),
            new Color32(187,128,90,255),
            new Color32(170,125,85,255),
            new Color32(145,80,28,255),
            new Color32(55,33,0,255)
        };

    }

}
