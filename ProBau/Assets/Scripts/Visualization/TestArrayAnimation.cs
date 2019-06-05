﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

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

    public float speed = 10f;
    private int count = 0;
    private int brickSize;

    private Vector3 temp;
    //Erhöhte Position der Steine
    public Vector3[] startPosition;
    //Originale Position der Steine
    public Vector3[] endPosition;

    private bool animationOn = false;

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
        buildingBlocks = selector.calculateBlocksSpiral(Voxelizer.Voxelize(mesh, tex, targetHeight));
        //
        //
        Debug.Log(buildingBlocks.Count);
        brickSize = buildingBlocks.Count;

        //Vector entlang der Y-Achse
        startHeight = buildingBlocks[brickSize - 1].pos.y + buildingBlocks[brickSize - 1].pos.y / 2;
        temp = new Vector3(0, 50, 0);
        startPosition = new Vector3[brickSize];
        endPosition = new Vector3[brickSize];



        buildingBlocks.Sort(SortByY);
        instantiateBricks();


        source = GetComponent<AudioSource>();
    }

    private int SortByY(BuildingBlock a, BuildingBlock b)
    {
        return a.pos.y.CompareTo(b.pos.y);
    }

    // Update is called once per frame
    void Update()
    {
        //Animation anhalten mit T  & starten mit R
        if ((Input.GetKey(KeyCode.T) && animationOn) || (animationOn && count == brickSize-1))
        {
            animationOn = false;
            Debug.Log(animationOn);
        }
        if (Input.GetKey(KeyCode.R) && animationOn == false && count < brickSize)
        {
            animationOn = true;
            count += 1;
            Debug.Log(animationOn);
        }

        //Man muss hier die Taste gedrückt halten, damit die Steine runterfallen. Ich habe schon Jan gefragt, wie man das löst.
        //Vielleicht findest du ja eine Möglichkeit
        if (animationOn == true)
        {
           
            Debug.Log(steine[count].transform.position);
           
            Debug.Log(endPosition[count]);
            Debug.Log(endPosition[count]);
            steine[count].SetActive(true);
            //steine[count].transform.position = Vector3.Lerp(startPosition[count] , endPosition[count], Time.deltaTime * speed);
            steine[count].transform.Translate(Vector3.down * Time.deltaTime * speed, Space.World);

            if (steine[count].transform.position.y == endPosition[count].y)
            {
                steine[count].transform.position = endPosition[count];
                count += 1;
            }
            //if (steine[count].transform.position == endPosition[count])
            //source.Play();
        }



        //Hier kann man Stein für Stein anzeigen lassen.
        if (Input.GetKeyDown(KeyCode.S) && (count <= steine.Length - 1))
        {
            if (count == 0 && !steine[0].activeInHierarchy)
            {
                steine[count].SetActive(true);
                Debug.Log(count);
            }
            else if (count < steine.Length - 1)
            {
                count += 1;
                steine[count].SetActive(true);
                Debug.Log(count);
            }
        }

        //Hier sollte man Stein für Stein wieder abziehen.
        //Wenn alle Steine angezeigt sind, dann würde er ja den letzten nicht abziehen. 
        //Das habe ich hier versucht zu regeln, aber leider nicht geschafft.
        if (Input.GetKeyDown(KeyCode.D) && count >= 0)
        {
            if (count == 0 && steine[0].activeInHierarchy)
            {
                steine[count].SetActive(false);
                Debug.Log(count);
            }
            else if (count > 0)
            {
                steine[count].SetActive(false);
                Debug.Log(count);
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
            Debug.Log(count);
        }


        //Steine wieder hochfallen lassen
        /*
        if (Input.GetKeyDown(KeyCode.H) && count > 0 && steine[count].transform.position == startPosition)
        {
            startPosition = steine[count].transform.position;
            endPosition = new Vector3(steine[count].transform.position.x, positionY[count], steine[count].transform.position.z);
            steine[count].transform.position = Vector3.Lerp(endPosition, startPosition, Time.deltaTime * speed);
            count -= 1;
        }
        */
    }

    private void instantiateBricks()
    {
        Quaternion rot = Quaternion.Euler(0, 0, 0);
        steine = new GameObject[buildingBlocks.Count];
        for (int i = 0; i < buildingBlocks.Count; i++)
        {
            if (buildingBlocks[i].isFlipped)
            {
                Debug.Log(buildingBlocks[i].isFlipped);
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
            endPosition[i] =  position;

            steine[i].transform.position = position + temp;

        }
    }
}