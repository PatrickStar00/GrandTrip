using System;
using System.Collections.Generic;
using UnityEngine;

/*
  * TODO: 
  * 1. 由於多數操作依賴playerself的存在, 而playerself可能未創建或者已經刪除
  *    所以是否可以考慮外部輸入還是設計成可以與某個obj綁定?
  * 2. 組合鍵, 設定一組鍵的狀態同時彙報, 例如wasd的狀態希望能夠在一個囘調函數中得到通知
  *    (不適用主動查詢的情況下), 使用主動查詢就必須存在一個Tick函數
  *    
  */
public class PlayerControl
{
    // static methods
    private static PlayerControl inst_ = new PlayerControl();
    public static PlayerControl Instance { get { return inst_; } }
    // properties
    public bool EnableMoveInput { get; set; }
    public bool EnableRotateInput { get; set; }
    public bool EnableSkillInput { get; set; }

    public float MoveDir { get; set; }
    public bool MotionChanged { get; set; }




    // methods
    public PlayerControl()
    {
        //pm_ = new PlayerMovement();
        //EnableMoveInput = true;
        //EnableRotateInput = true;
        //EnableSkillInput = true;
    }

    public void Reset()
    {
        EnableMoveInput = true;
        EnableRotateInput = true;
        EnableSkillInput = true;
    }
    public void Init()
    {

    }

    public void Tick()
    {

    }

    private void UpdateMoveState(CharacterInfo playerself, Vector3 targetpos, float towards)
    {
        //CharacterView view = EntityManager.Instance.GetUserViewById(playerself.GetId());
        //if (view != null && view.ObjectInfo.IsGfxMoveControl && Geometry.IsSamePoint(Vector3.Zero, targetpos))
        //{
        //    //LogSystem.Debug("UpdateMoveState IsGfxMoveControl : {0} , targetpos : {1}", view.ObjectInfo.IsGfxMoveControl, targetpos.ToString());
        //    return;
        //}
        //PlayerMovement.Motion m = Geometry.IsSamePoint(Vector3.Zero, targetpos) ? PlayerMovement.Motion.Stop : PlayerMovement.Motion.Moving;
        //pm_.JoyStickMotionChanged = pm_.JoyStickMotionStatus != m || !Geometry.IsSameFloat(m_lastDir, towards);
        //pm_.JoyStickMotionStatus = m;
        //pm_.MoveDir = towards;
        //if (Geometry.IsSamePoint(Vector3.Zero, targetpos))
        //{
        //    pm_.JoyStickMotionStatus = PlayerMovement.Motion.Stop;
        //    m_IsJoystickControl = false;
        //}
        //else
        //{
        //    m_IsJoystickControl = true;
        //}
    }




}
