using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MainScreen : MonoBehaviour {

    private Camera m_Cam;

    void Awake()
    {
        m_Cam = gameObject.GetComponent<Camera>();
    }

    void OnEnable()
    {
        EventManager.StartListening("splitscreen", triggerState);
    }

    void OnDisable()
    {
        EventManager.StopListening("splitscreen", triggerState);
    }

    void triggerState()
    {
        m_Cam.enabled = !SplitScreen.splitState();
        if (!SplitScreen.splitState())
        {
            //cry in a corner
        }
    }
}
