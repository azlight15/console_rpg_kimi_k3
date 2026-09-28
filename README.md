# Console_RPG

一个 C# 控制台回合制 RPG 小项目：起名、刷怪、升级、治疗、查看状态、JSON 存档/读档。

## 运行要求

- .NET SDK / Runtime：10.0（`Console_RPG.csproj` 中的 `TargetFramework` 为 `net10.0`）
- 代码本身没有使用 .NET 10 专属 API；如果你本机只有 .NET 8，可临时把 `TargetFramework` 改成 `net8.0` 验证。

## 运行

```bash
dotnet restore
dotnet run
```

## 冒烟自测

项目内置了无外部依赖的自测入口，适合 CI 或改完快速验证：

```bash
dotnet run -- --self-test
```

输出 `SELFTEST PASS` 表示核心逻辑通过：初始状态、连续升级、治疗上限、最低伤害、保存/读取。

## 目录说明

- `Program.cs`：入口、菜单、主循环、安全输入、`--self-test`
- `Battle.cs`：刷怪循环、回合战斗、胜负/撤退结算
- `MonsterFactory.cs`：按玩家等级随机生成怪物，含精英怪
- `Player.cs` / `Monster.cs`：玩家与怪物状态
- `LevelUp.cs`：经验与连续升级；`ApplyExp` 为纯逻辑，`GainExp` 负责提示
- `Heal.cs` / `ShowStatus.cs`：治疗与状态显示
- `SaveData.cs`：JSON 存档；`TrySave`/`TryLoad` 便于测试和错误提示
- `SelfTest.cs`：冒烟测试入口

## 协作约定

- 新功能先开分支，再提 PR；不要在 `main` 上直接大改。
- 改动后至少跑：`dotnet build` 和 `dotnet run -- --self-test`。
- 存档文件 `save.json`、`bin/`、`obj/` 不应提交。
