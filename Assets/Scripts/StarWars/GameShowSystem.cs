using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏展示系统
/// </summary>
public class GameShowSystem
{

    internal static GameShowSystem Instance
    {
        get
        {
            return s_Instance;
        }
    }
    private static GameShowSystem s_Instance = new GameShowSystem();


    private GameShowSystem() { }

    public static void Tick(float delata)
    {

    }
}
