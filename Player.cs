using System;

namespace Console_RPG;

/*
    PlayerStatistics 用于保存玩家的全局状态数据。
    包含等级、经验、血量、攻击力等核心属性。
    所有战斗、升级、治疗、存档模块都会直接读取或修改这里的数据。
*/
public static class PlayerStatistics
{
    public static string Name = null!;   // 玩家名字
    public static int Level = 1;         // 玩家等级
    public static double Exp = 0;        // 当前经验值

    // 当前等级升级所需经验
    public static double ExpToNextLevel => Level * 100;

    public static double Hp = 100;       // 当前血量
    public static double MaxHp = 100;    // 最大血量
    public static double Attack = 15;    // 攻击力
    public static double Treatment = 50; // 每次治疗恢复的血量

    // 恢复初始状态，供新开局或自测使用。
    public static void Reset(string name = "勇者")
    {
        Name = name;
        Level = 1;
        Exp = 0;
        MaxHp = 100;
        Hp = MaxHp;
        Attack = 15;
        Treatment = 50;
    }

    // 统一回血入口，避免各模块重复写上限判断。
    public static double Heal(double amount)
    {
        double before = Hp;
        Hp = Math.Min(MaxHp, Hp + amount);
        return Hp - before;
    }

    // 统一扣血入口，保证最低伤害为 1，且 HP 不小于 0。
    public static double TakeDamage(double rawDamage)
    {
        double damage = Math.Max(1, rawDamage);
        Hp = Math.Max(0, Hp - damage);
        return damage;
    }
}
