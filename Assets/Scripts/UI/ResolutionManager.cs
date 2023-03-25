using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionManager : MonoBehaviour {
    [SerializeField] private int width;    
    [SerializeField] private int height;

    public void SetWidth(int width) {
        this.width = width;
    }

    public void SetHeight(int height) {
        this.height = height;
    }

    public void SetResolution() {
        SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.BUTTON_CLICK);
        Screen.SetResolution(width, height, false);
    }

    public void SetResolutionMaximized() {
        SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.BUTTON_CLICK);
        // Get the maximum supported resolution of the display device
        Resolution maxResolution = Screen.currentResolution;

        // Set the resolution to the maximum supported resolution while still in windowed mode
        Screen.SetResolution(maxResolution.width, maxResolution.height, false);
    }

    public void SetResolutionFullScreen() {
        SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.BUTTON_CLICK);
        // Get the maximum supported resolution of the display device
        Resolution maxResolution = Screen.currentResolution;

        // Set the resolution to the maximum supported resolution while still in windowed mode
        Screen.SetResolution(maxResolution.width, maxResolution.height, true);
    }
}
