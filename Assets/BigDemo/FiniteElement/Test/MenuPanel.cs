using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class MenuPanel : MonoBehaviour {
    public Toggle swh;
    private BeamSystem m_beam { get { return Develop.Main.beamSystem; } set { Develop.Main.beamSystem = value; } }

    void Start()
    {
        swh.onValueChanged.AddListener((x) => {
            if (m_beam != null)
            {
                m_beam.SetWeiyiTexture(x);
            }
        });
    }
}
