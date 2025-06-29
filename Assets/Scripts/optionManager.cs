using UnityEngine;
using UnityEngine.UI;

public class optionManager : MonoBehaviour
{
    public Slider slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void VolumeManager()
    {
        AudioManager.SetMasterVolume(slider.value);
    }

    
}
