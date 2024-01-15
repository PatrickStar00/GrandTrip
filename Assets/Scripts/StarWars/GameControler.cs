using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameControler
{
    private static bool m_IsInited = false;
    private static bool m_IsPaused = false;

    public static bool IsInited { get => m_IsInited; }
    public static bool IsPaused { get => m_IsPaused; }

    public static void TickGame()
    {
        float data = Time.deltaTime;



    }



}
