using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// The actual Pipeline calling everything from voxelization to instacing the layouted bricks.
/// </summary>
public class ConversionController : MonoBehaviour
{
    public Mesh mesh;
    public Texture2D tex;
    /// <summary>
    /// Height in cm.
    /// </summary>
    public float targetHeight;

    private void Start()
    {
        BlockSelector selector = new BlockSelector(null);
        var buildingBlocks = selector.calculateBlocks(Voxelizer.Voxelize(mesh,tex, targetHeight));
        Debug.Log(buildingBlocks.Count);
        foreach (BuildingBlock bb in buildingBlocks)
        {
            Color color = Color.red;
            if (bb.blockType.extends.z == 2)
            {
                color = Color.blue;
            }
            if (bb.isFlipped)
            {
                VoxelTools.MakeCube(bb.pos, color, new Vector3(bb.blockType.extends.z, bb.blockType.extends.y, bb.blockType.extends.x));
            }
            else
            {
                VoxelTools.MakeCube(bb.pos, color, new Vector3(bb.blockType.extends.x, bb.blockType.extends.y, bb.blockType.extends.z));
            }

            //VoxelTools.MakeCube(bb.pos, VoxelTools.GetRandomColor(), bb.blockType.extends);
        }
    }
}

