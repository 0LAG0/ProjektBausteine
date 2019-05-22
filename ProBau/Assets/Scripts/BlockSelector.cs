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
            possibleBlocks = Resources.FindObjectsOfTypeAll(typeof(BlockType)).Cast<BlockType>().ToList();
        }
        possibleBlocks.OrderByDescending(b => b.extends.x * b.extends.y * b.extends.z);
        //Debug.Log(possibleBlocks[0].extends);
        //Debug.Log(possibleBlocks[1].extends);
        //Debug.Log(possibleBlocks[2].extends);
    }

    /*private void Awake()
    {
        if (possibleBlocks == null)
        {
            possibleBlocks = Resources.FindObjectsOfTypeAll(typeof(BlockType)).Cast<BlockType>().ToList();
        }
        possibleBlocks.OrderByDescending(b => b.extends.x * b.extends.y * b.extends.z);
    }*/

    /*public BuildingBlock[,,] calculateBlocks(bool[,,] voxels)
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
    }*/

    public List<BuildingBlock> calculateBlocks(bool[,,] voxels)
    {
        List<BuildingBlock> returnList = new List<BuildingBlock>();
        for (int y = 0; y < voxels.GetLength(1); y++)
        {
            bool directionPreference = (y % 2 == 0);
            for (int x = 0; x < voxels.GetLength(0); x++)
            {
                for (int z = 0; z < voxels.GetLength(2); z++)
                {
                    if (voxels[x,y,z])
                    {
                        returnList.Add(getLargestPossibleBlock(directionPreference, new Vector3Int(x, y, z), ref voxels));
                    }                    
                }
            }
        }
        //Debug.Log(returnList.Count);
        //Debug.Log(returnList[1]);
        return returnList;
    }

    private bool checkForFit(bool flipped, BlockType checkType,ref bool[,,] voxels, Vector3Int pos)
    {
        //Debug.Log(checkType.extends);
        int xExtends = checkType.extends.x;
        int zExtends = checkType.extends.z;
        if (flipped)
        {
            xExtends = checkType.extends.z;
            zExtends = checkType.extends.x;
        }
        
        //var temp = voxels;

        for (int y = 0; y < checkType.extends.y; y++)
        {
            for (int x = 0; x < xExtends; x++)
            {
                for (int z = 0; z < zExtends; z++)
                {
                    //temp[pos.x + x, pos.y + y, pos.z + z] = false;
                    if (pos.x + x >= voxels.GetLength(0) || pos.z + z >= voxels.GetLength(2) || pos.y + y >= voxels.GetLength(1) || !voxels[pos.x + x, pos.y + y, pos.z + z])
                    {
                        return false;
                    }
                }
            }
        }

        for (int y = 0; y < checkType.extends.y; y++)
        {
            for (int x = 0; x < xExtends; x++)
            {
                for (int z = 0; z < zExtends; z++)
                {
                    voxels[pos.x + x, pos.y + y, pos.z + z] = false;
                }
            }
        }
        Debug.Log("success !");
        //voxels = temp;
        return true;
    }

    private BuildingBlock getLargestPossibleBlock(bool directionPreference, Vector3Int pos,ref bool[,,] voxels)
    {
        for (int i = 0; i < possibleBlocks.Count; i++)
        {
            BlockType bt = possibleBlocks[i];
            Vector3 absPos = pos + ((Vector3)bt.extends / 2);
            //Debug.Log(checkForFit(directionPreference, bt, ref voxels, pos));
            if (checkForFit(directionPreference, bt,ref voxels, pos))
            {
                return new BuildingBlock(bt, directionPreference, absPos);
            }
            if (checkForFit(!directionPreference, bt,ref voxels, pos))
            {
                return new BuildingBlock(bt, !directionPreference, absPos);
            }
        }
        return null;
    }
}
