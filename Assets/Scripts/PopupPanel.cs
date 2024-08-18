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
    [SerializeField] private bool Closable = true;
    [SerializeField] private AK.Wwise.Event MenuBack;
    public float animProgress;
    private GameObject PreviousButton;
    public static bool open = false, visible = false;
    public static int mouseNeverMoved = 0;
    private Animator anim;
    private GameObject currentSelection;
    [SerializeField] private Image ScreenDarkener;

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
                EventSystem.current.SetSelectedGameObject(currentSelection);
            }
        }
        else if (EventSystem.current.currentSelectedGameObject == PrimaryButton)
        {
            EventSystem.current.SetSelectedGameObject(PreviousButton);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && Closable)
        {
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

        Color c = ScreenDarkener.color;
        c.a = animProgress * 0.5f;
        ScreenDarkener.color = c;
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        anim.SetFloat("Speed", 1);
        open = true;
        mouseNeverMoved = 2;
        visible = true;
        PreviousButton = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(PrimaryButton);
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
        ScreenDarkener.raycastTarget = false;
        visible = false;
        EventSystem.current.SetSelectedGameObject(PreviousButton);
        Debug.Log(EventSystem.current.currentSelectedGameObject);
    }

    public void Disable()
    {
        if (anim.GetFloat("Speed") < 0)
        {
            gameObject.SetActive(false);
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
}