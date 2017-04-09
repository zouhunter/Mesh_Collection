using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BaseToggleOpen :MonoBehaviour, IRunTimeToggle {
    public Toggle toggle
    {
        set
        {
            m_tog = value;
        }
    }
    private Toggle m_tog;

    public event UnityAction OnDelete;

    private void Start()
    {
        if(m_tog) m_tog.onValueChanged.AddListener(ShowObject);
    }

    void ShowObject(bool show)
    {
        gameObject.SetActive(show);
    }

    void OnDestroy () {
        if(OnDelete!=null) OnDelete.Invoke();
    }
}
