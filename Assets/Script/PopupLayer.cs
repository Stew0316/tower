using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopupLayer : MonoBehaviour
{
    [Header("UI文本")]
    public TextMeshProUGUI enemyAtt;
    public TextMeshProUGUI enemyDef;
    public TextMeshProUGUI heroAtt;
    public TextMeshProUGUI heroDef;
    public TextMeshProUGUI enemyHPText;
    public TextMeshProUGUI HeroHPText;

    [Header("动画（场景子对象）")]
    [Tooltip("在 Inspector 中把要显示动画的子 GameObject 拖进来（scene 中的子对象）")]
    public GameObject sceneChildForVisual; // 你场景里已存在的子对象（用于显示动画）

    [Header("UI Image 方案（可选）")]
    public Image animImage; // 若使用 Image 播放序列帧则拖入
    public float frameRate = 12f;

    private GameObject spawnedVisual; // 当我们 Instantiate 时引用，用于销毁/复用
    private Coroutine playCoroutine;

    private Animator collisionAnimator;  // collision对象动画（可选保留）
    private float enemyHP;
    private float HeroHP;

    private void Awake()
    {
        PopupChange(false);
    }

    public void Init(GameObject collisionObj, string enemyAttack, string enemyDefense, string heroAttack, string heroDefense, float enemyHPValue, float heroHPValue)
    {
        // ---- 你的原 UI 赋值 ----
        if (enemyAtt != null) enemyAtt.text = enemyAttack;
        if (enemyDef != null) enemyDef.text = enemyDefense;
        if (heroAtt != null) heroAtt.text = heroAttack;
        if (heroDef != null) heroDef.text = heroDefense;
        PopupChange(true);

        enemyHP = enemyHPValue;
        HeroHP = heroHPValue;
        UpdateDynamicUI();

        // 保存 source animator（原始 collisionObj 的 controller）
        if (collisionObj != null)
            collisionAnimator = collisionObj.GetComponent<Animator>();

        // ======= 方法 1：把 runtimeAnimatorController 复制给场景中的子对象的 Animator =======
        if (sceneChildForVisual != null && collisionAnimator != null)
        {
            Animator dst = sceneChildForVisual.GetComponent<Animator>();
            if (dst == null)
            {
                // 如果目标没有 Animator，可以自动添加（如果你希望）
                dst = sceneChildForVisual.AddComponent<Animator>();
            }

            // 复制 controller（Clip、Controller 会一起复制引用）
            dst.runtimeAnimatorController = collisionAnimator.runtimeAnimatorController;

            // 选择性：让目标Animator立刻播放默认状态
            dst.Play(0, 0, 0f);

            // 如果 collisionObj 的 animator 里使用了 Avatar/IK 等复杂设置，注意检查兼容性
        }
        // ======= 方法 2：把 collisionObj 的副本实例化到场景子对象下（更“原封不动”） =======
        else if (collisionObj != null && sceneChildForVisual != null)
        {
            // 如果你想直接实例化一个 visual 副本（摆放在 sceneChildForVisual 下）
            if (spawnedVisual != null) Destroy(spawnedVisual);

            spawnedVisual = Instantiate(collisionObj, sceneChildForVisual.transform);
            spawnedVisual.transform.localPosition = Vector3.zero;
            spawnedVisual.transform.localScale = Vector3.one;

            Animator instAnim = spawnedVisual.GetComponent<Animator>();
            if (instAnim != null)
                instAnim.Play(0, 0, 0f);

            // 如果 collisionObj 带有不需要的脚本（例如控制逻辑、Rigidbody 等），你可以在这里禁用：
            // var ctrl = spawnedVisual.GetComponent<YourController>(); if (ctrl) ctrl.enabled = false;
        }

        // ======= 方法 3：若 animImage 不为 null，且 collisionObj 提供 Sprite[]（SpriteFramesProvider），则按帧播放 =======
        if (animImage != null && collisionObj != null)
        {
            SpriteFramesProvider provider = collisionObj.GetComponent<SpriteFramesProvider>();
            if (provider != null && provider.frames != null && provider.frames.Length > 0)
            {
                if (playCoroutine != null) StopCoroutine(playCoroutine);
                playCoroutine = StartCoroutine(PlaySpriteFrames(provider.frames));
            }
        }

        // 检查是否需要立即关闭
        CheckClose();
    }

    public void UpdateDynamicValues(float enemyHPValue, float heroHPValue)
    {
        enemyHP = enemyHPValue;
        HeroHP = heroHPValue;
        UpdateDynamicUI();
        CheckClose();
    }

    private void UpdateDynamicUI()
    {
        if (enemyHPText != null) enemyHPText.text = enemyHP.ToString();
        if (HeroHPText != null) HeroHPText.text = HeroHP.ToString();
    }

    private void CheckClose()
    {
        if (enemyHP < 0 || HeroHP < 0)
        {
            PopupChange(false);
        }
    }

    private void PopupChange(bool status = false)
    {
        // 可以加淡出动画后再销毁
        gameObject.SetActive(status);

        // 关闭时清理
        if (!status)
        {
            if (spawnedVisual != null) Destroy(spawnedVisual);
            if (playCoroutine != null) { StopCoroutine(playCoroutine); playCoroutine = null; }
        }
    }

    private IEnumerator PlaySpriteFrames(Sprite[] frames)
    {
        int idx = 0;
        float interval = 1f / Mathf.Max(0.01f, frameRate);
        while (true)
        {
            if (animImage != null)
                animImage.sprite = frames[idx];
            idx = (idx + 1) % frames.Length;
            yield return new WaitForSeconds(interval);
        }
    }
}

// 辅助组件（如果你用方法3，在 collisionObj 上贴这个脚本并在 Inspector 填 frames）
public class SpriteFramesProvider : MonoBehaviour
{
    public Sprite[] frames;
}
