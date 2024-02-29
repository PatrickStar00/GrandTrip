using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillMultiLineTrackStyle : SkillTrackStyleBase
{
    #region ����
    private const string menuAssetPath = "Assets/SkillEditor/Editor/Track/Assets/MultiLineTrackStyle/MultiLineTrackMenu.uxml";
    private const string trackAssetPath = "Assets/SkillEditor/Editor/Track/Assets/MultiLineTrackStyle/MultiLineTrackContent.uxml";
    private const float headHeight = 35;//5�Ǽ��
    private const float itemHeight = 32;//2�ǵײ���߾�

    #endregion


    private Func<bool> addChildTrackFunc;
    private Func<int, bool> deleteChildTrackFunc;

    private VisualElement menuItemParent;//�ӹ���Ĳ˵�������
    private List<ChildTrack> childTracksList = new List<ChildTrack>();


    public void Init(VisualElement menuParent, VisualElement contentParent, string title, Func<bool> addChildTrackFunc, Func<int, bool> deleteChildTrackFunc)
    {
        this.menuParent = menuParent;
        this.contentParent = contentParent;
        this.addChildTrackFunc = addChildTrackFunc;
        this.deleteChildTrackFunc = deleteChildTrackFunc;

        menuRoot = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(menuAssetPath).Instantiate().Query().ToList()[1];//��Ҫ������ֱ�ӳ���Ŀ������
        contentRoot = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(trackAssetPath).Instantiate().Query().ToList()[1];//��Ҫ������ֱ�ӳ���Ŀ������
        menuParent.Add(menuRoot);
        contentParent.Add(contentRoot);

        titleLabel = menuRoot.Q<Label>("Title");
        titleLabel.text = title;

        menuItemParent = menuRoot.Q<VisualElement>("TrackMenuList");

        //����ӹ���İ�ť
        Button addButton = menuRoot.Q<Button>("AddButton");
        addButton.clicked += AddButtonClicked;

        UpdateSize();
    }

    private void UpdateSize()
    {
        float height = headHeight + (childTracksList.Count * itemHeight);
        contentRoot.style.height = height;
        menuRoot.style.height = height;
    }


    //����ӹ��
    private void AddButtonClicked()
    {
        if (addChildTrackFunc == null) return;

        //���ϼ������������ж��ܲ������
        if (addChildTrackFunc())
        {
            ChildTrack childTrack = new ChildTrack();
            childTrack.Init(menuItemParent, childTracksList.Count, contentRoot, DeleteChildTrack);
            childTracksList.Add(childTrack);
            UpdateSize();
        }
    }

    //ɾ���ӹ��
    private void DeleteChildTrack(ChildTrack childTrack)
    {
        if (deleteChildTrackFunc == null) return;
        int index = childTrack.GetIndex();
        if (deleteChildTrackFunc(index))
        {
            childTrack.Destory();
            childTracksList.RemoveAt(index);
            //���е��ӹ������Ҫ����һ������
            UpdateChilds(index);
            UpdateSize();
        }
    }

    private void UpdateChilds(int startIndex = 0)
    {
        for (int i = startIndex; i < childTracksList.Count; i++)
        {
            childTracksList[i].SetIndex(i);
        }
    }


    /// <summary>
    /// ���й�����ӹ��
    /// </summary>
    public class ChildTrack
    {
        private const string childTrackMenuAssetPath = "Assets/SkillEditor/Editor/Track/Assets/MultiLineTrackStyle/MultiLineTrackMenuItem.uxml";
        private const string childTrackContentAssetPath = "Assets/SkillEditor/Editor/Track/Assets/MultiLineTrackStyle/MultiLineTrackContentItem.uxml";

        public Label titleLabel;
        #region ������ڵ㣨���Լ���
        public VisualElement menuRoot;
        public VisualElement trackRoot;
        #endregion
        #region �����ڵ㣨�ŵ�˭���棩
        public VisualElement menuParent;
        public VisualElement trackParent;
        #endregion

        private Action<ChildTrack> deleteAction;
        private int index;

        public void Init(VisualElement menuParent, int index, VisualElement trackParent, Action<ChildTrack> deleteAction)
        {
            this.menuParent = menuParent;
            this.trackParent = trackParent;
            this.deleteAction = deleteAction;

            menuRoot = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(childTrackMenuAssetPath).Instantiate().Query().ToList()[1];//��Ҫ������ֱ�ӳ���Ŀ������
            trackRoot = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(childTrackContentAssetPath).Instantiate().Query().ToList()[1];//��Ҫ������ֱ�ӳ���Ŀ������
            menuParent.Add(menuRoot);
            trackParent.Add(trackRoot);

            Button deleteButton = menuRoot.Q<Button>("DeleteButton");
            deleteButton.clicked += () => deleteAction(this);

            SetIndex(index);
        }

        public int GetIndex()
        {
            return index;
        }

        public void SetIndex(int index)
        {
            this.index = index;
            float height = 0;
            Vector3 menuPos = menuRoot.transform.position;
            height = index * itemHeight;
            menuPos.y = height;
            menuRoot.transform.position = menuPos;

            Vector3 trackPos = trackRoot.transform.position;
            height = index * itemHeight + headHeight;
            trackPos.y = height;
            trackRoot.transform.position = trackPos;
        }


        public virtual void AddItem(VisualElement ve)
        {
            trackRoot.Add(ve);
        }

        public virtual void DeleteItem(VisualElement ve)
        {
            trackRoot.Remove(ve);
        }

        public virtual void Destory()
        {
            if (menuRoot != null) menuParent.Remove(menuRoot);
            if (trackRoot != null) trackParent.Remove(trackRoot);
        }










    }

}
