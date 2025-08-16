using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;

public class textFade : MonoBehaviour
{
    public TextMeshProUGUI textGUI;
    public float timeStemp = 6.0f;
    private bool up = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (up)
        {
            textGUI.alpha = textGUI.alpha + (Time.fixedDeltaTime * timeStemp);
        }
        else
        {
            textGUI.alpha = textGUI.alpha - (Time.fixedDeltaTime * timeStemp);
        }
        if (textGUI.alpha >= 1.0f)
        {
            up = false;
        }
        else if (textGUI.alpha <= 0.0f)
        {
            up = true;
        }
    }

}
