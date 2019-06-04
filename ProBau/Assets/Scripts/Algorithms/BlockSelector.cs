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
    int voxelId = 1;

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

    public List<BuildingBlock> calculateBlocksSpiral(Voxel[,,] voxels)
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
                    if (voxels[k, y, i].id == 0)
                    {
                        bool flipPref = (Random.Range(1, 10) % 2 == 0);
                        returnList.Add(getLargestPossibleBlock(flipPref, GlobalConstants.BlockDirections[0], new Vector3Int(k, y, i), voxels));
                    }
                    //test for block [k][i]
                }
                k++;
                for (i = k; i <= lastRow; i++)
                {
                    if (voxels[i, y, lastCol].id == 0)
                    {
                        bool flipPref = (Random.Range(1, 10) % 2 == 0);
                        returnList.Add(getLargestPossibleBlock(flipPref, GlobalConstants.BlockDirections[2], new Vector3Int(i, y, lastCol), voxels));
                    }
                    //test for block [i][lastCol]
                }
                lastCol--;
                if (k <= lastRow)
                {
                    for (i = lastCol; i >= l; i--)
                    {
                        if (voxels[lastRow, y, i].id == 0)
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
                        if (voxels[i, y, l].id == 0)
                        {
                            bool flipPref = (Random.Range(1, 10) % 2 == 0);
                            returnList.Add(getLargestPossibleBlock(flipPref, GlobalConstants.BlockDirections[1], new Vector3Int(i, y, l), voxels));
                        }
                        //test for block [i][l]
                    }
                }
                l++;
            }
        }
        Debug.Log(voxelId);
        voxelId = 1;
        //Debug.Log(returnList.Count);
        //Debug.Log(returnList[1]);
        return returnList;
    }
