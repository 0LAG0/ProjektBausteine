using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class performing the combination of voxels to larger blocks.
/// </summary>
public class BlockSelector
{
    private List<Vector3Int> possibleExtends;

    public BlockSelector(List<Vector3Int> inputExtends)
    {
        possibleExtends = inputExtends;

        if (possibleExtends == null)
        {
            possibleExtends = GlobalConstants.BlockTypes;
            //TODO use preddefined set of blocks, or catch in UI
        }
        //possibleExtends = possibleExtends.OrderByDescending(b => b.x * b.y * b.z * b.z * b.z).ToList();
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
                        //returnList.Add(getLargestPossibleBlock(directionPreference, new Vector3Int(x, y, z), ref voxels));
                    }
                }
            }
        }
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
                        bool flipPref = (Random.Range(1,10) % 2 == 0);
                        returnList.Add(getLargestPossibleBlock(flipPref,GlobalConstants.BlockDirections[0], new Vector3Int(k, y, i), voxels));
                    }
                    //test for block [k][i]
                }
                k++;
                for (i = k; i <= lastRow; i++)
                {
                    if (voxels[i, y, lastCol])
                    {
                        bool flipPref = (Random.Range(1, 10) % 2 == 0);
                        returnList.Add(getLargestPossibleBlock(flipPref, GlobalConstants.BlockDirections[1], new Vector3Int(i, y, lastCol), voxels));
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
                            bool flipPref = (Random.Range(1, 10) % 2 == 0);
                            returnList.Add(getLargestPossibleBlock(flipPref, GlobalConstants.BlockDirections[3], new Vector3Int(lastRow, y, i), voxels));
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
                            bool flipPref = (Random.Range(1, 10) % 2 == 0);
                            returnList.Add(getLargestPossibleBlock(flipPref, GlobalConstants.BlockDirections[2], new Vector3Int(i, y, l), voxels));
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

    private bool checkForFit(bool flipped, Vector3Int direction, Vector3Int checkType, bool[,,] voxels, Vector3Int pos)
    {
        // DIRECTION NEEDS TO BE HOOKED UP
        int xExtends = checkType.x;
        int zExtends = checkType.z;
        if (flipped)
        {
            xExtends = checkType.z;
            zExtends = checkType.x;
        }

        for (int y = 0; Mathf.Abs(y) < checkType.y; y+=direction.y)
        {
            for (int x = 0; Mathf.Abs(x) < xExtends; x += direction.x)
            {
                for (int z = 0; Mathf.Abs(z) < zExtends; z += direction.z)
                {
                    //temp[pos.x + x, pos.y + y, pos.z + z] = false;
                    if (pos.x + x >= voxels.GetLength(0) ||
                        pos.z + z >= voxels.GetLength(2) ||
                        pos.y + y >= voxels.GetLength(1) ||
                        pos.x + x < 0 ||
                        pos.z + z < 0 ||
                        pos.y + y < 0 ||
                        !voxels[pos.x + x, pos.y + y, pos.z + z])
                    {
                        return false;
                    }
                }
            }
        }

        for (int y = 0; Mathf.Abs(y) < checkType.y; y += direction.y)
        {
            for (int x = 0; Mathf.Abs(x) < xExtends; x += direction.x)
            {
                for (int z = 0; Mathf.Abs(z) < zExtends; z += direction.z)
                {
                    voxels[pos.x + x, pos.y + y, pos.z + z] = false;
                }
            }
        }

        return true;
    }

    private BuildingBlock getLargestPossibleBlock(bool flipPref, Vector3Int direction, Vector3Int pos, bool[,,] voxels)
    {
        for (int i = 0; i < possibleExtends.Count; i++)
        {
            Vector3Int bt = possibleExtends[i];
            Vector3 absPos = pos + ((Vector3)bt / 2);

            if (checkForFit(!flipPref, direction, bt, voxels, pos))
            {
                if (!flipPref)
                {
                    absPos = pos + ((Vector3)(new Vector3(bt.z, bt.y, bt.x)) / 2);
                }
                return new BuildingBlock(bt, direction,!flipPref, absPos, Color.blue);
            }
            if (checkForFit(flipPref, direction, bt, voxels, pos))
            {
                if (flipPref)
                {
                    absPos = pos + ((Vector3)(new Vector3(bt.z, bt.y, bt.x)) / 2);
                }
                return new BuildingBlock(bt, direction, flipPref, absPos,Color.red);
            }
        }
        return null;
    }
}