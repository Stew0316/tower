using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System;
using TMPro;
public class Fairy : MonoBehaviour
{
    public GameObject dialog1 = null;
    public GameObject dialog2 = null;
    public TextMeshProUGUI textHero = null;
    public TextMeshProUGUI textFairy = null;
    public GameObject fairy = null;
    private JArray heroDialogues;
    private JArray fairyDialog;
    private int spaceCount = 0;
    private bool isDialog = false;

    // Start is called before the first frame update
    void Start()
    {
        string filePath = Path.Combine(Application.dataPath, "Script/dialog.json");
        string jsonContent = File.ReadAllText(filePath);
        JObject data = JObject.Parse(jsonContent);
        heroDialogues = (JArray)data["hero"];
        fairyDialog = (JArray)data["fairy"];
    }

    // Update is called once per frame
    void Update()
    {
        if (Global.Instance.isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (spaceCount == 0)
                {
                    dialog1.SetActive(false);
                    dialog2.SetActive(true);
                }
                if(spaceCount == 1)
                {
                    textHero.text = (string)heroDialogues[0];
                    dialog1.SetActive(true);
                    dialog2.SetActive(false);
                }
                if(spaceCount == 2)
                {
                    textFairy.text = (string)fairyDialog[0];
                    dialog1.SetActive(false);
                    dialog2.SetActive(true);
                }
                if (spaceCount == 3)
                {
                    textHero.text = (string)heroDialogues[1];
                    dialog1.SetActive(true);
                    dialog2.SetActive(false);
                }
                if (spaceCount == 4)
                {
                    textFairy.text = (string)fairyDialog[1];
                    dialog1.SetActive(false);
                    dialog2.SetActive(true);
                }
                if(spaceCount >=5)
                {

                    dialogEnd();
                }
                spaceCount++;
            }
        }

    }

    private void dialogEnd()
    {
        isDialog = true;
        dialog2.SetActive(false);
        fairy.transform.position = new Vector3(1.48f, fairy.transform.position.y, fairy.transform.position.z);
        Global.Instance.isPaused = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDialog) return;
        Global.Instance.isPaused = true;
        dialog1.SetActive(true);

    }
}
