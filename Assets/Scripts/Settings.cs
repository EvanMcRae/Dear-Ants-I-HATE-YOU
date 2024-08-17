using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Settings
{
    public float soundVolume = 50f;
    public float musicVolume = 50f;
    public bool fullScreen = true;
    public float xRes = 0.5f, yRes = 0.5f;
    public bool vSync = false;
    public int quality = 1;
}
