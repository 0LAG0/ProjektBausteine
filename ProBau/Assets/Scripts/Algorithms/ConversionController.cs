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
    public BrickAnimationController animationController;
    private GameObject lastBricked;

    //using class as a buffer which undermines readable controllflow
    //but urgency forces me...
    private float[] lastExtends;
    /*
    public int height;
    public Mesh testMesh;
    public Texture2D testTex;
    public bool isDebug = false;

    private void Start()
    {
        if (isDebug)
        {
            BrickItConfiguration testCFG = GetTestCfg();
            runBrickification(testCFG);
        }
    }*/

    public List<BuildingBlock> GetBuildingBlocks(BrickItConfiguration cfg)
    {
        var oldMesh = cfg.mesh.vertices;
        if (lastBricked!=null)
        {
            DestroyImmediate(lastBricked);
            lastBricked = null;
        }
        BlockSelector selector = new BlockSelector(cfg.brickExtends);
        var tex = ColorCalculation.colorCalculate(cfg.tex, cfg.colors);
        var voxels = Voxelizer.Voxelize(cfg.mesh, tex, cfg.height);
        voxels = Voxelizer.AddWidth(voxels, cfg.depth);
        lastExtends = MeshUtils.GetBoundsPerDimension(cfg.mesh);
        cfg.mesh.vertices = oldMesh;
        cfg.mesh.RecalculateBounds();
        return selector.calculateBlocksSpiralWithBounds(voxels);
    }

    public void InstantiateBricks(BrickItConfiguration cfg)
    {
        var buildingBlocks = GetBuildingBlocks(cfg);
        foreach (BuildingBlock bb in buildingBlocks)
        {
            Vector3 position = new Vector3(bb.pos.x, bb.pos.y * GlobalConstants.VoxelHeight, bb.pos.z);

            if (bb.isFlipped)
            {
                VoxelTools.MakeCube(position, bb.blockColor, new Vector3(bb.extends.z, bb.extends.y * GlobalConstants.VoxelHeight, bb.extends.x));
            }
            else
            {
                VoxelTools.MakeCube(position, bb.blockColor, new Vector3(bb.extends.x, bb.extends.y * GlobalConstants.VoxelHeight, bb.extends.z));
            }
        }
        var cubeContainer = GameObject.Find(GlobalConstants.cubeContainerName);
        cubeContainer.transform.position = cfg.transform.position;
        cubeContainer.transform.rotation = cfg.transform.rotation;
        cubeContainer.transform.parent = GameObject.Find("ModelsContainer").transform;
        var boundsHandler = cubeContainer.AddComponent<BoundsHandler>();
        cubeContainer.tag = GlobalConstants.containerTag;
        cubeContainer.layer = 9;
        var pos = cubeContainer.transform.position;
        boundsHandler.Bounds = new Vector3(lastExtends[1] - lastExtends[0], lastExtends[3] - lastExtends[2], lastExtends[5] - lastExtends[4]);
        cubeContainer.transform.position = new Vector3(pos.x - boundsHandler.Bounds.x / 2, pos.y - boundsHandler.Bounds.y / 2, pos.z - boundsHandler.Bounds.z / 2);
        lastBricked = cubeContainer;
    }

    public void TriggerAnimation(BrickItConfiguration cfg)
    {
        animationController.StartAnimation(GetBuildingBlocks(cfg), lastExtends);
    }

    /*
    private BrickItConfiguration GetTestCfg()
    {
        var testCFG = new BrickItConfiguration();
        testCFG.height = height;
        testCFG.mesh = testMesh;
        testCFG.tex = testTex;
        testCFG.depth = 3;
        testCFG.colors = GlobalConstants.LegoColors;
        testCFG.brickExtends = GlobalConstants.BlockTypes;
        testCFG.transform = new GameObject().transform;
        return testCFG;
    }
    */
}