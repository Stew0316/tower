using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 轻量化弹窗：
/// - 两个可变化的数值（valueText1 / valueText2）
/// - 一个帧动画区域（frameAnimator 或 frameImage）
/// - 四个“静态参数”由调用方一次性传入（不会在弹窗生命周期内改变）
/// - 当任一动态数值 < 0 时触发完成回调并关闭
/// </summary>
public class PopupDialog : MonoBehaviour
{
    [Header("Fixed UI References")]
    public GameObject root;
    public CanvasGroup canvasGroup;

    [Header("Changing Values (fixed positions)")]
    public TextMeshProUGUI valueText1; // 变化值 1
    public TextMeshProUGUI valueText2; // 变化值 2

    [Header("Frame Animator")]
    public Image frameImage; // 若不使用 FrameAnimator，可直接设置 sprite
    public FrameAnimator frameAnimator; // 可选（FrameAnimator 内支持固定间隔播放）

    // 由外部传入的一次性静态参数（不会在弹窗内部被修改）
    private PopupStaticData staticData;

    private Action onComplete;
    private float internalValue1;
    private float internalValue2;

    private bool opened = false;

    void Awake()
    {
        HideImmediate();
    }

    /// <summary>
    /// 显示弹窗
    /// </summary>
    /// <param name="initialValue1">变化值1初始值</param>
    /// <param name="initialValue2">变化值2初始值</param>
    /// <param name="frames">帧数组（可为 null）</param>
    /// <param name="fps">帧率（仅当 FrameAnimator 使用 deltaTime 模式时生效）</param>
    /// <param name="staticData">调用方传入的不变参数集合（4个值）</param>
    /// <param name="onComplete">完成回调（当任一 internalValue < 0 时触发）</param>
    public void Show(float initialValue1, float initialValue2, Sprite[] frames, float fps, PopupStaticData staticData, Action onComplete = null)
    {
        this.onComplete = onComplete;
        this.staticData = staticData;
        this.internalValue1 = initialValue1;
        this.internalValue2 = initialValue2;

        UpdateValueTexts();

        if (frameAnimator != null)
            frameAnimator.Play(frames, fps);
        else if (frameImage != null && frames != null && frames.Length > 0)
            frameImage.sprite = frames[0];

        ShowImmediate();
        opened = true;

        CheckInternalValues();
    }

    /// <summary>
    /// 获取静态参数（只读）
    /// </summary>
    public PopupStaticData GetStaticData()
    {
        return staticData;
    }

    public void SetInternalValue1(float v)
    {
        internalValue1 = v;
        UpdateValueTexts();
        CheckInternalValues();
    }

    public void SetInternalValue2(float v)
    {
        internalValue2 = v;
        UpdateValueTexts();
        CheckInternalValues();
    }

    public void ModifyInternalValue1(float delta)
    {
        internalValue1 += delta;
        UpdateValueTexts();
        CheckInternalValues();
    }

    public void ModifyInternalValue2(float delta)
    {
        internalValue2 += delta;
        UpdateValueTexts();
        CheckInternalValues();
    }

    private void UpdateValueTexts()
    {
        if (valueText1 != null)
            valueText1.text = internalValue1.ToString("F0");
        if (valueText2 != null)
            valueText2.text = internalValue2.ToString("F0");
    }

    private void CheckInternalValues()
    {
        if (internalValue1 < 0f || internalValue2 < 0f)
            Complete();
    }

    public void Complete()
    {
        if (!opened) return;
        opened = false;
        try
        {
            onComplete?.Invoke();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        CloseImmediate();
    }

    public void CloseImmediate()
    {
        HideImmediate();
        onComplete = null;
        staticData = null;
        frameAnimator?.Stop();
    }

    private void ShowImmediate()
    {
        if (root != null) root.SetActive(true);
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }

    private void HideImmediate()
    {
        if (root != null) root.SetActive(false);
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}

/// <summary>
/// 可由调用方构造并传入的“静态参数”容器。
/// 使用 object 类型可以保持灵活性（调用方按需传入任意类型），
/// 如果你想要更严格的类型约束可以把字段改成具体类型。
/// </summary>
public class PopupStaticData
{
    public object param1;
    public object param2;
    public object param3;
    public object param4;

    public PopupStaticData(object p1, object p2, object p3, object p4)
    {
        param1 = p1;
        param2 = p2;
        param3 = p3;
        param4 = p4;
    }
}

public class FrameAnimator : MonoBehaviour
{
    public Image targetImage;
    public bool loop = true;

    // 如果 useFixedInterval = true，则使用 InvokeRepeating 以固定间隔播放（不依赖 deltaTime）
    // 否则使用原来的 deltaTime 计时逻辑（基于 fps）
    public bool useFixedInterval = true; // 默认开启固定间隔模式
    public float frameInterval = 0.3f; // 固定间隔（秒），当 useFixedInterval=true 时生效

    private Sprite[] frames;
    private float fps = 12f; // 当 useFixedInterval=false 时生效
    private int index = 0;
    private float timer = 0f;
    private bool playing = false;

    // 可选完成回调（当 loop==false 且播放结束时会触发）
    public Action onAnimationComplete;

    /// <summary>
    /// 开始播放帧动画（兼容旧签名）。
    /// useFixedInterval 由组件的 public 字段控制。
    /// </summary>
    public void Play(Sprite[] frames, float fps)
    {
        if (frames == null || frames.Length == 0)
        {
            Stop();
            return;
        }

        this.frames = frames;
        this.fps = Mathf.Max(1f, fps);
        index = 0;
        timer = 0f;
        playing = true;

        // 先显示第一帧
        ApplyFrame();

        if (useFixedInterval)
        {
            // 确保取消之前的 Invoke
            CancelInvoke(nameof(PlayNextFrame));
            // 从下一帧开始按固定间隔触发（首次触发在 frameInterval 秒后）
            InvokeRepeating(nameof(PlayNextFrame), frameInterval, frameInterval);
        }
    }

    /// <summary>
    /// 停止播放并清理
    /// </summary>
    public void Stop()
    {
        playing = false;
        frames = null;
        CancelInvoke(nameof(PlayNextFrame));
    }

    void Update()
    {
        if (useFixedInterval) return; // 固定间隔模式不走 Update
        if (!playing || frames == null || frames.Length == 0) return;

        timer += Time.deltaTime;
        float interval = 1f / fps;
        if (timer >= interval)
        {
            timer -= interval;
            AdvanceIndexAndApply();
        }
    }

    private void PlayNextFrame()
    {
        // InvokeRepeating 调用此方法
        if (!playing || frames == null || frames.Length == 0) return;
        AdvanceIndexAndApply();
    }

    private void AdvanceIndexAndApply()
    {
        index++;
        if (index >= frames.Length)
        {
            if (loop)
            {
                index = 0;
            }
            else
            {
                index = frames.Length - 1;
                playing = false;
                // 停止定时器
                CancelInvoke(nameof(PlayNextFrame));

                // 回调通知播放完成
                try
                {
                    onAnimationComplete?.Invoke();
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }
        ApplyFrame();
    }

    private void ApplyFrame()
    {
        if (targetImage != null && frames != null && frames.Length > 0)
            targetImage.sprite = frames[index];
    }
}