using UnityEngine;
using TMPro; // 如果使用TextMeshPro

public class PopupLayer : MonoBehaviour
{
    [Header("UI文本")]
    public TextMeshProUGUI enemyAtt;
    public TextMeshProUGUI enemyDef;
    public TextMeshProUGUI heroAtt;
    public TextMeshProUGUI heroDef;
    public TextMeshProUGUI enemyHPText;
    public TextMeshProUGUI HeroHPText;

    [Header("动画")]
    private Animator collisionAnimator;  // collision对象动画

    private float enemyHP;
    private float HeroHP;

    /// <summary>
    /// 初始化弹出层
    /// </summary>
    /// <param name="collisionObj">Collision对象，用于获取Animator</param>
    /// <param name="fixedVal1">固定值1</param>
    /// <param name="fixedVal2">固定值2</param>
    /// <param name="dynamicVal1">可变值1</param>
    /// <param name="dynamicVal2">可变值2</param>
    public void Init(GameObject collisionObj, string enemyAttack, string enemyDefense, string heroAttack, string heroDefense, float enemyHPValue, float heroHPValue)
    {
        // 设置固定值UI
        if (enemyAtt != null) enemyAtt.text = enemyAttack;
        if (enemyDef != null) enemyDef.text = enemyDefense;
        if (heroAtt != null) heroAtt.text = heroAttack;
        if (heroDef != null) heroDef.text = heroDefense;
        PopupChange(true);
        // 初始化可变值
        enemyHP = enemyHPValue;
        HeroHP = heroHPValue;
        UpdateDynamicUI();

        // 获取 collision Animator 并播放动画
        collisionAnimator = collisionObj.GetComponent<Animator>();
        if (collisionAnimator != null)
        {
            collisionAnimator.Play("CollisionAnimationName");
        }

        // 检查是否需要立即关闭
        CheckClose();
    }

    /// <summary>
    /// 更新可变值
    /// </summary>
    public void UpdateDynamicValues(float enemyHPValue, float heroHPValue)
    {
        enemyHP = enemyHPValue;
        HeroHP = heroHPValue;
        UpdateDynamicUI();
        CheckClose();
    }

    private void UpdateDynamicUI()
    {
        if (enemyHPText != null)
            enemyHPText.text = enemyHP.ToString();
        if (HeroHPText != null)
            HeroHPText.text = HeroHP.ToString();
    }

    private void CheckClose()
    {
        if (enemyHP < 0 || HeroHP < 0)
        {
            PopupChange();
        }
    }

    private void PopupChange(bool status = false)
    {
        // 可以加淡出动画后再销毁
        gameObject.SetActive(status);
    }

    private void Awake()
    {
        PopupChange();
    }
}
