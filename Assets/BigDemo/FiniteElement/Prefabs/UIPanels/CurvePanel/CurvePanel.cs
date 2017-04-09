using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System;

public class CurvePanel : MonoBehaviour,IRunTimeButton {
    private ICurveModle curveModle;
    public Button Btn
    {
        set
        {
            //
        }
    }

    public Slider slider;
    public int defutCount;
    public Button play;
    public InputField timeContent;
    public event UnityAction OnDelete;

    void Start()
    {
        if (curveModle == null)
        {
            curveModle = FindObjectOfType<BeamSystem>();
            if (curveModle == null)
            {
                curveModle = FindObjectOfType<SteelSystem>();
                if (curveModle == null)
                {
                    Facade.Instance.SendNotification<string[]>("PropUpPanel", new string[] { "初始化失败", "未找到可以压弯的模型" });
                    OnDestroyGameObject();
                }
            }
        }
        if (curveModle != null)
        {
            slider.onValueChanged.AddListener((x) => curveModle.ChangeCurve(x / slider.maxValue));
            play.onClick.AddListener(RePlay);
        }
    }

    void RePlay()
    {
        StopAllCoroutines();
        StartCoroutine(DelyCurve());
    }

    IEnumerator DelyCurve()
    {
        float time = 1;
        if (!string.IsNullOrEmpty(timeContent.text))
        {
            time = float.Parse(timeContent.text);
            time = time == 0 ? 1 : time;
        }
        for (float i = 0; i <= time; i += time / defutCount)
        {
            curveModle.ChangeCurve(i / time);
            yield return new WaitForSeconds(time / defutCount);
        }
    }

    void OnDestroyGameObject()
    {
        OnDelete.Invoke();
        Destroy(gameObject);
    }
}
