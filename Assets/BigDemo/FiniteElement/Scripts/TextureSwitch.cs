using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureSwitch : MonoBehaviour {

    [System.Serializable]
    public class Data
    {
        public shijianchoosebut.Choise choise;
        public string gangPath;
        public string weiyiPath;
        public string yinbianPath;
    }

    public List<Data> textures;
    
    void Start()
    {
        BeamSystem beam = GetComponent<BeamSystem>();
        SetBeamSystemTextures(beam);
        SteelSystem steel = GetComponent<SteelSystem>();
        SetSteelSystemTextures(steel);
    }

    void SetBeamSystemTextures(BeamSystem beam)
    {
        if (beam != null)
        {
            Data data = textures.Find(x => x.choise == shijianchoosebut.choise);
            Texture[] weiyi = Resources.LoadAll<Texture>(data.weiyiPath);
            Texture[] yinbian = Resources.LoadAll<Texture>(data.yinbianPath);
            beam.weiyiTextures = new List<Texture>(weiyi);
            beam.yinbianTextures = new List<Texture>(yinbian);
            beam.InitMaterial(weiyi[0],yinbian[0]);
        }
    }
    void SetSteelSystemTextures(SteelSystem steel)
    {
        if (steel != null)
        {
            Data data = textures.Find(x => x.choise == shijianchoosebut.choise);
            Texture[] txs = Resources.LoadAll<Texture>(data.gangPath);
            steel.textures = new List<Texture>(txs);
        }
    }
}