lock> returnList = new List<BuildingBlock>();


        for (int y = 0; y < voxels.GetLength(1); y++)
        {
            bool directionPreference = (y % 2 == 0);
            for (int x = 0; x < voxels.GetLength(0); x++)
            {
                for (int z = 0; z < voxels.GetLength(2); z++)
                {
                    if (voxels[x, y, z])
                    {
                        Vector3Int direction = calculateBestDirection(voxels, x, y, z);
                        returnList.Add(getLargestPossibleBlock(directionPreference, direction, new Vector3Int(x, y, z), voxels));
                    }
                }
            }
        }

        return returnList;
    }*/

    public List<BuildingBlock> calculateBlocksSpiralWithBounds(Voxel[,,] voxels)
    {
        int width = voxels.GetLength(0);
        int depth = voxels.GetLength(2);
        int height = voxels.GetLength(1);

        List<BuildingBlock> returnList = new List<BuildingBlock>();
        for (int y = 0; y < height; y++)
        {
            //int maxX = width-1;
            //int maxZ = depth-1;
            //int i;
            //int x = 0;
            //int z = 0;

            int maxZ = 0;
            int maxX = 0;
            int i;
            int minZ = int.MaxValue;
            int minX = int.MaxValue;

            //min max y-level
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    if (voxels[x, y, z].id != null)
                    {
                        if (z < minZ) minZ = z;
                        if (x < minX) minX = x;
                        if (z > maxZ) maxZ = z;
                        if (x > maxX) maxX = x;
                    }
                }
            }

            //bool directionPreference = (y % 2 == 0);
            while (minZ <= maxZ && minX <= maxX)
            {
                for (i = minX; i <= maxX; i++)
                {
                    if (voxels[i, y, minZ].id != null)
                    {
                        bool flipPref = (Random.Range(1, 10) % 2 == 0);
                        returnList.Add(getLargestPossibleBlock(flipPref, GlobalConstants.BlockDirections[0], new Vector3Int(i, y, minZ), voxels));
                    }
                    //test for block [k][i]
                }
                minZ++;
                for (i = minZ; i <= maxZ; i++)
                {
                    if (voxels[maxX, y, i].id != null)
                    {
                        bool flipPref = (Random.Range(1, 10) % 2 == 0);
                        returnList.Add(getLargestPossibleBlock(flipPref, GlobalConstants.BlockDirections[1], new Vector3Int(maxX, y, i), voxels));
                    }
                    //test for block [i][lastCol]
                }
                maxX--;
                if (minZ <= maxZ)
                {
                    for (i = maxX; i >= minX; i--)
                    {
                        if (voxels[i, y, maxZ].id != null)
                        {
                            bool flipPref = (Random.Range(1, 10) % 2 == 0);
                            returnList.Add(getLargestPossibleBlock(flipPref, GlobalConstants.BlockDirections[3], new Vector3Int(i, y, maxZ), voxels));
                        }
                        //test for block [lastRow][i]
                    }
                }
                maxZ--;
                if (minX <= maxX)
                {
                    for (i = maxZ; i >= minZ; i--)
                    {
                        if (voxels[minX, y, i].id != null)
                        {
                            bool flipPref = (Random.Range(1, 10) % 2 == 0);
                            returnList.Add(getLargestPossibleBlock(flipPref, GlobalConstants.BlockDirections[2], new Vector3Int(minX, y, i), voxels));
                        }
                        //test for block [i][l]
                    }
                }
                minX++;
            }
        }

        //Debug.Log(returnList.Count);
        //Debug.Log(returnList[1]);
        return returnList;
    }





    private bool checkForFit(bool flipped, Vector3Int direction, Vector3Int checkType, Voxel[,,] voxels, Vector3Int pos)
    {
        // DIRECTION NEEDS TO BE HOOKED UP
        int xExtends = checkType.x;
        int zExtends = checkType.z;
        if (flipped)
        {
            xExtends = checkType.z;
            zExtends = checkType.x;
        }
        var startColor = voxels[pos.x , pos.y , pos.z].color;
        for (int y = 0; Mathf.Abs(y) < checkType.y; y += direction.y)
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
                        voxels[pos.x + x, pos.y + y, pos.z + z].id == null ||
                        startColor != voxels[pos.x + x, pos.y + y, pos.z + z].color)

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
                    voxels[pos.x + x, pos.y + y, pos.z + z].id = voxelId;
                }
            }
        }
        voxelId++;

        return true;
    }

    private BuildingBlock getLargestPossibleBlock(bool flipPref, Vector3Int direction, Vector3Int pos, Voxel[,,] voxels)
    {
        for (int i = 0; i < possibleExtends.Count; i++)
        {
            Vector3Int bt = possibleExtends[i];
            //Vector3 absPos = pos + ((Vector3)bt / 2);
            Vector3 absPos = pos + (new Vector3(bt.x * direction.x, bt.y, bt.z * direction.z)) / 2 + (Vector3)direction * -0.5f;
            Color color = voxels[pos.x, pos.y, pos.z].color;

            if (checkForFit(flipPref, direction, bt, voxels, pos))
            {
                int id = voxels[pos.x, pos.y, pos.z].id ?? default(int);
                if (flipPref)
                {
                    //absPos = pos + (new Vector3(bt.z, bt.y, bt.x) / 2);
                    absPos = pos + ((new Vector3(bt.z * direction.x, bt.y, bt.x * direction.z)) / 2) + (Vector3)direction * -0.5f;
                }
                return new BuildingBlock(bt, direction, flipPref, absPos, color);
            }
            if (checkForFit(!flipPref, direction, bt, voxels, pos))
            {
                int id = voxels[pos.x, pos.y, pos.z].id ?? default(int);
                if (!flipPref)
                {
                    //absPos = pos + (new Vector3(bt.z, bt.y, bt.x) / 2);
                    absPos = pos + ((new Vector3(bt.z * direction.x, bt.y, bt.x * direction.z)) / 2) + (Vector3)direction * -0.5f;
                }
                return new BuildingBlock(bt, direction, !flipPref, absPos, color);
            }
        }
        return null;
    }
}
