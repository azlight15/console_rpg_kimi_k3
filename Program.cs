using System;

namespace Console_RPG;

/*
    Program 是游戏的入口类。
    负责初始化游戏、显示菜单，并控制整个游戏主循环。
*/
public static class Program
{
    private static bool _running = true;
    private static bool _selfTest;

    private static void Main(string[] args)
    {
        _selfTest = Array.IndexOf(args, "--self-test") >= 0;
        if (_selfTest)
        {
            SelfTest.Run();
            return;
        }

        StartMenu();
        GameConfirmed();

        while (_running)
        {
            OptionsMenu();
        }
    }

    private static void StartMenu()
    {
        Console.Clear();
        Console.WriteLine("===== 欢迎来玩Console RPG游戏 =====");
        Console.WriteLine("此游戏是控制台游戏，没有ui");
        Console.WriteLine("那么接下来请好好享受游戏吧！");
        Console.WriteLine("=================================");
        Console.Write("请输入你的名字（取了名字后不能更改！）：");

        PlayerStatistics.Name = Console.ReadLine() ?? "";

        while (string.IsNullOrWhiteSpace(PlayerStatistics.Name))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n不能只输入空格或直接回车！请重新输入你的名字：");
            Console.ResetColor();
            PlayerStatistics.Name = Console.ReadLine() ?? "";
        }
    }

    private static void GameConfirmed()
    {
        Console.Clear();
        Console.WriteLine("=================================");
        Console.WriteLine("正式游玩之前先看一下玩家状态");
        Console.WriteLine($"你的名字是：{PlayerStatistics.Name}");
        Console.WriteLine($"等级：{PlayerStatistics.Level}");
        Console.WriteLine($"HP：{PlayerStatistics.Hp}/{PlayerStatistics.MaxHp}");
        Console.WriteLine($"攻击值：{PlayerStatistics.Attack}");
        Console.WriteLine($"那么祝你玩的开心，{PlayerStatistics.Name}勇者！");
        Console.WriteLine("=================================");
        Console.WriteLine("按下回车开始游戏");
        Console.ReadLine();
    }

    private static void OptionsMenu()
    {
        Console.Clear();
        Console.WriteLine("=========================");
        Console.WriteLine("Console RPG");
        Console.WriteLine("请选择选项：");
        Console.WriteLine("1.开始对战");
        Console.WriteLine("2.升级");
        Console.WriteLine("3.治疗");
        Console.WriteLine("4.查看状态");
        Console.WriteLine("5.存档");
        Console.WriteLine("6.读档");
        Console.WriteLine("7.退出游戏");
        Console.WriteLine("==========================");

        switch (ReadMenuChoice())
        {
            case 1:
                Battle.StartBattle();
                break;
            case 2:
                UpLevel.GainExp(100);
                break;
            case 3:
                Heal._Heal();
                break;
            case 4:
                ShowStatus.Show();
                break;
            case 5:
                SaveManager.Save();
                break;
            case 6:
                SaveManager.Load();
                break;
            case 7:
                Console.Clear();
                Console.WriteLine("欢迎再次玩Console RPG，谢谢");
                Console.WriteLine("那么下次再见，勇者！");
                _running = false;
                break;
        }
    }

    private static int ReadMenuChoice()
    {
        while (true)
        {
            Console.Write("请选择选项：");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int option) && option is >= 1 and <= 7)
            {
                return option;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("输入错误，请输入 1 到 7 的数字。");
            Console.ResetColor();
        }
    }

    public static void Loading()
    {
        if (_selfTest)
        {
            return;
        }

        Console.WriteLine("按下回车回到选择页面");
        Console.ReadLine();
    }
}
