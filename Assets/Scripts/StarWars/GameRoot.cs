using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    #region Singleton
    private static GameRoot s_instance;
    public static GameRoot Instance
    {
        get { return s_instance; }
    }
    #endregion

    private static GameLogic s_GameLogic = new GameLogic();
    public static GameLogic GameLogic
    {
        get { return s_GameLogic; }
    }

    //渲染帧累计时间
    private float accumilatedTime = 0f;
    //逻辑帧间隔
    private float frameLength = 1 / 60f; //30 miliseconds
    private int frameCount = 0;

    private void Awake()
    {
        s_instance = this;
    }

    void Start()
    {
        RendingSprite rs = GameObject.Find("0").GetComponent<RendingSprite>();

        CharacterView characterView = new CharacterView(rs.aniData);
        EntityManager.Instance.AdduserView(characterView);
        rs.characterView = characterView;
    }

    //called once per unity frame
    internal void Update()
    {
        accumilatedTime = accumilatedTime + Time.deltaTime;

        while (accumilatedTime > frameLength)
        {
            GameLogic.OnTick(frameLength);
            accumilatedTime = accumilatedTime - frameLength;
            frameCount++;
        }
        GameShowSystem.Tick(Time.deltaTime);
    }
}
