using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI EXPValue, EXPValue2, WaveValue, LevelValue;
    [SerializeField] private GameObject WorkshopPanel = null;
    [SerializeField] private List<Image> Hearts;
    [SerializeField] private Sprite fullHeart, emptyHeart;
    [SerializeField] private Animator HurtVignette;
    public static HUDManager main;

    void Awake()
    {
        main = this;
    }

    // Update is called once per frame
    void Update()
    {
        WaveValue.text = WaveManager.CurrentWave + "";
        LevelValue.text = stageManager.level + "";
    }

    public void OpenWorkshop()
    {
        WorkshopPanel.SetActive(true);
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
}