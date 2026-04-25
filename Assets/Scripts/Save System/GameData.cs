// 用于存储游戏中需要保存和加载的数据
using UnityEngine;

public class GameData
{
    // ---- 背包数据 -----
    // 金钱
    public int money;
    // 物品
    public SerializableDictionary<string, int> inventoryDict;
    // 装备
    public SerializableDictionary<string, int> equipmentDict;

    // ---- 仓库 ----
    // 仓库
    public SerializableDictionary<string, int> storageDict;
    // 材料贮藏室
    public SerializableDictionary<string, int> stashDict;

    // 存档点
    public SerializableDictionary<string, bool> unlockedCheckpointDict;

    public GameData()
    {
        inventoryDict = new SerializableDictionary<string, int>();
        equipmentDict = new SerializableDictionary<string, int>();

        storageDict = new SerializableDictionary<string, int>();
        stashDict = new SerializableDictionary<string, int>();

        unlockedCheckpointDict = new SerializableDictionary<string, bool>();
    }
}
