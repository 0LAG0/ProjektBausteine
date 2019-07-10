using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class SimpleMovement : MonoBehaviour
{
    private bool started = false;
    private Vector3 target;
    private Vector3 start;
    private float percentage = 0;
    private float startLerpTime;
    private float timeForFullLerp;
    private AudioSource audioSource;

    public void MoveObjectFromTo(Vector3 startPos, Vector3 targetPos, float TimeForFullLerp, AudioSource audio)
    {
        audioSource = audio;
        start = startPos;
        target = targetPos;
        timeForFullLerp = TimeForFullLerp;
        this.transform.localPosition = startPos;
        startLerpTime = Time.time;
        started = true;
    }

    private void Update()
    {
        if (started)
        {
            if (percentage < 1.0f)
            {
                float currentLerpTime = Time.time - startLerpTime;
                // calculate finished percentage of lerping process
                percentage = currentLerpTime / timeForFullLerp;
                this.transform.localPosition = Vector3.Lerp(start, target, percentage);
            }
            else
            {
                audioSource.Play();
                Destroy(GetComponent<SimpleMovement>());
            }
        }
    }
}
