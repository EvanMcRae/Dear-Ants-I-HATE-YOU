using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopListing : MonoBehaviour
{

    public int cost = 1;
    public string name = "undefined";

    public TMPro.TextMeshProUGUI listingText;
    public TMPro.TextMeshProUGUI listingCost;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateLabels()
    {
        listingText.text = name;
        listingCost.text = "Exp: " + cost;
    }

    public virtual void OnClick()
    {
        //Start placement step, if there is sufficient exp
    }
}
