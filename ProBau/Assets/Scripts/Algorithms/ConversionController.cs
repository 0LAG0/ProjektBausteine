using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// The actual Pipeline calling everything from voxelization, to instacing the layouted bricks.
/// </summary>
public class ConversionController : MonoBehaviour
{
    public Mesh mesh;
    public Texture2D tex;
    /// <summary>
    /// Height in cm.
    /// </summary>
    public float targetHeight;

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            VoxelTools.MakeAllCubesFall();
        }
    }

    private void Start()
    {
        BlockSelector selector = new BlockSelector(null);
        tex = ColorCalculation.colorCalculate(tex, GlobalConstants.LegoColors);
        var voxels = Voxelizer.Voxelize(mesh, tex, targetHeight);
        var buildingBlocks = selector.calculateBlocksSpiralWithBounds(voxels);
        ///Debug.Log(buildingBlocks.Count);
        foreach (BuildingBlock bb in buildingBlocks)
        {
            //bb.calcAdjacencies(voxels, buildingBlocks);
            Vector3 position = new Vector3(bb.pos.x, bb.pos.y, bb.pos.z);
            /*Color testColor = Color.green;
            if (bb.adjacencies.Count==0)
            {
                testColor = Color.magenta;
            }*/
            
            if (bb.isFlipped)
            {
                
                VoxelTools.MakeCube(position, bb.blockColor, new Vector3(bb.extends.z - 0.1f, bb.extends.y - 0.1f, bb.extends.x - 0.1f));
            }
            else
            {
                VoxelTools.MakeCube(position, bb.blockColor, new Vector3(bb.extends.x - 0.1f, bb.extends.y - 0.1f, bb.extends.z - 0.1f));
            }

            //VoxelTools.MakeCube(bb.pos, VoxelTools.GetRandomColor(), bb.blockType.extends);
        }
    }

    
    public void runBrickification(BrickItConfiguration cfg)
    {
        mesh = cfg.mesh;
        targetHeight = cfg.height;
        BlockSelector selector = new BlockSelector(cfg.brickExtends);
        tex = ColorCalculation.colorCalculate(tex, cfg.colors);
        var buildingBlocks = selector.calculateBlocksSpiralWithBounds(Voxelizer.Voxelize(mesh, tex, targetHeight));
        ///Debug.Log(buildingBlocks.Count);
        foreach (BuildingBlock bb in buildingBlocks)
        {

            if (bb.isFlipped)
            {
                Vector3 position = new Vector3(bb.pos.x, bb.pos.y, bb.pos.z);
                VoxelTools.MakeCube(position, bb.blockColor, new Vector3(bb.extends.z - 0.1f, bb.extends.y - 0.1f, bb.extends.x - 0.1f));
            }
            else
            {
                Vector3 position = new Vector3(bb.pos.x, bb.pos.y, bb.pos.z);
                VoxelTools.MakeCube(position, bb.blockColor, new Vector3(bb.extends.x - 0.1f, bb.extends.y - 0.1f, bb.extends.z - 0.1f));
            }

            //VoxelTools.MakeCube(bb.pos, VoxelTools.GetRandomColor(), bb.blockType.extends);
        }
        GameObject.Find(GlobalConstants.cubeContainerName).transform.position = cfg.posOfObject;
    }
}