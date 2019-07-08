﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestArrayAnimation : MonoBehaviour
{
    //better not touch this -.-
    public Mesh mesh;
    public Texture2D tex;
    public float targetHeight;
    //better not touch this -.-


    //public GameObject[] steineListe;
    private GameObject[] steine;
    private List<BuildingBlock> buildingBlocks;

    private float[] positionY;

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

    //Vektoren für extends-Werte
    Vector3 stein_1x1 = new Vector3(1, 1, 1);
    Vector3 stein_1x2 = new Vector3(1, 1, 2);
    Vector3 stein_1x3 = new Vector3(1, 1, 3);
    Vector3 stein_1x4 = new Vector3(1, 1, 4);
    Vector3 stein_1x6 = new Vector3(1, 1, 6);
    Vector3 stein_1x8 = new Vector3(1, 1, 8);
    Vector3 stein_2x2 = new Vector3(2, 1, 2);
    Vector3 stein_2x3 = new Vector3(2, 1, 3);
    Vector3 stein_2x4 = new Vector3(2, 1, 4);
    Vector3 stein_2x6 = new Vector3(2, 1, 6);
    Vector3 stein_2x8 = new Vector3(2, 1, 8);

    private float startHeight;

    public float speed = 1;
    private int count = 0;
    private int brickSize;

    private bool animationOn = false;
    float layerHeight = 1.92f;
    int layer = 0;

    private Vector3[] startPos = null;
    private Vector3[] endPos = null;
    private float distance;
    private float currentLerpTime;
    private float startLerpTime;
    float lerpTime = 1.0f;


    AudioSource source;


    void Start()
    {
        //
        // use this to animate bricks
        // for now you only need pos/extends/isFlipped
        // pos: the position of the brick (center)
        // extends: the blocktype (a 2x4 block would be (2,1,4))
        // isFlipped: the brick is rotated by 90° (pos still stays the same)
        BlockSelector selector = new BlockSelector(null);
        buildingBlocks = selector.calculateBlocksSpiralWithBounds(Voxelizer.Voxelize(mesh, tex, targetHeight));
        //
        //
        Debug.Log(buildingBlocks.Count);
        brickSize = buildingBlocks.Count;

        buildingBlocks.Sort(SortByY);
        instantiateBricks();

        source = GetComponent<AudioSource>();

        distance = steine[brickSize - 1].transform.position.y;
        startPos = new Vector3[brickSize];
        endPos = new Vector3[brickSize];

        for (int i = 0; i < brickSize; i++)
        {
            endPos[i] = steine[i].transform.position;
            startPos[i] = endPos[i] + new Vector3(0, distance, 0);
        }
    }

    private int SortByY(BuildingBlock a, BuildingBlock b)
    {
        return a.pos.y.CompareTo(b.pos.y);
    }

    IEnumerator MoveToPosition (GameObject obj, Vector3 start, Vector3 end)
    {
        float perc = 0;
        startLerpTime = Time.time;
        while (perc < 1.0f) {
            float currentLerpTime = Time.time - startLerpTime;
            // calculate finished percentage of lerping process
            perc = currentLerpTime / lerpTime;  
            obj.transform.position = Vector3.Lerp(start, end, perc);
            source.Play();
            yield return null;
        }
        
    }

    // used when animationOn = true aka when G is clicked
    // bricks shows up and move to end position one by one by adding delay time
    // delay time is diversed brick by brick
    IEnumerator WaitToDisplay(int index)
    {
        yield return new WaitForSeconds(lerpTime * index);      // delay time
        steine[index].SetActive(true);
        StartCoroutine(MoveToPosition(steine[index], startPos[index], endPos[index]));
    }

    // Update is called once per frame
    void Update()
    {
        //Animation mit G starten und H anhalten
        if (Input.GetKey(KeyCode.H))
        {
            Time.timeScale = 0;
            animationOn = false;
            // Debug.Log(animationOn);
        }
        if (Input.GetKey(KeyCode.G))
        {
            Time.timeScale = 1;
            animationOn = true;
            //Debug.Log(animationOn);
        }

        if (animationOn == true)
        {
            if (count <= steine.Length - 1)
            {
                StartCoroutine(WaitToDisplay(count));
            }
            count += 1;
        }

        //Hier kann man Stein für Stein anzeigen lassen.
        if (Input.GetKeyDown(KeyCode.S) && (count <= steine.Length - 1))
        {
            if (count == 0 && !steine[0].activeInHierarchy)
            {
                steine[count].SetActive(true);
                StartCoroutine(MoveToPosition(steine[count], startPos[count], endPos[count]));
            }
            else if (count < steine.Length - 1)
            {
                count += 1;
                steine[count].SetActive(true);
                StartCoroutine(MoveToPosition(steine[count], startPos[count], endPos[count]));
                // Debug.Log(count);
            }
            
        }

        //Stein für Stein wird abgezogen
        if (Input.GetKeyDown(KeyCode.D) && count >= 0)
        {
            if (count == 0 && steine[0].activeInHierarchy)
            {
                steine[count].SetActive(false);
                //Debug.Log(count);
            }
            else if (count > 0)
            {
                steine[count].SetActive(false);
                //Debug.Log(count);
                count -= 1;
            }

        }


        //Hier kann man sich alle Steine anzeigen lassen
        if (Input.GetKeyDown(KeyCode.A) && count <= steine.Length - 1)
        {
            for (int i = count; i <= steine.Length - 1; i++)
            {
                steine[i].SetActive(true);
            }
            count = steine.Length - 1;
            //Debug.Log(count);
        }
        
        // Reset Funktion
        if (Input.GetKeyDown(KeyCode.R) && count >=0)
        {
            Time.timeScale = 1;
            for (int i = 0; i <= steine.Length - 1; i++)
            {
                steine[i].SetActive(false);
            }
            StopAllCoroutines();
            animationOn = false;
            count = 0;
        }

        //Modell Ebene für Ebene aufbauen
        if (Input.GetKeyDown(KeyCode.K) && count <= steine.Length - 1)
        {
            int naechster = (int) Mathf.Round(steine[count + 1].transform.position.y / layerHeight);
            layer = (int)Mathf.Round(steine[count].transform.position.y / layerHeight);
            if (layer < naechster)
            {
                layer += 1;
            }
            for (int i = 0; i <= steine.Length - 1; i++)
            {
                int level = (int)Mathf.Round(steine[i].transform.position.y / layerHeight);
                if (layer == level)
                {
                    steine[i].SetActive(true);
                    count = i;
                    StartCoroutine(MoveToPosition(steine[count], startPos[count], endPos[count]));
                }
            }
        }

        //Ebenenweise abziehen
        if (Input.GetKeyDown(KeyCode.L) && count >= 0)
        {
            layer = (int)Mathf.Round(steine[count].transform.position.y / layerHeight);
            for (int i = steine.Length - 1; i >= 0 ; i--)
            {
                int level = (int)Mathf.Round(steine[i].transform.position.y / layerHeight);
                if (layer == level)
                {
                    steine[i].SetActive(false);
                    if (count > 0)
                    {
                        count = i - 1;
                    }
                }
            }

        }
    }

        private void instantiateBricks()
        {
            Quaternion rot = Quaternion.Euler(0, 0, 0);
            steine = new GameObject[buildingBlocks.Count];
            for (int i = 0; i < buildingBlocks.Count; i++)
            {
                if (buildingBlocks[i].isFlipped)
                {
                    //Debug.Log(buildingBlocks[i].isFlipped);
                    rot = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    rot = Quaternion.Euler(0, 90f, 0);
                }
                float yPos = buildingBlocks[i].pos.y * 1.92f;
                float xPos = buildingBlocks[i].pos.x * 1.6f;
                float zPos = buildingBlocks[i].pos.z * 1.6f;
                Vector3 position = new Vector3(xPos, yPos, zPos);
                if (buildingBlocks[i].extends == stein_1x1)
                {
                    steine[i] = Instantiate(brick_1x1, position, rot);
                }
                else if (buildingBlocks[i].extends == stein_1x2)
                {
                    steine[i] = Instantiate(brick_1x2, position, rot);
                }
                else if (buildingBlocks[i].extends == stein_1x3)
                {
                    steine[i] = Instantiate(brick_1x3, position, rot);
                }
                else if (buildingBlocks[i].extends == stein_1x4)
                {
                    steine[i] = Instantiate(brick_1x4, position, rot);
                }
                else if (buildingBlocks[i].extends == stein_1x6)
                {
                    steine[i] = Instantiate(brick_1x6, position, rot);
                }
                else if (buildingBlocks[i].extends == stein_1x8)
                {
                    steine[i] = Instantiate(brick_1x8, position, rot);
                }
                else if (buildingBlocks[i].extends == stein_2x2)
                {
                    steine[i] = Instantiate(brick_2x2, position, rot);
                }
                else if (buildingBlocks[i].extends == stein_2x3)
                {
                    steine[i] = Instantiate(brick_2x3, position, rot);
                }
                else if (buildingBlocks[i].extends == stein_2x4)
                {
                    steine[i] = Instantiate(brick_2x4, position, rot);
                }
                else if (buildingBlocks[i].extends == stein_2x6)
                {
                    steine[i] = Instantiate(brick_2x6, position, rot);
                }
                else if (buildingBlocks[i].extends == stein_2x8)
                {
                    steine[i] = Instantiate(brick_2x8, position, rot);
                }
                steine[i].SetActive(false);

                Color whateverColor = buildingBlocks[i].blockColor;
                Color testcolor = new Color(1, 0, 0);
                //steine[i].gameObject.AddComponent<MeshRenderer>();

                MeshRenderer gameObjectRenderer = steine[i].GetComponentInChildren<MeshRenderer>();

                Material newMaterial = new Material(Shader.Find("Diffuse"));

                newMaterial.color = whateverColor;
                gameObjectRenderer.material = newMaterial;

                //Debug.Log(buildingBlocks[i].blockColor);



            }
        }
    

}