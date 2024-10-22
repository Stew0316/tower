using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class textFade : MonoBehaviour
{
    public TextMeshProUGUI textGUI;
    public float timeStemp = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        textGUI = GetComponent<TextMeshProUGUI>();

        //Debug.Log($"1111:{textGUI}{textGUI.text}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
