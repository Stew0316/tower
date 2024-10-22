using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public float speed = 5f; // 移动速度
    private Rigidbody2D rb;
    public bool moveDisabled = false;
    public Vector2 movement;

    private SpriteRenderer sr;
    //上右下左的方向
    public Sprite[] sr_arr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    } 

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        //// 获取输入
        //float moveX = Input.GetAxis("Horizontal");
        //float moveY = Input.GetAxis("Vertical");

        //// 计算移动量
        //movement = new Vector2(moveX, moveY) * speed * Time.fixedDeltaTime;
        ////Debug.Log($"{movement}, {movement[0]}, {movement[1]}");
        //if(!moveDisabled)
        //{
        //    rb.MovePosition(rb.position + movement);
        //}
        if (Global.Instance.isPaused) return;
        float h = Input.GetAxisRaw("Horizontal");
        transform.Translate(Vector3.right * h * speed * Time.deltaTime, Space.World);
        if(h<0)
        {
            sr.sprite = sr_arr[3]; 
        }
        else if(h>0)
        {
            sr.sprite = sr_arr[1];
        }


        float v = Input.GetAxisRaw("Vertical");
        transform.Translate(Vector3.up * v * speed * Time.deltaTime, Space.World);
        if (v < 0)
        {
            sr.sprite = sr_arr[2];
        }
        else if (v > 0)
        {
            sr.sprite = sr_arr[0];
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }
}
