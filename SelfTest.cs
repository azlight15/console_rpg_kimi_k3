using System;
using System.IO;

namespace Console_RPG;

// 无外部依赖的冒烟测试：dotnet run -- --self-test
public static class SelfTest
{
    public static void Run()
    {
        int failed = 0;

        Check("初始状态", PlayerStatistics.Level == 1 && PlayerStatistics.Hp == 100, ref failed);

        PlayerStatistics.Reset("测试");
        int ups = UpLevel.ApplyExp(300);
        Check("连续升级", ups == 2 && PlayerStatistics.Level == 3 && PlayerStatistics.Exp == 0, ref failed);

        PlayerStatistics.Reset("治疗");
        PlayerStatistics.Hp = 10;
        double healed = Heal.ApplyHeal();
        Check("治疗上限", healed == 50 && PlayerStatistics.Hp == 60, ref failed);

        PlayerStatistics.Reset("承伤");
        double damage = PlayerStatistics.TakeDamage(0);
        Check("最低伤害", damage == 1 && PlayerStatistics.Hp == 99, ref failed);

        string temp = Path.Combine(Path.GetTempPath(), "console_rpg_selftest_save.json");
        SaveManager.SavePath = temp;
        PlayerStatistics.Reset("存档");
        PlayerStatistics.Level = 5;
        PlayerStatistics.Hp = 44;
        Check("保存", SaveManager.TrySave(out _), ref failed);
        PlayerStatistics.Reset("覆盖");
        Check("读取", SaveManager.TryLoad(out _) && PlayerStatistics.Name == "存档" && PlayerStatistics.Level == 5 && PlayerStatistics.Hp == 44, ref failed);

        Console.WriteLine(failed == 0 ? "SELFTEST PASS" : $"SELFTEST FAIL: {failed}");
        Environment.ExitCode = failed == 0 ? 0 : 1;
    }

    private static void Check(string name, bool ok, ref int failed)
    {
        Console.WriteLine($"{(ok ? "PASS" : "FAIL")} {name}");
        if (!ok)
        {
            failed++;
        }
    }
}
