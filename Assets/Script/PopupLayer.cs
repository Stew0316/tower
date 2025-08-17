using UnityEngine;
using TMPro; // ���ʹ��TextMeshPro

public class PopupLayer : MonoBehaviour
{
    [Header("UI�ı�")]
    public TextMeshProUGUI enemyAtt;
    public TextMeshProUGUI enemyDef;
    public TextMeshProUGUI heroAtt;
    public TextMeshProUGUI heroDef;
    public TextMeshProUGUI enemyHPText;
    public TextMeshProUGUI HeroHPText;

    [Header("����")]
    private Animator collisionAnimator;  // collision���󶯻�

    private float enemyHP;
    private float HeroHP;

    /// <summary>
    /// ��ʼ��������
    /// </summary>
    /// <param name="collisionObj">Collision�������ڻ�ȡAnimator</param>
    /// <param name="fixedVal1">�̶�ֵ1</param>
    /// <param name="fixedVal2">�̶�ֵ2</param>
    /// <param name="dynamicVal1">�ɱ�ֵ1</param>
    /// <param name="dynamicVal2">�ɱ�ֵ2</param>
    public void Init(GameObject collisionObj, string enemyAttack, string enemyDefense, string heroAttack, string heroDefense, float enemyHPValue, float heroHPValue)
    {
        // ���ù̶�ֵUI
        if (enemyAtt != null) enemyAtt.text = enemyAttack;
        if (enemyDef != null) enemyDef.text = enemyDefense;
        if (heroAtt != null) heroAtt.text = heroAttack;
        if (heroDef != null) heroDef.text = heroDefense;
        PopupChange(true);
        // ��ʼ���ɱ�ֵ
        enemyHP = enemyHPValue;
        HeroHP = heroHPValue;
        UpdateDynamicUI();

        // ��ȡ collision Animator �����Ŷ���
        collisionAnimator = collisionObj.GetComponent<Animator>();
        if (collisionAnimator != null)
        {
            collisionAnimator.Play("CollisionAnimationName");
        }

        // ����Ƿ���Ҫ�����ر�
        CheckClose();
    }

    /// <summary>
    /// ���¿ɱ�ֵ
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
        // ���Լӵ���������������
        gameObject.SetActive(status);
    }

    private void Awake()
    {
        PopupChange();
    }
}
