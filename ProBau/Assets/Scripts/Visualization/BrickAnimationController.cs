using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickAnimationController : MonoBehaviour
{

    //public GameObject[] steineListe;
    private GameObject[] buildingBlockObjects;
    private List<BuildingBlock> localBlocks;

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
    private int LastIndexSet = -1;

    private bool animationOn = false;
    const float layerHeight = 1.92f;
    const float layerWidth = 1.6f;
    int layer = 0;

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
        localBlocks = inputBlocks;
        var AmmountOfBricks = inputBlocks.Count;
        localBlocks.Sort(SortByDimensions);
        InstantiateBricks(localBlocks, minMax);
        distance = buildingBlockObjects[AmmountOfBricks - 1].transform.position.y;
    }

    private int SortByDimensions(BuildingBlock a, BuildingBlock b)
    {
        var yComp = a.pos.y.CompareTo(b.pos.y);
        if (yComp == 0)
        {
            var xComp = a.pos.x.CompareTo(b.pos.x);
            if (xComp == 0)
            {
                var zComp = a.pos.z.CompareTo(b.pos.z);
                return zComp;
            }
            return xComp;
        }
        return yComp;
    }

    // used when animationOn = true aka when G is clicked
    // bricks shows up and move to end position one by one by adding delay time
    // delay time is diversed brick by brick
    IEnumerator DisplayDelayed(int index)
    {
        yield return new WaitForSeconds(lerpTime);      // delay time
        MoveBrick(index);
    }

    private void SetDefaultState()
    {
        Time.timeScale = 1;
        animationOn = false;
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
    }

    public void AddBrick()
    {
        if (LastIndexSet + 1 < buildingBlockObjects.Length)
        {
            LastIndexSet++;
            MoveBrick(LastIndexSet);
        }
    }

    public void MoveBrick(int brickToSet)
    {
        if (brickToSet < buildingBlockObjects.Length && brickToSet >= 0)
        {
            buildingBlockObjects[brickToSet].SetActive(true);
            var movementScript = buildingBlockObjects[brickToSet].AddComponent<SimpleMovement>();
            var pos = localBlocks[brickToSet].pos;
            Vector3 position = new Vector3(pos.x*layerWidth, pos.y*layerHeight, pos.z*layerWidth);
            movementScript.MoveObjectFromTo(position + new Vector3(0, distance, 0), position, lerpTime, source);
        }
    }

    public void RemoveBrick()
    {
        if (LastIndexSet >= 0)
        {
            buildingBlockObjects[LastIndexSet].SetActive(false);
            LastIndexSet--;
        }
    }

    public void AddLayer()
    {
        if (LastIndexSet + 1 < buildingBlockObjects.Length)
        {
            var nextY = buildingBlockObjects[LastIndexSet+1].transform.position.y;
            var lookahead = 1;
            var foundNextLayer = false;
            while (LastIndexSet + lookahead + 1 < buildingBlockObjects.Length && !foundNextLayer)
            {
                var lookaheadY = buildingBlockObjects[LastIndexSet + lookahead + 1].transform.position.y;
                foundNextLayer = nextY != lookaheadY;
                if (!foundNextLayer)
                {
                    lookahead++;
                }
                //LastIndexSet+lookahead= last stone on Layer;
            }
            for (int i = LastIndexSet + 1; i <= LastIndexSet + lookahead; i++)
            {
                MoveBrick(i);
            }
            LastIndexSet += lookahead;
        }
    }

    public void RemoveLayer()
    {
        //Ebenenweise abziehen
        if (LastIndexSet >= 0)
        {
            var LastY = buildingBlockObjects[LastIndexSet].transform.position.y;
            var lookback = 1;
            var foundPreviousLayer = false;
            while (LastIndexSet - lookback >= 0 && !foundPreviousLayer)
            {
                var lookbackY = buildingBlockObjects[LastIndexSet - lookback].transform.position.y;
                foundPreviousLayer = LastY != lookbackY;
                if (!foundPreviousLayer)
                {
                    lookback++;
                }
                //LastIndexSet-lookback= last stone on Layer;
            }
            for (int i = LastIndexSet; i > LastIndexSet - lookback; i--)
            {
                buildingBlockObjects[i].SetActive(false);
            }
            LastIndexSet -= lookback;
        }
    }
    public void ShowAll()
    {
        Reset();
        if (LastIndexSet <= buildingBlockObjects.Length - 1 && LastIndexSet>=0)
        {
            for (int i = LastIndexSet; i <= buildingBlockObjects.Length - 1; i++)
            {
                buildingBlockObjects[i].SetActive(true);
            }
            LastIndexSet = buildingBlockObjects.Length - 1;
            //Debug.Log(count);
        }
    }

    public void Reset()
    {
        if (LastIndexSet >= 0)
        {
            Time.timeScale = 1;
            for (int i = 0; i < buildingBlockObjects.Length; i++)
            {
                buildingBlockObjects[i].SetActive(false);
            }
            StopAllCoroutines();
            animationOn = false;
            LastIndexSet = 0;
        }
    }



    private void InstantiateBricks(List<BuildingBlock> blocksToInstantiate, float[] minMax)
    {
        if (animationBlockContainer != null)
        {
            DestroyImmediate(animationBlockContainer);
        }
        animationBlockContainer = new GameObject("Animation Block Container");
        animationBlockContainer.transform.parent = this.transform;
        Quaternion rot = Quaternion.Euler(0, 0, 0);
        var pos = animationBlockContainer.transform.position;
        //Pivot durch parent object centern
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
            float xPos = blocksToInstantiate[i].pos.x * layerWidth;
            float zPos = blocksToInstantiate[i].pos.z * layerWidth;
            Vector3 position = new Vector3(xPos, yPos, zPos);
            buildingBlockObjects[i] = Instantiate(brickMap[blocksToInstantiate[i].extends], position, rot, animationBlockContainer.transform);
            buildingBlockObjects[i].SetActive(false);

            MeshRenderer gameObjectRenderer = buildingBlockObjects[i].GetComponentInChildren<MeshRenderer>();
            Material newMaterial = new Material(Shader.Find("Diffuse"));
            newMaterial.color = blocksToInstantiate[i].blockColor;
            gameObjectRenderer.material = newMaterial;
        }
        SetLayerRecursively(animationBlockContainer, 10);
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
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
            if (LastIndexSet <= buildingBlockObjects.Length - 1)
            {
                StartCoroutine(DisplayDelayed(LastIndexSet));
                LastIndexSet += 1;
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