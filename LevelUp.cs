using System;

namespace Console_RPG;

/*
    UpLevel 负责管理玩家经验获取与升级逻辑。
    ApplyExp 是纯逻辑：加经验、连续升级、刷新属性，并返回升级次数。
    GainExp 负责控制台提示，便于战斗和菜单调用。
*/
public static class UpLevel
{
    // 纯逻辑版本：不读写控制台，方便自测。
    public static int ApplyExp(double exp)
    {
        if (exp <= 0)
        {
            return 0;
        }

        PlayerStatistics.Exp += exp;
        int levelUps = 0;

        while (PlayerStatistics.Exp >= PlayerStatistics.ExpToNextLevel)
        {
            PlayerStatistics.Exp -= PlayerStatistics.ExpToNextLevel;
            PlayerStatistics.Level++;
            PlayerStatistics.MaxHp += 20;
            PlayerStatistics.Attack += 5;
            PlayerStatistics.Hp = PlayerStatistics.MaxHp;
            levelUps++;
        }

        return levelUps;
    }

    // 控制台交互版本。
    public static void GainExp(double exp)
    {
        Console.WriteLine($"获得经验 {exp} 点");
        int levelUps = ApplyExp(exp);

        for (int i = 0; i < levelUps; i++)
        {
            Console.WriteLine();
            Console.WriteLine("升级了！");
            Console.WriteLine($"当前等级：{PlayerStatistics.Level}");
            Console.WriteLine("最大HP +20\n攻击值 +5\nHP已回满");
        }

        if (levelUps > 0)
        {
            Program.Loading();
        }
    }
}
