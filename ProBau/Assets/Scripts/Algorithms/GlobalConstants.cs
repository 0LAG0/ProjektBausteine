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
            new Color(255,236,108),
            new Color(250,200,10),
            new Color(252,172,0),
            new Color(214,121,35),
            new Color(180,0,0),
            new Color(114,0,18),
            new Color(144,31,118),
            new Color(211,53,157),
            new Color(255,158,205),
            new Color(205,164,222),
            new Color(160,110,185),
            new Color(68,26,145),
            new Color(25,50,90),
            new Color(30,90,168),
            new Color(115,150,200),
            new Color(157,195,247),
            new Color(112,129,154),
            new Color(104,195,226),
            new Color(70,155,195),
            new Color(211,242,234),
            new Color(226,249,154),
            new Color(165,202,24),
            new Color(88,171,65),
            new Color(0,133,43),
            new Color(0,69,26),
            new Color(62,60,57),
            new Color(27,42,52),
            new Color(100,100,100),
            new Color(140,140,140),
            new Color(150,150,150),
            new Color(244,244,244),
            new Color(112,142,124),
            new Color(119,119,78),
            new Color(137,125,98),
            new Color(176,160,111),
            new Color(255,201,149),
            new Color(187,128,90),
            new Color(170,125,85),
            new Color(145,80,28),
            new Color(55,33,0)
        };

    }

}
