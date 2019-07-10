using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickAnimationController : MonoBehaviour
{

    //public GameObject[] steineListe;
    private GameObject[] buildingBlockObjects;

    //Prefabs als GameObjects
    public GameObject brick_1x1;
    public GameObject brick_1x2;
    public GameObject brick_1x3;
    public GameObject brick_1x4;
    public GameObject brick_1x6;
    public GameObject brick_1x8;
    public GameObject brick_2x2;
    public GameObject brick_2x3;
    public GameObject brick_2x4;
    public GameObject brick_2x6;
    public GameObject brick_2x8;

    private Dictionary<Vector3Int, GameObject> brickMap;
    private float startHeight;

    public float speed = 1;
    private int count = 0;

    private bool animationOn = false;
    const float layerHeight = 1.92f;
    int layer = 0;

    private Vector3[] startPos = null;
    private Vector3[] endPos = null;
    private float distance;
    private float currentLerpTime;
    private float startLerpTime;
    float lerpTime = 1.0f;

    private GameObject animationBlockContainer;

    AudioSource source;

    private void Awake()
    {
        brickMap = new Dictionary<Vector3Int, GameObject>();
        brickMap.Add(GlobalConstants.BlockTypes[0], brick_2x8);
        brickMap.Add(GlobalConstants.BlockTypes[1], brick_2x6);
        brickMap.Add(GlobalConstants.BlockTypes[2], brick_2x4);
        brickMap.Add(GlobalConstants.BlockTypes[3], brick_2x3);
        brickMap.Add(GlobalConstants.BlockTypes[4], brick_2x2);
        brickMap.Add(GlobalConstants.BlockTypes[5], brick_1x8);
        brickMap.Add(GlobalConstants.BlockTypes[6], brick_1x6);
        brickMap.Add(GlobalConstants.BlockTypes[7], brick_1x4);
        brickMap.Add(GlobalConstants.BlockTypes[8], brick_1x3);
        brickMap.Add(GlobalConstants.BlockTypes[9], brick_1x2);
        brickMap.Add(GlobalConstants.BlockTypes[10], brick_1x1);

        source = GetComponent<AudioSource>();
    }

    public void StartAnimation(List<BuildingBlock> inputBlocks, float[] minMax)
    {
        var localBlocks = inputBlocks;
        var AmmountOfBricks = inputBlocks.Count;
        localBlocks.Sort(SortByY);
        InstantiateBricks(localBlocks, minMax);
        distance = buildingBlockObjects[AmmountOfBricks - 1].transform.position.y;
        startPos = new Vector3[AmmountOfBricks];
        endPos = new Vector3[AmmountOfBricks];

        for (int i = 0; i < AmmountOfBricks; i++)
        {
            endPos[i] = buildingBlockObjects[i].transform.position;
            startPos[i] = endPos[i] + new Vector3(0, distance, 0);
        }
    }

    private int SortByY(BuildingBlock a, BuildingBlock b)
    {
        return a.pos.y.CompareTo(b.pos.y);
    }

    IEnumerator MoveToPosition(GameObject obj, Vector3 start, Vector3 end, int count)
    {
        float percentage = 0;
        startLerpTime = Time.time;
        while (percentage < 1.0f)
        {
            float currentLerpTime = Time.time - startLerpTime;
            // calculate finished percentage of lerping process
            percentage = currentLerpTime / lerpTime;
            obj.transform.localPosition = Vector3.Lerp(start, end, percentage);
            source.Play();
            yield return count;
        }

    }

    // used when animationOn = true aka when G is clicked
    // bricks shows up and move to end position one by one by adding delay time
    // delay time is diversed brick by brick
    IEnumerator WaitToDisplay(int index)
    {
        yield return new WaitForSeconds(lerpTime * index);      // delay time
        buildingBlockObjects[index].SetActive(true);
        StartCoroutine(MoveToPosition(buildingBlockObjects[index], startPos[index], endPos[index], index));
    }

    private void GetCurrentCount()
    {
        int countActive = 0;
        for (int i = 0; i < buildingBlockObjects.Length; i++)
        {
            if (buildingBlockObjects[i].activeInHierarchy)
            {
                countActive++;
            }
        }
        count = countActive - 1;
    }


    public void PlayAnimation()
    {
        Time.timeScale = 1;
        animationOn = true;
    }

    public void PauseAnimation()
    {
        Time.timeScale = 0;
        animationOn = false;
        GetCurrentCount();
    }

    public void AddBrick()
    {
        StopAllCoroutines();
        Time.timeScale = 1;
        animationOn = false;
        if (count <= buildingBlockObjects.Length - 1)
        {
            if (count == 0 && !buildingBlockObjects[0].activeInHierarchy)
            {
                buildingBlockObjects[count].SetActive(true);
                StartCoroutine(MoveToPosition(buildingBlockObjects[count], startPos[count], endPos[count], count));
            }

            else if (buildingBlockObjects[count].activeInHierarchy && buildingBlockObjects[count].transform.position != endPos[count])
            {
                buildingBlockObjects[count].SetActive(true);
                StartCoroutine(MoveToPosition(buildingBlockObjects[count], startPos[count], endPos[count], count));
            }

            else if (count < buildingBlockObjects.Length - 1)
            {
                count += 1;
                buildingBlockObjects[count].SetActive(true);
                StartCoroutine(MoveToPosition(buildingBlockObjects[count], startPos[count], endPos[count], count));

                // Debug.Log(count);
            }
        }
    }

    public void RemoveBrick()
    {
        if (count >= 0)
        {
            if (count == 0 && buildingBlockObjects[0].activeInHierarchy)
            {
                buildingBlockObjects[count].SetActive(false);
                //Debug.Log(count);
            }
            else if (count > 0)
            {
                buildingBlockObjects[count].SetActive(false);
                //Debug.Log(count);
                count -= 1;
            }
        }
    }

    public void ShowAll()
    {
        Reset();
        if (count <= buildingBlockObjects.Length - 1)
        {
            for (int i = count; i <= buildingBlockObjects.Length - 1; i++)
            {
                buildingBlockObjects[i].SetActive(true);
            }
            count = buildingBlockObjects.Length - 1;
            //Debug.Log(count);
        }
    }

    public void Reset()
    {
        if (count >= 0)
        {
            Time.timeScale = 1;
            for (int i = 0; i <= buildingBlockObjects.Length - 1; i++)
            {
                buildingBlockObjects[i].SetActive(false);
            }
            StopAllCoroutines();
            animationOn = false;
            count = 0;
        }
    }

    public void AddLayer()
    {
        if (count <= buildingBlockObjects.Length - 1)
        {
            int naechster = (int)Mathf.Round(buildingBlockObjects[count + 1].transform.position.y / layerHeight);
            layer = (int)Mathf.Round(buildingBlockObjects[count].transform.position.y / layerHeight);
            if (layer < naechster)
            {
                layer += 1;
            }
            for (int i = 0; i <= buildingBlockObjects.Length - 1; i++)
            {
                int level = (int)Mathf.Round(buildingBlockObjects[i].transform.position.y / layerHeight);
                if (layer == level)
                {
                    buildingBlockObjects[i].SetActive(true);
                    count = i;
                    StartCoroutine(MoveToPosition(buildingBlockObjects[count], startPos[count], endPos[count], count));
                }
            }
        }
    }

    public void RemoveLayer()
    {
        //Ebenenweise abziehen
        if (count >= 0)
        {
            layer = (int)Mathf.Round(buildingBlockObjects[count].transform.position.y / layerHeight);
            for (int i = buildingBlockObjects.Length - 1; i >= 0; i--)
            {
                int level = (int)Mathf.Round(buildingBlockObjects[i].transform.position.y / layerHeight);
                if (layer == level)
                {
                    buildingBlockObjects[i].SetActive(false);
                    if (count > 0)
                    {
                        count = i - 1;
                    }
                }
            }
        }
    }

    private void InstantiateBricks(List<BuildingBlock> blocksToInstantiate, float[] minMax)
    {
        if (animationBlockContainer!=null)
        {
            DestroyImmediate(animationBlockContainer);
        }
        animationBlockContainer = new GameObject("Animation Block Container");
        animationBlockContainer.transform.parent = this.transform;
        Quaternion rot = Quaternion.Euler(0, 0, 0);
        var pos = animationBlockContainer.transform.position;
        animationBlockContainer.transform.position = new Vector3(pos.x - (minMax[1] - minMax[0]) / 2, pos.y - (minMax[3] - minMax[2]) / 2, pos.z - (minMax[5] - minMax[4]) / 2);
        buildingBlockObjects = new GameObject[blocksToInstantiate.Count];
        for (int i = 0; i < blocksToInstantiate.Count; i++)
        {
            if (!blocksToInstantiate[i].isFlipped)
            {
                rot = Quaternion.Euler(0, 90f, 0);
            }
            else
            {
                rot = Quaternion.Euler(0, 0, 0);
            }
            float yPos = blocksToInstantiate[i].pos.y * layerHeight;
            float xPos = blocksToInstantiate[i].pos.x * 1.6f;
            float zPos = blocksToInstantiate[i].pos.z * 1.6f;
            Vector3 position = new Vector3(xPos, yPos, zPos);
            buildingBlockObjects[i] = Instantiate(brickMap[blocksToInstantiate[i].extends], position, rot, animationBlockContainer.transform);
            buildingBlockObjects[i].SetActive(false);

            MeshRenderer gameObjectRenderer = buildingBlockObjects[i].GetComponentInChildren<MeshRenderer>();
            Material newMaterial = new Material(Shader.Find("Diffuse"));
            newMaterial.color = blocksToInstantiate[i].blockColor;
            gameObjectRenderer.material = newMaterial;
        }
        
    }

    //sollte eigentlich in einen richtigen input handler, aber aufgrund von zeitmangel verschoben.
    void Update()
    {
        //Animation mit G starten und H anhalten
        if (Input.GetKey(KeyCode.H))
        {
            PauseAnimation();
            // Debug.Log(animationOn);
        }
        if (Input.GetKey(KeyCode.G))
        {
            PlayAnimation();
            //Debug.Log(animationOn);
        }

        if (animationOn == true)
        {
            if (count <= buildingBlockObjects.Length - 1)
            {
                StartCoroutine(WaitToDisplay(count));
                count += 1;
            }
        }

        //Hier kann man Stein für Stein anzeigen lassen.
        if (Input.GetKeyDown(KeyCode.S))
        {
            AddBrick();
        }

        //Stein für Stein wird abgezogen
        if (Input.GetKeyDown(KeyCode.D))
        {
            RemoveBrick();
        }


        //Hier kann man sich alle Steine anzeigen lassen
        if (Input.GetKeyDown(KeyCode.A))
        {
            ShowAll();
        }

        // Reset Funktion
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }

        //Modell Ebene für Ebene aufbauen
        if (Input.GetKeyDown(KeyCode.K))
        {
            AddLayer();
        }

        //Ebenenweise abziehen
        if (Input.GetKeyDown(KeyCode.L))
        {
            RemoveLayer();

        }
    }
}