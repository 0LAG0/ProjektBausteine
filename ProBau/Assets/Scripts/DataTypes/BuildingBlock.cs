using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Instanciatable Building Block.
/// </summary>
public class BuildingBlock
{
    public Vector3Int extends;

    public Vector3Int direction;

    public Color blockColor;

    private float volumeError;

    public bool isFlipped;

    public Vector3 pos;

    int blockid;

    public BuildingBlock(Vector3Int extends, Vector3Int direction, bool flipped, Vector3 pos , Color blockColor, int blockid)
    {
        this.blockid = blockid;
        this.extends = extends;
        this.direction = direction;
        isFlipped = flipped;
        this.pos = pos;
        this.blockColor = blockColor;
    }
}