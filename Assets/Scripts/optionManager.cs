using UnityEngine;

public class OptionManager : MonoBehaviour
{
    public void VolumeManager(float value)
    {
        AudioManager.SetMasterVolume(value);
        print(value);
    }


}
