using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fairy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"{Global.Instance.Attack}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Global.Instance.isPaused = true;
        Debug.Log("fairy coll enter");
    }
}
