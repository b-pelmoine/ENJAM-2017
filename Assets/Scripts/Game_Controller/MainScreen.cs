using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MainScreen : MonoBehaviour {

    private Camera m_Cam;

    void awake()
    {
        m_Cam = GetComponent<Camera>();
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
    }
}
