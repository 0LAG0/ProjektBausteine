using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BuildingBlock
{
    public BlockType blockType;

    private Color blockColor;

    private float volumeError;

    bool isFlipped;

    public Vector3 pos;

    public BuildingBlock(BlockType bt, bool flipped, Vector3 pos)
    {
        blockType = bt;
        isFlipped = flipped;
        this.pos = pos;
    }
}