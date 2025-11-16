using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("volume", 0.5f);
        AudioListener.volume = slider.value;

        slider.onValueChanged.AddListener(delegate {
            AudioListener.volume = slider.value;
            PlayerPrefs.SetFloat("volume", slider.value);
        });
    }
}
