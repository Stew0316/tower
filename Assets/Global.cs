using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    // Start is called before the first frame update
    // 核心移动区是宽11高12，外圈一格，左边5格
    // 黄钥匙Y，蓝钥匙B，红钥匙R，等级Level，生命Health，攻击Attack，防御Defense，金币Coin，经验Exp
    static Global _instance;
    public int KEY_Y = 10;
    public int KEY_R = 1;
    public int KEY_B = 1;
    public int Level = 1;
    public int Health = 1000;
    public int Attack = 10;
    public int Defense = 10;
    public int Coin = 0;
    public int Exp = 0;

    private void Awake()
    {
        _instance = this;
    }

    public static Global Instance
    {
        get { return _instance; }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
