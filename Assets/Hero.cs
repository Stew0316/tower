using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public float speed = 5f; // �ƶ��ٶ�
    private Rigidbody2D rb;
    public bool moveDisabled = false;
    public Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // ��ȡ����
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // �����ƶ���
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
