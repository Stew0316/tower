using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpFloor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        FadeManager fadeManager = FindObjectOfType<FadeManager>();
        string sceneName = SceneManager.GetActiveScene().name;
        string level = sceneName.Split('-')[1];
        int levelVal = int.Parse(level);
        Global.Instance.LevelValue++;
        fadeManager.StartFadeOut($"level-{levelVal + 1}");
    }
}
