// ==========================
// 对战系统
// 负责处理玩家与怪物之间的完整战斗流程
// ==========================

using System;

namespace Console_RPG;

/*
    Battle 模块负责控制战斗入口、战斗流程与战斗结果结算。
    包括刷怪循环、战斗操作、胜负判断以及经验奖励。
*/
public static class Battle
{
    private enum BattleResult
    {
        Victory,
        Retreat,
        Defeat
    }

    public static void StartBattle()
    {
        if (PlayerStatistics.Hp <= 0)
        {
            Console.WriteLine("你的HP不足，帮你回到选择页面");
            Program.Loading();
            return;
        }

        while (PlayerStatistics.Hp > 0)
        {
            var monster = MonsterFactory.Monster;
            BattleResult result = Fight(monster);

            if (result == BattleResult.Defeat)
            {
                Console.WriteLine("你回到了营地，治疗后再来挑战吧。");
                Program.Loading();
                return;
            }

            Console.WriteLine("是否继续刷怪？输入 Y 继续，其他任意输入返回菜单：");
            string? choice = Console.ReadLine();
            if (!string.Equals(choice?.Trim(), "Y", StringComparison.OrdinalIgnoreCase))
            {
                Program.Loading();
                return;
            }
        }
    }

    private static BattleResult Fight(MonsterStatistics monster)
    {
        Console.Clear();
        Console.WriteLine($"你遇到了 {monster.Name}");
        Console.WriteLine($"等级：{monster.Level}");
        Console.WriteLine($"血量 {monster.Hp}/{monster.MaxHp}");
        Console.WriteLine($"攻击力 {monster.Attack}");
        Console.WriteLine($"预计获得经验值：{monster.ExpReward}");
        Console.WriteLine("按回车进入战斗！");
        Console.ReadLine();

        while (PlayerStatistics.Hp > 0 && monster.Hp > 0)
        {
            Console.Clear();
            Console.WriteLine($"{PlayerStatistics.Name}  Lv.{PlayerStatistics.Level}");
            Console.WriteLine($"HP：{PlayerStatistics.Hp}/{PlayerStatistics.MaxHp}");
            Console.WriteLine("=======================");
            Console.WriteLine($"{monster.Name}  Lv.{monster.Level}");
            Console.WriteLine($"HP：{monster.Hp}/{monster.MaxHp}");
            Console.WriteLine("=======================");
            Console.WriteLine("普通攻击（A）| 治疗（D）| 撤退（F）");
            Console.Write("请选择行动：");

            string? action = Console.ReadLine();
            bool retreated = false;

            switch (action?.Trim().ToUpperInvariant())
            {
                case "A":
                    monster.Hp = Math.Max(0, monster.Hp - PlayerStatistics.Attack);
                    Console.WriteLine($"你攻击了 {monster.Name}，造成 {PlayerStatistics.Attack} 点伤害！");
                    break;
                case "D":
                    double healed = PlayerStatistics.Heal(PlayerStatistics.Treatment);
                    Console.WriteLine($"你治疗了自己，恢复 {healed} HP");
                    break;
                case "F":
                    Console.WriteLine("你选择了撤退");
                    retreated = true;
                    break;
                default:
                    Console.WriteLine("无效操作，本回合你犹豫了一下。");
                    break;
            }

            if (retreated)
            {
                return BattleResult.Retreat;
            }

            if (monster.Hp <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"你击败了 {monster.Name}！");
                Console.ResetColor();
                UpLevel.GainExp(monster.ExpReward);
                return BattleResult.Victory;
            }

            double damage = PlayerStatistics.TakeDamage(monster.Attack - PlayerStatistics.Level);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{monster.Name} 反击你，造成 {damage} 点伤害");
            Console.ResetColor();

            if (PlayerStatistics.Hp <= 0)
            {
                PlayerStatistics.Exp = Math.Max(0, PlayerStatistics.Exp * 0.9);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("你倒下了……经验值损失 10%。");
                Console.ResetColor();
                return BattleResult.Defeat;
            }

            Console.WriteLine("按回车进入下一回合");
            Console.ReadLine();
        }

        return PlayerStatistics.Hp <= 0 ? BattleResult.Defeat : BattleResult.Retreat;
    }
}
