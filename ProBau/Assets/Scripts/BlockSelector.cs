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
        possibleBlocks = possibleBlocks.OrderByDescending(b => b.extends.x * b.extends.y * b.extends.z * b.extends.z * b.extends.z).ToList();
        /*Debug.Log(possibleBlocks[0].extends);
        Debug.Log(possibleBlocks[1].extends);
        Debug.Log(possibleBlocks[2].extends);
        Debug.Log(possibleBlocks[3].extends);
        Debug.Log(possibleBlocks[4].extends);
        Debug.Log(possibleBlocks[5].extends);
        Debug.Log(possibleBlocks[6].extends);
        Debug.Log(possibleBlocks[7].extends);
        Debug.Log(possibleBlocks[8].extends);
        Debug.Log(possibleBlocks[9].extends);
        Debug.Log(possibleBlocks[10].extends);*/
    }

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
                    if (voxels[x, y, z])
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

    public List<BuildingBlock> calculateBlocksSpiral(bool[,,] voxels)
    {
        int width = voxels.GetLength(0);
        int depth = voxels.GetLength(2);
        int height = voxels.GetLength(1);

        List<BuildingBlock> returnList = new List<BuildingBlock>();
        for (int y = 0; y < height; y++)
        {
            int lastRow = width - 1;
            int lastCol = depth - 1;
            int i;
            int k = 0;
            int l = 0;
            //bool directionPreference = (y % 2 == 0);
            while (k <= lastRow && l <= lastCol)
            {
                for (i = l; i <= lastCol; i++)
                {
                    if (voxels[k, y, i])
                    {
                        bool directionPreference = (Random.Range(1,10) % 2 == 0);
                        returnList.Add(getLargestPossibleBlock(directionPreference, new Vector3Int(k, y, i), ref voxels));
                    }
                    //test for block [k][i]
                }
                k++;
                for (i = k; i <= lastRow; i++)
                {
                    if (voxels[i, y, lastCol])
                    {
                        bool directionPreference = (Random.Range(1, 10) % 2 == 0);
                        returnList.Add(getLargestPossibleBlock(directionPreference, new Vector3Int(i, y, lastCol), ref voxels));
                    }
                    //test for block [i][lastCol]
                }
                lastCol--;
                if (k <= lastRow)
                {
                    for (i = lastCol; i >= l; i--)
                    {
                        if (voxels[lastRow, y, i])
                        {
                            bool directionPreference = (Random.Range(1, 10) % 2 == 0);
                            returnList.Add(getLargestPossibleBlock(directionPreference, new Vector3Int(lastRow, y, i), ref voxels));
                        }
                        //test for block [lastRow][i]
                    }
                }
                lastRow--;
                if (l <= lastCol)
                {
                    for (i = lastRow; i >= k; i--)
                    {
                        if (voxels[i, y, l])
                        {
                            bool directionPreference = (Random.Range(1, 10) % 2 == 0);
                            returnList.Add(getLargestPossibleBlock(directionPreference, new Vector3Int(i, y, l), ref voxels));
                        }
                        //test for block [i][l]
                    }
                }
                l++;
            }
        }

        //Debug.Log(returnList.Count);
        //Debug.Log(returnList[1]);
        return returnList;
    }

    private bool checkForFit(bool flipped, BlockType checkType, ref bool[,,] voxels, Vector3Int pos)
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
        //Debug.Log("success !");
        //voxels = temp;
        return true;
    }

    private BuildingBlock getLargestPossibleBlock(bool directionPreference, Vector3Int pos, ref bool[,,] voxels)
    {
        for (int i = 0; i < possibleBlocks.Count; i++)
        {
            BlockType bt = possibleBlocks[i];
            Vector3 absPos = pos + ((Vector3)bt.extends / 2);
            //Vector3 absPos = pos + ((Vector3)(new Vector3(bt.extends.z, bt.extends.y, bt.extends.x)) / 2);
            //Debug.Log(checkForFit(directionPreference, bt, ref voxels, pos));
            if (checkForFit(!directionPreference, bt, ref voxels, pos))
            {
                if (!directionPreference)
                {
                    absPos = pos + ((Vector3)(new Vector3(bt.extends.z, bt.extends.y, bt.extends.x)) / 2);
                }
                return new BuildingBlock(bt, !directionPreference, absPos);
            }
            if (checkForFit(directionPreference, bt, ref voxels, pos))
            {
                if (directionPreference)
                {
                    absPos = pos + ((Vector3)(new Vector3(bt.extends.z, bt.extends.y, bt.extends.x)) / 2);
                }
                return new BuildingBlock(bt, directionPreference, absPos);
            }
        }
        return null;
    }
}