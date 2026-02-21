// 用于存储游戏中需要保存和加载的数据
public class GameData
{
    // ---- 背包数据 -----
    // 金钱
    public int money;

    // 物品
    public SerializableDictionary<string, int> itemsDict;

    public GameData()
    {
        itemsDict = new SerializableDictionary<string, int>();
    }
}
