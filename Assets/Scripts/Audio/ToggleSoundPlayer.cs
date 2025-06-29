using System;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSoundPlayer : MonoBehaviour
{
    Toggle m_Toggle;
    Boolean pitch = false;

    void Start()
    {
        //Fetch the Toggle GameObject
        m_Toggle = GetComponent<Toggle>();
        //Add listener for when the state of the Toggle changes, to take action
        m_Toggle.onValueChanged.AddListener(delegate {
            if (pitch) {
                pitch = false;
            }
            else if (pitch == false) {
                pitch = true;
            }
            ToggleValueChanged(m_Toggle);
        });
    }

    void ToggleValueChanged(Toggle change)
    {
        if (pitch)
            AudioManager.PlaySound2D("Click", 0.3f, 0.9f);
        else
            AudioManager.PlaySound2D("Click", 0.3f);
    }
}