using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI EXPValue, WaveValue, LevelValue;
    [SerializeField] private GameObject WorkshopPanel;
    // TODO need hearts container here

    // Update is called once per frame
    void Update()
    {
        WaveValue.text = WaveManager.CurrentWave + "";
        LevelValue.text = stageManager.level + "";
    }
}
