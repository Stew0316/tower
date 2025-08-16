using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Key : MonoBehaviour
{
    [SerializeField]
    public CommonTools commonTools;
    public enum KeyType { Yellow, Blue, Red }
    public KeyType keyType;
    private Dictionary<KeyType, UnityAction> run;
    private void Awake()
    {
        run = new Dictionary<KeyType, UnityAction>();
        run.Add(KeyType.Red, () =>
        {
            Global.Instance.KEY_R++;
        });
        run.Add(KeyType.Yellow, () =>
        {
            Global.Instance.KEY_Y++;
        });
        run.Add(KeyType.Blue, () =>
        {
            Global.Instance.KEY_B++;
        });
        commonTools = GetComponent<CommonTools>();
        if (commonTools == null)
        {
            commonTools = gameObject.AddComponent<CommonTools>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        run[keyType]();
        commonTools.Play();
    }

}
