using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestArrayAnimation : MonoBehaviour
{
    public GameObject[] steine;
    private float[] positionY;
    private GameObject[] steineFallen;

    public float startHeight = 10f;
    public float speed = 10;
    private int count = 0;
    private Vector3 temp;
    Vector3 step = new Vector3(0,-1,0);

    void Start()
    {
        temp = new Vector3(0, startHeight, 0);
        positionY = new float[steine.Length];
        for (int i = 0; i < steine.Length; i++)
        {
            positionY[i] = steine[i].transform.position.y;
            
            steine[i].transform.position += temp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 startPosition = steine[count].transform.position;
        Vector3 endPosition = new Vector3(steine[count].transform.position.x, positionY[count], steine[count].transform.position.z);
        steine[count].transform.position = Vector3.Lerp(startPosition, endPosition, Time.deltaTime * speed);

        if (steine[count].transform.position == endPosition)
        {
            count += 1;
        }
    }
}
