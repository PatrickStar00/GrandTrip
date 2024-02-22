using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.SceneManagement;
using System;
using System.Collections.Generic;

public class SkillEditorWindow : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("SkillEditor/SkillEditorWindow")]
    public static void ShowExample()
    {
        SkillEditorWindow wnd = GetWindow<SkillEditorWindow>();
        wnd.titleContent = new GUIContent("SkillEditorWindow");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);

        InitTopMenu();

        InitTimeShaft();
    }

    #region TopMenu
    private const string skillEditorScenePath = "Assets/SkillEditor/SkillEditorScene.unity";
    private const string previewCharacterParentPath = "PreviewCharacterRoot";
    private string oldScenePath;

    private Button LoadEditorSceneButton;
    private Button LoadOldSceneButton;
    private Button SkillBasicButton;

    private void InitTopMenu()
    {
        LoadEditorSceneButton = rootVisualElement.Q<Button>(nameof(LoadEditorSceneButton));
        LoadOldSceneButton = rootVisualElement.Q<Button>(nameof(LoadOldSceneButton));
        SkillBasicButton = rootVisualElement.Q<Button>(nameof(SkillBasicButton));

        LoadEditorSceneButton.clicked += LoadEditorSceneButtonClick;
        LoadOldSceneButton.clicked += LoadOldSceneButtonClick;
        SkillBasicButton.clicked += SkillBasicButtonClick;

    }

    private void LoadEditorSceneButtonClick()
    {
        string currentpath = EditorSceneManager.GetActiveScene().path;
        if (currentpath == skillEditorScenePath) return;

        oldScenePath = currentpath;
        EditorSceneManager.OpenScene(skillEditorScenePath);
    }

    private void LoadOldSceneButtonClick()
    {
        if (!string.IsNullOrEmpty(oldScenePath))
        {
            string currentpath = EditorSceneManager.GetActiveScene().path;
            if (currentpath == oldScenePath) return;

            EditorSceneManager.OpenScene(oldScenePath);
        }
        else
        {
            Debug.LogWarning("����������");
        }
    }

    private void SkillBasicButtonClick()
    {

    }

    #endregion


    #region TimerShaft

    private SkillEditorConfig skillEditorConfig = new SkillEditorConfig();

    private IMGUIContainer timeShaft;
    private VisualElement contentContainer;
    private VisualElement contentViewPort;



    /// <summary>
    /// ��ǰ���������ƫ������
    /// </summary>
    private float contentOffsetPos { get => Mathf.Abs(contentContainer.transform.position.x); }
    /// <summary>
    /// ��ǰ֡��ʱ�������������λ�ã����λ��+�·��������ƶ�λ�ã�
    /// </summary>
    //private float currentSelectFramePos { get => CurrentSelectFrameIndex * skillEditorConfig.FrameUnitWidth; }

    private void InitTimeShaft()
    {
        ScrollView MainContentView = rootVisualElement.Q<ScrollView>("MainContentView");
        contentContainer = MainContentView.Q<VisualElement>("unity-content-container");
        contentViewPort = MainContentView.Q<VisualElement>("unity-content-viewport");

        timeShaft = rootVisualElement.Q<IMGUIContainer>("TimeShaft");
        //selectLine = rootVisualElement.Q<IMGUIContainer>("SelectLine");


        timeShaft.onGUIHandler = DrawTimeShaft;

        timeShaft = rootVisualElement.Q<IMGUIContainer>("TimeShaft");
        timeShaft.onGUIHandler = DrawTimeShaft;
    }

    private void DrawTimeShaft()
    {
        Handles.BeginGUI();
        Handles.color = Color.white;
        Rect rect = timeShaft.contentRect;

        //����
        int index = Mathf.CeilToInt(contentOffsetPos / skillEditorConfig.FrameUnitWidth);
        //���������ʼƫ��λ��
        float startOffset = 0;
        if (index > 0)
            startOffset = skillEditorConfig.FrameUnitWidth - (contentOffsetPos % skillEditorConfig.FrameUnitWidth);

        int tickStep = 5;
        for (float i = startOffset; i < rect.width; i += skillEditorConfig.FrameUnitWidth)
        {
            if (index % tickStep == 0)
            {
                Handles.DrawLine(new Vector2(i, rect.height - 10), new Vector2(i, rect.height));
                string indexStr = index.ToString();
                GUI.Label(new Rect(i - indexStr.Length * 4.5f, rect.y, 35, 20), indexStr);

            }
            else
            {
                Handles.DrawLine(new Vector2(i, rect.height - 5), new Vector2(i, rect.height));
            }
            index++;
        }


        Handles.EndGUI();
    }

    #endregion
}

public class SkillEditorConfig
{
    /// <summary>
    /// ÿ֡�ı�׼��λ���ؿ̶�
    /// </summary>
    public const int StandframeUnitWidth = 10;

    /// <summary>
    /// ��10��
    /// </summary>
    public const int MaxFrameWidthLV = 10;

    /// <summary>
    /// ��ǰ��֡��λ�̶ȣ������Ŷ��仯��
    /// </summary>
    public int FrameUnitWidth = 10;

    /// <summary>
    /// Ĭ��֡��
    /// </summary>
    public float DefaultFrameRate = 10;

}