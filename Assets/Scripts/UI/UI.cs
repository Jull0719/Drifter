using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public void StartButton()
    {
        Debug.Log("开始游戏");
        // 加载场景1

    }

    public void QuitButton()
    {
        Debug.Log("退出游戏");
        Application.Quit();
    }
}
