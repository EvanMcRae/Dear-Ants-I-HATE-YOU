using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IDeselectHandler, ISelectHandler
{
    [SerializeField] private Sprite normal;
    [SerializeField] private bool MainMenu;
    [SerializeField] private AK.Wwise.Event MenuNav;
    [SerializeField] private Image image;
    public static bool noSound = false, pleaseNoSound = false;
    public int popupID = 0;
    public bool deselectsOnPointerLeave = false;
    
    public void OnDeselect(BaseEventData eventData)
    {
        if (image == null)
            GetComponent<Image>().sprite = normal;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (PopupPanel.mouseNeverMoved > 0)
        {
            PopupPanel.mouseNeverMoved--;
            return;
        }

        if (popupID == PopupPanel.numPopups)
            EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (eventData.hovered.Contains(gameObject) && EventSystem.current.currentSelectedGameObject != gameObject && popupID == PopupPanel.numPopups)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (deselectsOnPointerLeave && EventSystem.current.currentSelectedGameObject == gameObject)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (noSound || pleaseNoSound)
        {
            noSound = false;
            return;
        }
        if (MainMenuManager.firstopen && !MainMenuManager.quitting && !MainMenuManager.playing)
        {
            MenuNav?.Post(gameObject);
        }
    }
}
