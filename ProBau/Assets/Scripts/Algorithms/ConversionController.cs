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
    public bool isDebug = false;

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            VoxelTools.MakeAllCubesFall();
        }
    }

    private void Start()
    {
        if (isDebug)
        {
            BrickItConfiguration testCFG = getTestCfg();
            runBrickification(testCFG);
        }
    }

    public void runBrickification(BrickItConfiguration cfg)
    {
        BlockSelector selector = new BlockSelector(cfg.brickExtends);
        var startT = System.DateTime.Now;
        var tex = ColorCalculation.colorCalculate(cfg.tex, cfg.colors);
        var colorDone = System.DateTime.Now;
        var optimizedMesh = MeshUtils.OptimizeMesh(cfg.mesh, cfg.height);
        optimizedMesh.RecalculateNormals();
        var voxels = Voxelizer.Voxelize(cfg.mesh, tex, cfg.height);
        var voxelsInitDone = System.DateTime.Now;
        voxels = Voxelizer.AddWidth(voxels, cfg.depth);
        var widthAdded = System.DateTime.Now;
        var buildingBlocks = selector.calculateBlocksSpiralWithBounds(voxels);
        var blocksSelected = System.DateTime.Now;
        Debug.Log(buildingBlocks.Count);
        foreach (BuildingBlock bb in buildingBlocks)
        {
            //Wird hin und wieder null, sofern nicht alle steine selektiert sind
            Vector3 position = new Vector3(bb.pos.x, bb.pos.y * GlobalConstants.VoxelHeight, bb.pos.z);

            if (bb.isFlipped)
            {
                VoxelTools.MakeCube(position, bb.blockColor, new Vector3(bb.extends.z , bb.extends.y * GlobalConstants.VoxelHeight, bb.extends.x ));
            }
            else
            {
                VoxelTools.MakeCube(position, bb.blockColor, new Vector3(bb.extends.x , bb.extends.y * GlobalConstants.VoxelHeight, bb.extends.z ));
            }

            //VoxelTools.MakeCube(bb.pos, VoxelTools.GetRandomColor(), bb.blockType.extends);
        }
        var cobeContainer = GameObject.Find(GlobalConstants.cubeContainerName);
        cobeContainer.transform.position = cfg.posOfObject;
        cobeContainer.transform.parent = GameObject.Find("ModelsContainer").transform;
        var pos = cobeContainer.transform.position;
        float[] minMax = MeshUtils.GetBoundsPerDimension(optimizedMesh);
        cobeContainer.transform.position = new Vector3(pos.x - (minMax[1] - minMax[0]) / 2, pos.y - (minMax[3] - minMax[2]) / 2, pos.z - (minMax[5] - minMax[4]) / 2);
        var blocksInstaniated = System.DateTime.Now;
        Debug.Log($"Total Time needed: {blocksInstaniated-startT}");
        Debug.Log($"Time needed for Colors: {colorDone - startT}");
        Debug.Log($"Time needed for Voxel Init: {voxelsInitDone - colorDone}");
        Debug.Log($"Time needed for width add: {widthAdded - voxelsInitDone}");
        Debug.Log($"Time needed for block select: {blocksSelected - widthAdded}");
        Debug.Log($"Time needed for blocks instaniated: {blocksInstaniated - blocksSelected}");

    }

    private BrickItConfiguration getTestCfg()
    {
        var testCFG = new BrickItConfiguration();
        testCFG.height = height;
        testCFG.mesh = testMesh;
        testCFG.tex = testTex;
        testCFG.depth = 3;
        testCFG.colors = GlobalConstants.LegoColors;
        testCFG.brickExtends = GlobalConstants.BlockTypes;
        testCFG.posOfObject = new Vector3(0, 0, 0);
        return testCFG;
    }
}