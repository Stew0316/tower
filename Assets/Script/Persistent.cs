using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    private static PersistentObject _instance;

    void Awake()
    {
        Debug.Log($"[PersistentDebug] Awake on {name}, root = {transform.root.name}");
        // 防止重复（如果场景里后续再次有同名 prefab 被实例化）
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        // 将整个根对象标记为在场景切换时不被销毁
        DontDestroyOnLoad(gameObject);
    }
}