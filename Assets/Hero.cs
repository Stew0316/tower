using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public float speed = 5f; // 移动速度
    private Rigidbody2D rb;
    public bool moveDisabled = false;
    public Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // 获取输入
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // 计算移动量
        movement = new Vector2(moveX, moveY) * speed * Time.fixedDeltaTime;
        //Debug.Log($"{movement}, {movement[0]}, {movement[1]}");
        if(!moveDisabled)
        {
            rb.MovePosition(rb.position + movement);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        moveDisabled = true;
        Debug.Log($"hero enter {collision.bounds}");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        moveDisabled = false;
        Debug.Log("hero exit");
    }
}
