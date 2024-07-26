using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBoundary : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("enter1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("colllllllll");
    }
}
