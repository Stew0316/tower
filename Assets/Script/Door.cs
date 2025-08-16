using System;
using UnityEngine;
using Game.Enums;
[RequireComponent(typeof(Animator), typeof(Collider2D))]
public class KeyCollisionPlay : MonoBehaviour
{
    public DoorType doorType;
    private Animator _anim;
    private Collider2D _col;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _col = GetComponent<Collider2D>();
    }

    // 碰撞回调 —— 玩家和钥匙的非 Trigger 碰撞
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player"))
        {
            return;
        }
        bool check = Global.Instance.CheckOpenDoor(doorType);
        if (!check) return;
        // 播放动画
        _anim.SetTrigger("Play");
    }

    // 最后一帧的 Animation Event 回调
    public void OnAnimFinish()
    {
        Global.Instance.ReduceDoor(doorType);
        Destroy(gameObject);
    }
}
