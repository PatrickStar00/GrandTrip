using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public partial class CharacterView
{
    protected bool m_IsCombatState = false;

    public AniData aniData;

    public CharacterView(AniData _aniData)
    {
        aniData = _aniData;
    }
    int time = 0;
    int frame = 0;
    public int GetCurFrameId()
    {
        return frame;
    }

    public void Tick(float dalta)
    {
        time += (int)(dalta * 1000f);
        if (time >= aniData.frames[frame].Delay)
        {
            Debug.Log("Delay:" + time);
            time = 0;
            frame++;
            if (frame >= aniData.frames.Length && aniData.loop)
            {
                frame = 0;
            }
        }
    }

}