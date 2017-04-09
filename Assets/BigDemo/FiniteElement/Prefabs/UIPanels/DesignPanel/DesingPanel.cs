using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

public class DesingPanel :MonoBehaviour, IRunTimeButton {
    public Button Btn
    {
        set
        {
            //openBtn = value;
        }
    }
    public event UnityAction OnDelete;
    public BeamSystem beamPfb;

    public Button closeBtn;
    public Button initBtn;
    public Button pointBtn;
    public Button lineBtn;
    public InputField gridSize;

    private BeamSystem m_beam { get { return Develop.Main.beamSystem; } set { Develop.Main.beamSystem = value; } }
    //private Button openBtn;

    void Awake()
    {
        initBtn.onClick.AddListener(InitBeam);
        closeBtn.onClick.AddListener(OnClose);
        pointBtn.onClick.AddListener(CreatePoints);
        lineBtn.onClick.AddListener(CreateLine);
    }

    void InitBeam()
    {
        if (m_beam != null){
            Destroy(m_beam.gameObject);
        }
        m_beam = Instantiate(beamPfb);
        m_beam.InitBeamView();
    }

    void OnClose()
    {
        OnDelete.Invoke();
        Destroy(gameObject);
    }
    void CreatePoints()
    {
        if (!string.IsNullOrEmpty(gridSize.text)||(gridSize.text.Contains(".") && gridSize.text.Substring('.').Length >2))
        {
            int value =Mathf.CeilToInt(float.Parse(gridSize.text) * 100);
            if (m_beam == null)
            {
                m_beam = Instantiate(beamPfb);
            }
            if (value < 3)
            {
                Debug.Log(value);
                Facade.Instance.SendNotification<string[]>("PopupPanel", new string[] { "创建失败", "尺寸不能小于0.03m，对于当前尺寸" + gridSize.text + "m,程序性能不足" });
            }
            else if (!m_beam.CreatePoints(value))
            {
                Facade.Instance.SendNotification<string[]>("PopupPanel", new string[] { "创建失败", "最小尺寸不是长宽高的公约数，请重新设置" });
            }
        }
        else
        {
            Facade.Instance.SendNotification<string[]>("PropUpPanel", new string[] { "创建失败", "请输入以m为单位精度为cm的尺寸" });
        }
    }

    void CreateLine()
    {
        m_beam.CreateLines(true);
    }
}
