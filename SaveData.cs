// ==========================
// 存档系统
// 负责将玩家当前状态保存为 JSON 文件，并在下次启动时读取恢复
// ==========================

using System;
using System.IO;
using System.Text.Json;

namespace Console_RPG;

/*
    SaveData 是用于序列化存档的数据结构。
    它从 PlayerStatistics 复制数据，用来写入 JSON 文件。
    读取存档时再把数据还原回 PlayerStatistics。
*/
public class SaveData
{
    public string Name { get; set; } = "";   // 玩家名字
    public int Level { get; set; }           // 等级
    public double Exp { get; set; }          // 当前经验值
    public double Hp { get; set; }           // 当前血量
    public double MaxHp { get; set; }        // 最大血量
    public double Attack { get; set; }       // 攻击力
    public double Treatment { get; set; }    // 治疗值
}

/*
    SaveManager 负责存档的保存与读取。
    将玩家数据序列化为 JSON 文件，或从 JSON 文件中恢复数据。
*/
public static class SaveManager
{
    // 默认存档文件名；测试时可改到临时目录。
    public static string SavePath { get; set; } = "save.json";

    public static SaveData Snapshot()
    {
        return new SaveData
        {
            Name = PlayerStatistics.Name,
            Level = PlayerStatistics.Level,
            Exp = PlayerStatistics.Exp,
            Hp = PlayerStatistics.Hp,
            MaxHp = PlayerStatistics.MaxHp,
            Attack = PlayerStatistics.Attack,
            Treatment = PlayerStatistics.Treatment
        };
    }

    public static void Restore(SaveData data)
    {
        PlayerStatistics.Name = string.IsNullOrWhiteSpace(data.Name) ? "勇者" : data.Name;
        PlayerStatistics.Level = Math.Max(1, data.Level);
        PlayerStatistics.Exp = Math.Max(0, data.Exp);
        PlayerStatistics.MaxHp = Math.Max(1, data.MaxHp);
        PlayerStatistics.Hp = Math.Clamp(data.Hp, 0, PlayerStatistics.MaxHp);
        PlayerStatistics.Attack = Math.Max(1, data.Attack);
        PlayerStatistics.Treatment = Math.Max(0, data.Treatment);
    }

    public static bool TrySave(out string error)
    {
        try
        {
            string json = JsonSerializer.Serialize(Snapshot(), new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SavePath, json);
            error = "";
            return true;
        }
        catch (Exception ex) when (ex is IOException or JsonException or UnauthorizedAccessException)
        {
            error = ex.Message;
            return false;
        }
    }

    public static bool TryLoad(out string error)
    {
        error = "";

        if (!File.Exists(SavePath))
        {
            error = "没有找到存档文件";
            return false;
        }

        try
        {
            string json = File.ReadAllText(SavePath);
            SaveData? data = JsonSerializer.Deserialize<SaveData>(json);
            if (data is null)
            {
                error = "存档内容为空";
                return false;
            }

            Restore(data);
            return true;
        }
        catch (Exception ex) when (ex is IOException or JsonException or UnauthorizedAccessException)
        {
            error = ex.Message;
            return false;
        }
    }

    // 控制台交互入口。
    public static void Save()
    {
        Console.WriteLine(TrySave(out string error) ? "游戏已保存" : $"保存失败：{error}");
        Program.Loading();
    }

    public static void Load()
    {
        Console.WriteLine(TryLoad(out string error) ? "存档读取成功！" : error);
        Program.Loading();
    }
}
