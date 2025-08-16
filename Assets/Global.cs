using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Game.Enums;

// 将原有的 Global 脚本替换为基于 Stat/字典 的实现，便于扩展与 UI 绑定
// 说明：
// - 在 Inspector 中可以看到各个 Stat 的初始值（序列化的 Stat 字段）
// - 通过 StatBinding 在 Inspector 里把某个 TextMeshProUGUI 绑定到某个 Stat
// - 通过 Stat.OnChanged 订阅刷新 UI（也会在 Awake 时立即刷新一次）

[Serializable]
public enum StatType
{
    KeyYellow,
    KeyRed,
    KeyBlue,
    Level,
    Health,
    Attack,
    Defense,
    Coin,
    Exp
}

[Serializable]
public class Stat
{
    [SerializeField]
    private int _value;

    public int Value
    {
        get => _value;
        set
        {
            if (_value == value) return;
            _value = value;
            OnChanged?.Invoke(_value);
        }
    }

    // 变更事件（订阅者可刷新 UI、保存、同步等）
    public event Action<int> OnChanged;

    public void Add(int delta) => Value = _value + delta;
}

[Serializable]
public class StatBinding
{
    public StatType statType;
    public TextMeshProUGUI text; // 在 Inspector 中关联对应的 TMP 文本
}

public class Global : MonoBehaviour
{
    static Global _instance;
    public static Global Instance => _instance;
    public bool isPaused = false;
    public int this[StatType t]
    {
        get => GetStat(t);
        set => SetStat(t, value);
    }

    [Header("初始状态（可在 Inspector 修改）")]
    public Stat keyYellow = new Stat() { Value = 0 };
    public Stat keyRed = new Stat() { Value = 1 };
    public Stat keyBlue = new Stat() { Value = 1 };
    public Stat level = new Stat() { Value = -1 };
    public Stat health = new Stat() { Value = 1000 };
    public Stat attack = new Stat() { Value = 10 };
    public Stat defense = new Stat() { Value = 10 };
    public Stat coin = new Stat() { Value = 0 };
    public Stat exp = new Stat() { Value = 0 };

    [Header("UI 绑定：在 Inspector 把 TextMeshProUGUI 拖到对应 StatBinding 上")]
    public StatBinding[] uiBindings;

    // 内部字典：方便通过枚举访问
    private Dictionary<StatType, Stat> stats;

    // 门 -> StatType 的映射（钥匙对应）
    private Dictionary<DoorType, StatType> doorToStat = new Dictionary<DoorType, StatType>()
    {
        { DoorType.Red, StatType.KeyRed },
        { DoorType.Yellow, StatType.KeyYellow },
        { DoorType.Blue, StatType.KeyBlue }
    };

    // 宝石处理器（可扩展）
    private Dictionary<GemType, Action<int>> gemHandlers;

    private void Awake()
    {
        // 单例处理
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 构建 stats 字典
        stats = new Dictionary<StatType, Stat>()
        {
            { StatType.KeyYellow, keyYellow },
            { StatType.KeyRed, keyRed },
            { StatType.KeyBlue, keyBlue },
            { StatType.Level, level },
            { StatType.Health, health },
            { StatType.Attack, attack },
            { StatType.Defense, defense },
            { StatType.Coin, coin },
            { StatType.Exp, exp }
        };

        // UI 绑定：订阅变更并立即刷新显示初始值
        if (uiBindings != null)
        {
            foreach (var b in uiBindings)
            {
                if (b == null) continue;
                if (!stats.TryGetValue(b.statType, out var s)) continue;

                // 订阅事件，刷新 Text
                s.OnChanged += (val) =>
                {
                    if (b.text != null)
                        b.text.text = $"{val}";
                };

                // 立即刷新一次
                if (b.text != null)
                    b.text.text = $"{s.Value}";
            }
        }

        // 初始化宝石处理器
        gemHandlers = new Dictionary<GemType, Action<int>>()
        {
            { GemType.Red, v => stats[StatType.Attack].Add(v) },
            { GemType.Blue, v => stats[StatType.Defense].Add(v) }
            // 若还有其他 GemType，可在此加入处理逻辑
        };
    }

    #region 方便的 API（通用访问）
    public int GetStat(StatType t) => stats[t].Value;
    public void SetStat(StatType t, int val) => stats[t].Value = val;
    public void AddStat(StatType t, int delta) => stats[t].Add(delta);

    // 兼容旧字段访问
    public int KEY_Y { get => GetStat(StatType.KeyYellow); set => SetStat(StatType.KeyYellow, value); }
    public int KEY_R { get => GetStat(StatType.KeyRed); set => SetStat(StatType.KeyRed, value); }
    public int KEY_B { get => GetStat(StatType.KeyBlue); set => SetStat(StatType.KeyBlue, value); }
    public int LevelValue { get => GetStat(StatType.Level); set => SetStat(StatType.Level, value); }
    public int Health { get => GetStat(StatType.Health); set => SetStat(StatType.Health, value); }
    public int Attack { get => GetStat(StatType.Attack); set => SetStat(StatType.Attack, value); }
    public int Defense { get => GetStat(StatType.Defense); set => SetStat(StatType.Defense, value); }
    public int Coin { get => GetStat(StatType.Coin); set => SetStat(StatType.Coin, value); }
    public int Exp { get => GetStat(StatType.Exp); set => SetStat(StatType.Exp, value); }
    #endregion

    #region 门与宝石逻辑
    public bool CheckOpenDoor(DoorType doorType)
    {
        if (!doorToStat.TryGetValue(doorType, out var st)) return false;
        return stats[st].Value > 0;
    }

    public void ReduceDoor(DoorType doorType)
    {
        if (!doorToStat.TryGetValue(doorType, out var st)) return;
        stats[st].Add(-1);
    }

    public void UseGem(GemType gem, int value)
    {
        if (gemHandlers.TryGetValue(gem, out var handler)) handler(value);
    }
    #endregion

    #region UI / 辅助
    // 强制刷新所有绑定 UI（例如切换场景后需要手动刷新）
    public void UploadUI()
    {
        if (uiBindings == null) return;
        foreach (var b in uiBindings)
        {
            if (b == null) continue;
            if (!stats.TryGetValue(b.statType, out var s)) continue;
            if (b.text != null)
                b.text.text = $"{s.Value}";
        }
    }
    #endregion
}
