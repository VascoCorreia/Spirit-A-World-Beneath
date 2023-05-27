using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VcaController1 : MonoBehaviour
{
    private FMOD.Studio.VCA VcaController;
    public string VcaName;

    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        VcaController = FMODUnity.RuntimeManager.GetVCA("vca:/" + VcaName);
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    public void SetVolume(float volume)
    {
        VcaController.setVolume(volume);
    }
}
