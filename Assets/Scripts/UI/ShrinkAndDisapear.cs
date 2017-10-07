using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkAndDisapear : MonoBehaviour {

    float start;
    float duration;

	void Awake () {
        start = Time.time;
        duration = GameManager.instance.circleDuration;
	}

	void Update () {
        float ratio = ((Time.time - start) / ((start + duration) - start));
        if(ratio < .5f)
            transform.localScale = Vector3.Lerp(Vector3.one * 3, Vector3.one, (ratio)*2);
        else
        {
            if(ratio < .75f)
                transform.localScale = Vector3.Slerp(Vector3.one, Vector3.one * 1.5f, (ratio-.5f) * 4);
            else
                transform.localScale = Vector3.Slerp(Vector3.one * 1.5f, Vector3.zero, (ratio-.75f) * 4);
        }
	}
}
