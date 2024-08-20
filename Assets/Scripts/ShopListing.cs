using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopListing : MonoBehaviour
{

    public int cost = 1;
    public new string name = "undefined";

    public TMPro.TextMeshProUGUI listingText;
    public TMPro.TextMeshProUGUI listingCost;
    public Image backing;

    public Button button;
    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public void UpdateLabels()
    {
        listingText.text = name;
        listingCost.text = "Exp: " + cost;
    }

    public virtual void OnClick()
    {
        HUDManager.main.CloseWorkshop();
        //Start placement step, if there is sufficient exp
    }
}
