using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PopupPanel : MonoBehaviour
{
    [SerializeField] private GameObject PrimaryButton;
    public bool Closable = true, ClosableOverride = false;
    [SerializeField] private AK.Wwise.Event MenuBack;
    public float animProgress;
    private GameObject PreviousButton;
    public static bool open = false;
    public static int numPopups = 0;
    public bool visible = false;
    public static int mouseNeverMoved = 0;
    private Animator anim;
    private GameObject currentSelection;
    [SerializeField] private Image ScreenDarkener;
    [SerializeField] private bool darkensScreen = true;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        open = true;

        if (visible)
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                currentSelection = EventSystem.current.currentSelectedGameObject;
            }
            else
            {
                MenuButton.noSound = true;
                EventSystem.current.SetSelectedGameObject(currentSelection);
            }
        }
        else if (EventSystem.current.currentSelectedGameObject == PrimaryButton)
        {
            EventSystem.current.SetSelectedGameObject(PreviousButton);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && Closable)
        {
            if (ClosableOverride)
                ClosableOverride = false;
            else
                Close();
        }

        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            mouseNeverMoved = 0;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.0 && anim.GetFloat("Speed") < 0 && open)
        {
            Disable();
        }

        if (darkensScreen)
        {
            Color c = ScreenDarkener.color;
            c.a = animProgress * 0.5f;
            ScreenDarkener.color = c;
        }
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        anim.SetFloat("Speed", 1);
        open = true;
        mouseNeverMoved = 2;
        numPopups++;
        foreach (MenuButton c in GetComponentsInChildren<MenuButton>())
        {
            c.popupID = numPopups;
        }
        visible = true;
        PreviousButton = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(PrimaryButton);
        Debug.Log("set prim button");
        MenuButton.pleaseNoSound = true;
        if (darkensScreen)
            ScreenDarkener.raycastTarget = true;
    }

    public void Close()
    {
        MenuBack?.Post(gameObject);
        // anim.SetTrigger("Close");
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.93)
            anim.SetFloat("Speed", -2);
        else
            anim.SetFloat("Speed", -10);
        if (darkensScreen)
            ScreenDarkener.raycastTarget = false;
        numPopups--;
        visible = false;
        MenuButton.pleaseNoSound = true;
        EventSystem.current.SetSelectedGameObject(PreviousButton);
    }

    public void Disable()
    {
        if (anim.GetFloat("Speed") < 0)
        {
            gameObject.SetActive(false);
            MenuButton.pleaseNoSound = false;
            anim.SetFloat("Speed", 0);
            open = false;
        }
    }

    public void NormalSpeed()
    {
        if (anim.GetFloat("Speed") < 0)
        {
            anim.SetFloat("Speed", -2);
        }
    }

    public void ResetSpeed()
    {
        if (anim.GetFloat("Speed") > 0)
        {
            anim.SetFloat("Speed", 0);
            MenuButton.pleaseNoSound = false;
        }
    }
}