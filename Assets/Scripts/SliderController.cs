using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    private Slider _slider;

    void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    void Start()
    {
        float volume01 = AudioManager.GetMasterVolume();
        _slider.value = volume01;
    }

    public void SetVolume(float value)
    {
        AudioManager.SetMasterVolume(value);
        // print(value);
    }
}
