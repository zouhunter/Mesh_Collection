using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SystemCamera : MonoBehaviour,IRunTimeMessage {

    public event UnityAction OnDelete;

    private Camera m_Camera;
    private void OnEnable()
    {
        m_Camera = GetComponentInChildren<Camera>();
        Facade.Instance.RegisterEvent<object>(AppFixed.ExperimentEvents.OPENCAMERA, HandleMessage);
    }
    
    public void HandleMessage(object message)
    {
        if (message is bool)
        {
            bool isOpen = (bool)message;
            if (!isOpen)
            {
                Destroy(gameObject);
            }
        }
        else if (message is RectTransform)
        {
            m_Camera.enabled = true;
            RectTransform rect = (RectTransform)message;
            m_Camera.rect = new Rect(rect.anchorMin.x, rect.anchorMin.y, rect.anchorMax.x - rect.anchorMin.x, rect.anchorMax.y - rect.anchorMin.y);
        }
        
    }

    private void OnDisable()
    {
        Facade.Instance.RemoveEvent<object>(AppFixed.ExperimentEvents.OPENCAMERA, HandleMessage);
    }
    private void OnDestroy()
    {
        OnDelete.Invoke();
    }
}
