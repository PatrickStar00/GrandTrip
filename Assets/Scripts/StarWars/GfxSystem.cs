using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 作为逻辑层调用表现成的粘合类
/// </summary>
public class GfxSystem
{

    internal static GfxSystem Instance
    {
        get
        {
            return s_Instance;
        }
    }
    private static GfxSystem s_Instance = new GfxSystem();


    private GfxSystem() { }

    public static void Tick(float delata)
    {

    }


    private bool m_IsLastHitUi;
    private Vector3 m_LastMousePos;
    private Vector3 m_CurMousePos;
    private Vector3 m_MouseRayPoint;

    private bool[] m_KeyPressed = new bool[500];
    private bool[] m_ButtonPressed = new bool[500];

    private HashSet<int> m_KeysForListen = new HashSet<int>();

    private void HandleInput()
    {
        m_LastMousePos = m_CurMousePos;
        m_CurMousePos = Input.mousePosition;

        if ((m_CurMousePos - m_LastMousePos).sqrMagnitude >= 1 && null != Camera.main)
        {
            Ray ray = Camera.main.ScreenPointToRay(m_CurMousePos);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                m_MouseRayPoint = hitInfo.point;
            }
        }

        foreach (int c in m_KeysForListen)
        {
            if (Input.GetKeyDown((KeyCode)c))
            {
                m_KeyPressed[c] = true;
                FireKeyboard(c, (int)Keyboard.Event.Down);
            }
            else if (Input.GetKeyUp((KeyCode)c))
            {
                m_KeyPressed[c] = false;
                FireKeyboard(c, (int)Keyboard.Event.Up);
            }
        }
    }


    public static bool IsKeyPressed(KeyCode c)
    {
        return s_Instance.m_KeyPressed[(int)c];
    }

    private void FireKeyboard(int c, int e)
    {

    }
}

public static class Keyboard
{
    public enum Event
    {
        Up,
        Down,
        DoubleClick,
        LongPressed,
    }
}