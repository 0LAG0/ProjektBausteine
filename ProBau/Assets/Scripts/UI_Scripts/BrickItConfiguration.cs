using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BrickItConfiguration
{
    public List<Vector3Int> brickExtends;
    public List<Color> colors;
    public bool filled;
    public int height;
    public Mesh mesh;
    public Texture2D tex;
    public Vector3 posOfObject;
}
