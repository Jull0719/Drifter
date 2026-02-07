using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string fullPath;
    private bool shouldEncrypt;
    private string encrpytCode = "drifter";

    public FileDataHandler(string savePath, string fileName, bool shouldEncrypt)
    {
        fullPath = Path.Combine(savePath, fileName);
        this.shouldEncrypt = shouldEncrypt;
    }

    /// <summary>
    /// 存储游戏数据
    /// </summary>
    /// <param name="gameData">存储的游戏数据</param>
    public void SaveData(GameData gameData)
    {
        try
        {
            // 指定路径中如果不存在目录，则创建
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // 将要存储的数据转换为Json格式
            string dataToSave = JsonUtility.ToJson(gameData, true);

            if (shouldEncrypt)
                dataToSave = EncryptDecrypt(dataToSave);

            // 打开/创建文件
            using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
            {
                // 文件中写入Json数据
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.Write(dataToSave);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log($"存储数据到{fullPath}时发生错误：{e}");
        }
    }

    /// <summary>
    /// 加载游戏数据
    /// </summary>
    /// <returns>加载的游戏数据</returns>
    public GameData LoadData()
    {
        GameData loadData = null;

        // 检查保存的文件是否存在，存在则读取数据
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                // 打开文件
                using (FileStream fileStream = new FileStream(fullPath, FileMode.Open))
                {
                    // 读取数据
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (shouldEncrypt)
                    dataToLoad = EncryptDecrypt(dataToLoad);

                // 将读取的Json数据转换为GameData
                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.Log($"从{fullPath}读取数据时，发生错误：{e}");
            }
        }

        return loadData;
    }

    /// <summary>
    /// 删除游戏数据
    /// </summary>
    public void DeleteData()
    {
        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }

    // 数据加密和解密方法
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";

        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encrpytCode[i % encrpytCode.Length]);
        }

        return modifiedData;
    }
}
