using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enums;
using static Key;
using UnityEngine.Events;
public class Gem : MonoBehaviour
{
    [SerializeField]
    public CommonTools commonTools;
    public GemType gemType;
    private Dictionary<GemType, UnityAction> run;
    public int Count = 3;

    private void Awake()
    {
        commonTools = GetComponent<CommonTools>();
        if (commonTools == null)
        {
            commonTools = gameObject.AddComponent<CommonTools>();
        }
        run = new Dictionary<GemType, UnityAction>();
        run.Add(GemType.Red, () =>
        {
            Global.Instance.Attack += Count;
        });
        run.Add(GemType.Blue, () =>
        {
            Global.Instance.Defense += Count;
        });

    }
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
        run[gemType]();
        commonTools.InitFade();
        commonTools.FadeOut();

    }
}
