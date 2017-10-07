using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitScreen : MonoBehaviour {

    static bool splitEnabled = false;

    private enum POSITION{
        LEFT,
        RIGHT
    }

    [SerializeField]
    private Camera m_Cam;
    private Color m_color;
    [SerializeField]
    private POSITION m_pos = POSITION.LEFT;

	void Start () {
        m_Cam = (Camera) gameObject.GetComponent<Camera>();
        if (!m_Cam) Debug.LogError("Splitscreen : Missing camera object");
        else
        {
            m_color = m_Cam.backgroundColor;
            m_Cam.enabled = false;
        }
        switch(m_pos)
        {
            case POSITION.LEFT: { m_Cam.rect = new Rect(0.0f,0.0f,0.5f,1.0f); } break;
            case POSITION.RIGHT: { m_Cam.rect = new Rect(0.5f,0.0f,0.5f,1.0f); } break;
        }
	}

    void OnEnable()
    {
        EventManager.StartListening("splitscreen", triggerState);
    }

    void OnDisable()
    {
        EventManager.StopListening("splitscreen", triggerState);
    }

    public static void switchState()
    {
        SplitScreen.splitEnabled = !splitEnabled;
    }

    public static bool splitState()
    {
        return SplitScreen.splitEnabled;
    }

    void triggerState()
    {
        m_Cam.enabled = splitEnabled;
    }
}
