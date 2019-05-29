using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestArrayAnimation : MonoBehaviour
{
    public GameObject[] steine;

    private float[] positionY;

    public float startHeight = 10f;

    public float speed = 10;
    private int count = 0;

    private Vector3 temp;

    AudioSource source;

    private void Awake()
    {
        
    }

    void Start()
    {
        steine = sortedArray(steine);
        for (int i = 0; i < steine.Length; i++)
        {
            Debug.Log(steine[i].transform.position.y);
        }

        source = GetComponent<AudioSource>();

        //Vector entlang der Y-Achse
        temp = new Vector3(0, startHeight, 0);

        //Array in dem die Steine mit erhöhter Position gespeichert werden
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
        //Erhöhte Position der Steine
        Vector3 startPosition = steine[count].transform.position;
        //Originale Position der Steine
        Vector3 endPosition = new Vector3(steine[count].transform.position.x, positionY[count], steine[count].transform.position.z);
        //Bewegung der Steine zwischen erhöhter und Originalposition
        steine[count].transform.position = Vector3.Lerp(startPosition, endPosition, Time.deltaTime * speed);
        if (Input.GetKeyDown(KeyCode.H) && count < steine.Length)
        {
            count += 1;
            source.Play();
        }
    
    }

    //Sortiert Array nach Y-Werten
    private GameObject[] sortedArray(GameObject[] steine)
    {
        steine = steine.OrderBy(go => go.transform.position.y).ToArray();
        return steine;
    }
}