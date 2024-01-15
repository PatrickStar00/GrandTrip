using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSystem
{

    // public class Msg_CRC_Create_Handler


    //        if (enter.is_player_self)
    //        {

    //            UserInfo user = WorldSystem.Instance.CreatePlayerSelf(enter.role_id, enter.hero_id);

    //    user.SetAIEnable(false);
    //            user.SetCampId(enter.camp_id);
    //            user.SetLevel(enter.role_level);
    //            user.GetMovementStateInfo().SetPosition2D(enter.position.x, enter.position.z);
    //    user.GetMovementStateInfo().SetFaceDir(enter.face_dirction);

    //    EntityManager.Instance.CreateUserView(enter.role_id);
    //            /*for (int index = 0; index < enter.skill_levels.Count; index++) {
    //              int skillId = 0;
    //              SkillInfo skillInfo = new SkillInfo(skillId);
    //              skillInfo.SkillLevel = enter.skill_levels[index];
    //              user.GetSkillStateInfo().AddSkill(index, skillInfo);
    //            }*/
    //            if (enter.scene_start_time > 0)
    //            {
    //                WorldSystem.Instance.SceneStartTime = enter.scene_start_time;
    //            }

    //user.SetNickName(enter.nickname);

    //UserView view = EntityManager.Instance.GetUserViewById(enter.role_id);
    //if (view != null)
    //{
    //    GfxSystem.SendMessage("GfxGameRoot", "CameraFollowImmediately", view.Actor);
    //}

    //if (WorldSystem.Instance.IsPvpScene())
    //{
    //    int campId = WorldSystem.Instance.CampId;
    //    /*if (campId == (int)CampIdEnum.Blue) {
    //      GfxSystem.SendMessage("GfxGameRoot", "CameraFixedYaw", Math.PI / 2);
    //    } else if (campId == (int)CampIdEnum.Red) {
    //      GfxSystem.SendMessage("GfxGameRoot", "CameraFixedYaw", -Math.PI / 2);
    //    }*/

    //    SceneResource scene = WorldSystem.Instance.GetCurScene();
    //    scene.NotifyUserEnter();
    //}
    //else
    //{
    //    SceneResource scene = WorldSystem.Instance.GetCurScene();
    //    scene.NotifyUserEnter();
    //}

    //WorldSystem.Instance.SyncGfxUserInfo(enter.role_id);

    //        }
    //        else
    //{

    //    UserInfo other = WorldSystem.Instance.CreateUser(enter.role_id, enter.hero_id);

    //    other.SetAIEnable(false);
    //    other.SetCampId(enter.camp_id);
    //    other.SetLevel(enter.role_level);
    //    other.GetMovementStateInfo().SetPosition2D(enter.position.x, enter.position.z);
    //    other.GetMovementStateInfo().SetFaceDir(enter.face_dirction);

    //    EntityManager.Instance.CreateUserView(enter.role_id);
    //    /*for (int index = 0; index < enter.skill_levels.Count; index++) {
    //      int skillId = 0;
    //      SkillInfo skillInfo = new SkillInfo(skillId);
    //      skillInfo.SkillLevel = enter.skill_levels[index];
    //      other.GetSkillStateInfo().AddSkill(index, skillInfo);
    //    }*/
    //    other.SetNickName(enter.nickname);

    //    if (WorldSystem.Instance.IsObserver)
    //    {
    //        if (enter.scene_start_time > 0)
    //        {
    //            WorldSystem.Instance.SceneStartTime = enter.scene_start_time;
    //        }

    //        LogSystem.Debug("User {0}({1}) create", enter.nickname, enter.role_id);
    //    }

    //    UserView view = EntityManager.Instance.GetUserViewById(enter.role_id);
    //    if (view != null)
    //    {
    //    }

    //    WorldSystem.Instance.SyncGfxUserInfo(enter.role_id);

    //}
}
