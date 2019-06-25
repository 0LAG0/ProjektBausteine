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
    public int height;
    public Mesh testMesh;
    public Texture2D testTex;

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            VoxelTools.MakeAllCubesFall();
        }
    }

    private void Start()
    {
        BrickItConfiguration testCFG = getTestCfg();
        runBrickification(testCFG);
        /*
        BlockSelector selector = new BlockSelector(null);
        var tex = ColorCalculation.colorCalculate(testCFG.tex, GlobalConstants.LegoColors);
        var voxels = Voxelizer.Voxelize(testCFG.mesh, testCFG.tex, testCFG.height, 0);
        //voxels = Voxelizer.AddWidth(voxels, 2);
        var buildingBlocks = selector.calculateBlocksSpiralWithBounds(voxels);
        ///Debug.Log(buildingBlocks.Count);
        foreach (BuildingBlock bb in buildingBlocks)
        {
            //bb.calcAdjacencies(voxels, buildingBlocks);
            Vector3 position = new Vector3(bb.pos.x, bb.pos.y * GlobalConstants.VoxelHeight, bb.pos.z);
            Color testColor = Color.green;
            if (bb.adjacencies.Count==0)
            {
                testColor = Color.magenta;
            }

            if (bb.isFlipped)
            {
                VoxelTools.MakeCube(position, bb.blockColor, new Vector3(bb.extends.z - 0.1f, bb.extends.y * GlobalConstants.VoxelHeight - 0.1f, bb.extends.x - 0.1f));
            }
            else
            {
                VoxelTools.MakeCube(position, bb.blockColor, new Vector3(bb.extends.x - 0.1f, bb.extends.y - 0.1f, bb.extends.z - 0.1f));
            }

            //VoxelTools.MakeCube(bb.pos, VoxelTools.GetRandomColor(), bb.blockType.extends);
        }
        */
    }

    public void runBrickification(BrickItConfiguration cfg)
    {
        BlockSelector selector = new BlockSelector(cfg.brickExtends);
        var tex = ColorCalculation.colorCalculate(cfg.tex, cfg.colors);
        var voxels = Voxelizer.Voxelize(cfg.mesh, tex, cfg.height, 0);
        voxels = Voxelizer.AddWidth(voxels, 3);
        var buildingBlocks = selector.calculateBlocksSpiralWithBounds(voxels);
        ///Debug.Log(buildingBlocks.Count);
        foreach (BuildingBlock bb in buildingBlocks)
        {
            Vector3 position = new Vector3(bb.pos.x, bb.pos.y * GlobalConstants.VoxelHeight, bb.pos.z);

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
        GameObject.Find(GlobalConstants.cubeContainerName).transform.position = cfg.posOfObject;
    }

    private BrickItConfiguration getTestCfg()
    {
        var testCFG = new BrickItConfiguration();
        testCFG.height = height;
        testCFG.mesh = testMesh;
        testCFG.tex = testTex;
        testCFG.colors = GlobalConstants.LegoColors;
        testCFG.brickExtends = GlobalConstants.BlockTypes;
        testCFG.posOfObject = new Vector3(0, 0, 0);
        testCFG.filled = false;
        return testCFG;
    }
}