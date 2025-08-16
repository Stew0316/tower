using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    public CommonTools commonTools;
    public int HP = 200;
    private void Awake()
    {
        commonTools = GetComponent<CommonTools>();
        if (commonTools == null)
        {
            commonTools = gameObject.AddComponent<CommonTools>();
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Global.Instance.Health += HP;
        commonTools.Play();
    }
}
