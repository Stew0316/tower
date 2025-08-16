using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
[RequireComponent(typeof(CanvasGroup))]
public class CommonTools : MonoBehaviour
{
    [Header("拾取动画设置")]
    public float sinkSpeed = 2f;
    public float sinkDistance = 1f;
    public AnimationCurve sinkCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Fade Settings")]
    [Tooltip("淡入/淡出时长（秒）")]
    public float duration = 0.3f;

    [Header("Events")]
    public UnityEvent onFadeInComplete;
    public UnityEvent onFadeOutComplete;

    private CanvasGroup cg;
    private Coroutine currentCoroutine;

    private bool isBeingCollected = false;

    public void InitFade(bool isShow = true)
    {
        cg = GetComponent<CanvasGroup>();
        // 默认隐藏状态
        cg.alpha = isShow ? 1f : 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;
        gameObject.SetActive(isShow);
    }

    public void FadeIn()
    {
        // 如果正在淡出，先停止
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        gameObject.SetActive(true);
        currentCoroutine = StartCoroutine(Fade(0f, 1f, () =>
        {
            cg.interactable = true;
            cg.blocksRaycasts = true;
            onFadeInComplete?.Invoke();
        }));
    }

    public void FadeOut()
    {
        // 如果正在淡入，先停止
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        cg.interactable = false;
        cg.blocksRaycasts = false;
        currentCoroutine = StartCoroutine(Fade(1f, 0f, () =>
        {
            gameObject.SetActive(false);
            onFadeOutComplete?.Invoke();
        }));
    }

    private IEnumerator Fade(float from, float to, UnityAction onComplete)
    {
        float elapsed = 0f;
        cg.alpha = from;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        cg.alpha = to;
        onComplete?.Invoke();
    }

    // Start is called before the first frame update
    public void Play()
    {
        if (!isBeingCollected)
        {
            isBeingCollected = true;
            StartCoroutine(AnimatePickup());
        }
    }

    // Update is called once per frame
    private IEnumerator AnimatePickup()
    {
        // 禁用碰撞器
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) yield break;

        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + Vector3.down * sinkDistance;
        Color startColor = sr.color;

        float timer = 0f;
        float duration = sinkDistance / sinkSpeed;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            float curveValue = sinkCurve.Evaluate(t);

            transform.position = Vector3.Lerp(startPos, targetPos, curveValue);
            Color newColor = startColor;
            newColor.a = Mathf.Lerp(1f, 0f, t);
            sr.color = newColor;

            yield return null;
        }

        Destroy(gameObject);
    }
}
