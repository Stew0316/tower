using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public float speed = 5f; // �ƶ��ٶ�
    private Rigidbody2D rb;

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
        Vector2 movement = new Vector2(moveX, moveY) * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hero enter");
    }
}
