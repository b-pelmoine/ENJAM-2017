using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaleUpAndDown : MonoBehaviour {

    public float pulseSpeed;
    private float endTime = 0.0f;
    private float startTime = 0.0f;
    private float elapsedTime = 0.0f;
    public float pulseScale = 2.0f;
    private bool ascend = true;
    private Vector3 baseScale;

    void Start()
    {
        baseScale = transform.localScale;
    }

	void Update () {
        elapsedTime += Time.deltaTime;
        if(startTime + elapsedTime > endTime)
        {
            startTime = Time.time;
            endTime = startTime + pulseSpeed;
            elapsedTime = 0;
            ascend = !ascend;
        }
        if(ascend)
        {
            transform.localScale = Vector3.Lerp(baseScale, baseScale * pulseScale, elapsedTime / pulseSpeed);
        }
        else
        {
            transform.localScale = Vector3.Lerp(baseScale * pulseScale, baseScale, elapsedTime / pulseSpeed);
        }
    }
}
