using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager
{
    #region Singleton
    private static EntityManager s_instance_ = new EntityManager();
    public static EntityManager Instance
    {
        get { return s_instance_; }
    }
    #endregion

    #region Fields

    //private Dictionary<int, UserView> m_UserViews = new Dictionary<int, UserView>();
    //private Dictionary<int, NpcView> m_NpcViews = new Dictionary<int, NpcView>();
    //private Dictionary<int, SpaceInfoView> m_SpaceInfoViews = new Dictionary<int, SpaceInfoView>();

    #endregion
    private List<CharacterView> m_UserViews = new List<CharacterView>();

    public void CreateUserView(CharacterView userView)
    {
        m_UserViews.Add(userView);
    }


    public void AdduserView(CharacterView userView)
    {
        m_UserViews.Add(userView);
    }

    public void RemoveuserView(CharacterView userView)
    {
        m_UserViews.Remove(userView);
    }

    public void Tick(float dalta)
    {
        foreach (var userView in m_UserViews)
        {
            userView.Tick(dalta);
        }
    }
}
