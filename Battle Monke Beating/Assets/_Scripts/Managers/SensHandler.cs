using UnityEngine;
using UnityEngine.UI;

public class SensHandler : MonoBehaviour
{
    PlayerCam PCam;
    public Slider slider;
    public float sliderSensValue;

    void Start()
    {
        PCam = FindObjectOfType<Camera>().GetComponentInChildren<PlayerCam>();

        slider.minValue = 1f;
        slider.maxValue = 600f;

        slider.value = sliderSensValue;
    }

    public void UpdateSens()
    {
        sliderSensValue = slider.value;
        PCam.ChangeSens(sliderSensValue);
    }
}
