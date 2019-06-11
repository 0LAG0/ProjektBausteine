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

    Vector3Int initPos;

    public int blockid;

    public HashSet<int> adjacencies = new HashSet<int>();

    public BuildingBlock(Vector3Int extends, Vector3Int direction, bool flipped, Color blockColor, int blockid, Vector3Int initPos)
    {
        this.initPos = initPos;
        this.blockid = blockid;
        this.extends = extends;
        this.direction = direction;
        isFlipped = flipped;
        this.blockColor = blockColor;
        this.pos = initPos + (new Vector3(extends.x * direction.x, extends.y * direction.y, extends.z * direction.z)) / 2 + (Vector3)direction * -0.5f;
        if (isFlipped)
        {
            this.pos = initPos + ((new Vector3(extends.z * direction.x, extends.y * direction.y, extends.x * direction.z)) / 2) + (Vector3)direction * -0.5f;
        }
    }

    public void calcAdjacencies(Voxel[,,] voxels, List<BuildingBlock> blocks)
    {
        int xExtends = extends.x;
        int zExtends = extends.z;
        if (isFlipped)
        {
            xExtends = extends.z;
            zExtends = extends.x;
        }

        for (int x = 0; Mathf.Abs(x) < xExtends; x += direction.x)
        {
            for (int z = 0; Mathf.Abs(z) < zExtends; z += direction.z)
            {
                if (initPos.y != 0)
                {
                    int neigborId = voxels[initPos.x + x, initPos.y - 1, initPos.z + z].id ?? default(int);
                    if (neigborId != 0)
                    {
                        adjacencies.Add(neigborId);
                    }
                }

                if (initPos.y < voxels.GetLength(1)-1)
                {
                    int neigborId = voxels[initPos.x + x, initPos.y + 1, initPos.z + z].id ?? default(int);
                    if (neigborId != 0)
                    {
                        adjacencies.Add(neigborId);
                    }
                }
            }
        }
    }
}