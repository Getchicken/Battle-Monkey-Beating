using UnityEngine;
using UnityEngine.UI;

public class VolumeHandler : MonoBehaviour
{
    AudioManager am;
    public Slider slider;
    public float sliderVolumeValue;

    void Start()
    {
        am = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();

        slider.minValue = 0.0f;
        slider.maxValue = 1.0f;

        sliderVolumeValue = 0.1f;
        slider.value = sliderVolumeValue;
    }

    void FixedUpdate()
    {
        sliderVolumeValue = slider.value;
        am.OnPitchSliderChanged(sliderVolumeValue);
    }
}
