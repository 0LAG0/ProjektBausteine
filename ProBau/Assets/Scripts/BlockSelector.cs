using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BlockSelector : MonoBehaviour
{
    private List<BlockType> possibleBlocks;

    public BlockSelector(List<BlockType> blockTypes)
    {
        possibleBlocks = blockTypes;

        if (possibleBlocks == null)
        {
            Debug.Log("starting resource find...");
            possibleBlocks = Resources.FindObjectsOfTypeAll(typeof(BlockType)).Cast<BlockType>().ToList();
            Debug.Log(possibleBlocks.Count);
        }
        possibleBlocks.OrderByDescending(b => b.extends.x * b.extends.y * b.extends.z);
    }

    private void Awake()
    {
        if (possibleBlocks == null)
        {
            Debug.Log("starting resource find...");
            possibleBlocks = Resources.FindObjectsOfTypeAll(typeof(BlockType)).Cast<BlockType>().ToList();
            Debug.Log(possibleBlocks.Count);
        }
        possibleBlocks.OrderByDescending(b => b.extends.x * b.extends.y * b.extends.z);
    }

    public BuildingBlock[,,] calculateBlocks(bool[,,] voxels)
    {
        BuildingBlock[,,] returnArray = new BuildingBlock[voxels.GetLength(0), voxels.GetLength(1), voxels.GetLength(2)];
        for (int y = 0; y < voxels.GetLength(1); y++)
        {
            bool directionPreference = (y % 2 == 0);
            for (int x = 0; x < voxels.GetLength(0); x++)
            {
                for (int z = 0; z < voxels.GetLength(2); z++)
                {
                    getLargestPossibleBlock(directionPreference, new Vector3Int(x, y, z), ref voxels);
                }
            }
        }
        return returnArray;
    }

    private bool checkForFit(bool flipped, BlockType checkType,ref bool[,,] voxels, Vector3Int pos)
    {
        int xExtends = checkType.extends.x;
        int zExtends = checkType.extends.z;
        if (flipped)
        {
            xExtends = checkType.extends.z;
            zExtends = checkType.extends.x;
        }

        Debug.Log(xExtends);
        Debug.Log(zExtends);
        var temp = voxels;

        for (int y = 0; y < checkType.extends.y; y++)
        {
            for (int x = 0; x < xExtends; x++)
            {
                for (int z = 0; z < zExtends; z++)
                {
                    temp[pos.x + x, pos.y + y, pos.z + z] = false;
                    if (!voxels[pos.x + x, pos.y + y, pos.z + z])
                    {
                        return false;
                    }
                }
            }
        }
        voxels = temp;
        return true;
    }

    private BuildingBlock getLargestPossibleBlock(bool directionPreference, Vector3Int pos,ref bool[,,] voxels)
    {
        BuildingBlock block;
        for (int i = 0; i < possibleBlocks.Count; i++)
        {
            BlockType bt = possibleBlocks[i];
            Vector3 absPos = pos + ((Vector3)bt.extends / 2);
            if (checkForFit(directionPreference, bt,ref voxels, pos))
            {
                return new BuildingBlock(bt, directionPreference, absPos);
            }
            if (checkForFit(!directionPreference, bt,ref voxels, pos))
            {
                return new BuildingBlock(bt, !directionPreference, absPos);
            }
        }
        Debug.Log("returns null...");
        return null;
    }
}
