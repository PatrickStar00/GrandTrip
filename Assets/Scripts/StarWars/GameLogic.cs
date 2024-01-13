using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic
{
    private ulong m_LastLogicTime = 0;
    private const uint c_FrameTime = 50;
    private int m_FillFrameNum = 0;


    // int m_ServerFrameDelta;//毫秒
    // int m_ServerRandomSeed;
    // int m_FillFrameNum;

    public void OnInit()
    {

    }

    public void OnTick(float dalta)
    {
        if (!GameControler.IsPaused)
        {

            //todo:角色控制

            //todo:世界系统
            //PlayerControl.Instance.Tick();
            //WorldSystem.Instance.Tick();
            EntityManager.Instance.Tick(dalta);

            m_FillFrameNum++;
            m_LastLogicTime += c_FrameTime;
        }
    }


    public void OnQuit()
    {

    }

}
