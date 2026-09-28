# 开发笔记

## 当前结构

```text
Program.cs        入口/菜单/主循环；支持 --self-test
Battle.cs         战斗流程：遇怪 -> 回合行动 -> 胜/负/撤退
MonsterFactory.cs 怪物生成：按玩家等级缩放，10% 精英
LevelUp.cs        ApplyExp 纯逻辑 + GainExp 控制台提示
SaveData.cs       Snapshot/Restore/TrySave/TryLoad + Save/Load 交互入口
SelfTest.cs       冒烟测试，dotnet run -- --self-test
```

## 这次 Kimi K3 已处理

- 菜单输入从 `Convert.ToInt32` 改为 `int.TryParse` 循环，输入字母/空行不再崩溃。
- 战斗输入从 `ReadKey` 改为 `ReadLine`，避免回车残留导致下一回合被跳过。
- 战败不再原地满血复活导致无限刷怪；现在 HP 归零、损失 10% 经验并退出战斗。
- 升级去掉“双重 while”嵌套，拆出可测试的 `UpLevel.ApplyExp`。
- 治疗/承伤/存档读档做了边界收敛：HP 不超过上限、最低伤害 1、读档校验空存档和非法数值。
- 新增 `dotnet run -- --self-test` 冒烟测试。
- 新增 `.gitignore`，忽略 `bin/`、`obj/`、`save.json`。

## 已知待做

- 装备系统、技能系统、更多怪物与掉落。
- 战斗平衡还需要调参；精英怪当前固定 1.5 倍。
- 存档目前只有一个槽位，未来可做多槽位和版本号迁移。
- 如果引入 xUnit/NUnit，需要恢复 NuGet 网络或 CI 缓存；当前自测故意零依赖。
