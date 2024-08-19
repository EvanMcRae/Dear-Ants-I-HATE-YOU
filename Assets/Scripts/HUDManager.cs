using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI EXPValue, WaveValue, LevelValue;
    [SerializeField] private GameObject WorkshopPanel = null;
    // TODO need hearts container here
    // TODO need hurt vignette here

    // Update is called once per frame
    void Update()
    {
        WaveValue.text = WaveManager.CurrentWave + "";
        LevelValue.text = stageManager.level + "";
    }

    public void OpenWorkshop()
    {
        // TODO stopgap for now, remove later
        EventSystem.current.SetSelectedGameObject(null);
        if (WorkshopPanel != null)
            WorkshopPanel.SetActive(true);
    }
}
