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
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("enter");
        // 当角色进入边界碰撞体时，可以执行特定逻辑
        
    }
    void OnTriggerExit2D(Collider2D other)
    {
        // 处理离开触发区域的逻辑
        Debug.Log("Exited trigger: ");
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("dddd");
    }
}
