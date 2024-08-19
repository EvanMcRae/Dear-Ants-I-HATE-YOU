using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Settings
{
    public float masterVolume = 50f;
    public float musicVolume = 50f;
    public float soundVolume = 50f;
    public int quality = 3;
    public bool fullScreen = true;
    public bool vSync = false;
    public float xRes = 1f, yRes = 1f;
}
