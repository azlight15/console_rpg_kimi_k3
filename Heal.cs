using System;

namespace Console_RPG;

public static class Heal
{
    // 纯逻辑治疗，返回实际恢复量。
    public static double ApplyHeal()
    {
        return PlayerStatistics.Heal(PlayerStatistics.Treatment);
    }

    // 保留旧入口，避免外部调用一次性改完。
    public static void _Heal()
    {
        double healed = ApplyHeal();
        Console.Clear();
        Console.WriteLine($"你治疗了自己，恢复 {healed} HP");
        Program.Loading();
    }
}
