using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpFloor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Global.Instance.Level_Name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        FadeManager fadeManager = FindObjectOfType<FadeManager>();
        fadeManager.StartFadeOut("level-2");
        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log("当前场景名称: " + sceneName);
        Debug.Log("英雄升层");
    }
}
