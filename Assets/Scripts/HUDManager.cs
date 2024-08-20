using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI EXPValue, EXPValue2, WaveValue, LevelValue, WorkshopHeader;
    [SerializeField] private GameObject WorkshopPanel = null;
    [SerializeField] private List<Image> Hearts;
    [SerializeField] private Sprite fullHeart, emptyHeart;
    [SerializeField] private Animator HurtVignette;
    public static HUDManager main;
    public bool units = true;
    [SerializeField] private AK.Wwise.Event MenuConfirm;

    public GameObject TowersList;
    public GameObject UpgradesList;

    void Awake()
    {
        main = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!WaveManager.captureWaveMonitor)
            WaveValue.text = WaveManager.CurrentWave + 1 + "";
        LevelValue.text = stageManager.level + "";
    }

    public void OpenWorkshop()
    {
        WorkshopPanel.SetActive(true);
        MenuConfirm.Post(gameObject);
    }

    public void UpdateHealth()
    {
        for (int i = 0; i < GameplayManager.main.maxPlayerHealth; i++)
        {
            if (i <= GameplayManager.main.currPlayerHealth - 1)
            {
                Hearts[i].sprite = fullHeart;
            }
            else
                Hearts[i].sprite = emptyHeart;
        }
    }

    public void UpdateEXP()
    {
        EXPValue.text = GameplayManager.main.resourcePoints + "";
        EXPValue2.text = EXPValue.text;
    }

    public void HurtEffect()
    {
        HurtVignette.SetTrigger("hurt");
    }

    public void LoadUnits()
    {
        if (!units)
        {
            units = true;
            WorkshopHeader.text = "UNITS";
            MenuConfirm.Post(gameObject);
            UpgradesList.SetActive(false);
            TowersList.SetActive(true);
        }
    }

    public void LoadUpgrades()
    {
        if (units)
        {
            units = false;
            WorkshopHeader.text = "UPGRADES";
            MenuConfirm.Post(gameObject);
            UpgradesList.SetActive(true);
            TowersList.SetActive(false);
        }
    }
}
