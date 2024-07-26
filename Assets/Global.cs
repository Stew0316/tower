using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    // Start is called before the first frame update
    // �����ƶ����ǿ�11��12����Ȧһ�����5��
    // ��Կ��Y����Կ��B����Կ��R���ȼ�Level������Health������Attack������Defense�����Coin������Exp
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
