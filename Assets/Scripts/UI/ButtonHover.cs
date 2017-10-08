using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Sprite ImageHover;
    public Sprite ImageNormal;

    private Image buttonImage;

    public bool state;

    public void Awake()
    {
        buttonImage = gameObject.GetComponent<Image>();
        state = EventSystem.current.currentSelectedGameObject == gameObject;
    }

    public void Start()
    {
        if (gameObject.name == "Quit")
        {
            Button btn = GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => { Application.Quit(); });
        }
        if(gameObject.name == "Start")
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }

    public void Update()
    {
        bool newState = EventSystem.current.currentSelectedGameObject == gameObject;
        if (state != newState)
        {
            PointerEventData data = new PointerEventData(EventSystem.current);
            if (newState)
                OnPointerEnter(data);
            else
                OnPointerExit(data);
            state = newState;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.sprite = ImageHover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.sprite = ImageNormal;
    }
}