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

    public static SkillEditorWindow Instance;

    [MenuItem("SkillEditor/SkillEditorWindow")]
    public static void ShowExample()
    {
        SkillEditorWindow wnd = GetWindow<SkillEditorWindow>();
        wnd.titleContent = new GUIContent("���ܱ༭��");
    }

    public void CreateGUI()
    {
        SkillConfig.SetValidateAction(ResetView);


        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        rootVisualElement.Add(labelFromUXML);

        Instance = this;


        InitTopMenu();
        InitTimeShaft();
        InitConsole();
        InitContent();

        if (skillConfig != null)
        {
            SkillConfigObjectField.value = skillConfig;
            CurrentFrameCount = skillConfig.FrameCount;
        }
        else
        {
            CurrentFrameCount = 100;
        }

        if (currentPreviewCharacterPrefab != null)
        {
            PreviewCharacterPrefabObjectField.value = currentPreviewCharacterPrefab;
        }
        if (currentPreviewCharacterObj != null)
        {
            PreviewCharacterObjectField.value = currentPreviewCharacterObj;
        }

        CurrentSelectFrameIndex = 0;
    }

    private void ResetView()
    {
        //ResetTrackData();
        //UpdateContentSize();
        //ResetTrack();

        SkillConfig temp = skillConfig;
        SkillConfigObjectField.value = null;
        SkillConfigObjectField.value = temp;
    }

    private void OnDestroy()
    {
        if (skillConfig != null) SaveConfig();
    }


    #region TopMenu 
    private const string skillEditorScenePath = "Assets/SkillEditor/SkillEditorScene.unity";
    private const string PreviewCharacterParentPath = "PreviewCharacterRoot";
    private string oldScenePath;

    private Button LoadEditorSceneButton;
    private Button LoadOldSceneButton;
    private Button SkillBasicButton;

    private ObjectField PreviewCharacterPrefabObjectField;
    private ObjectField PreviewCharacterObjectField;
    private ObjectField SkillConfigObjectField;
    private GameObject currentPreviewCharacterPrefab;
    private GameObject currentPreviewCharacterObj;
    public GameObject PreviewCharacterObj { get => currentPreviewCharacterObj; }

    private void InitTopMenu()
    {
        LoadEditorSceneButton = rootVisualElement.Q<Button>(nameof(LoadEditorSceneButton));
        LoadOldSceneButton = rootVisualElement.Q<Button>(nameof(LoadOldSceneButton));
        SkillBasicButton = rootVisualElement.Q<Button>(nameof(SkillBasicButton));

        PreviewCharacterPrefabObjectField = rootVisualElement.Q<ObjectField>(nameof(PreviewCharacterPrefabObjectField));
        PreviewCharacterObjectField = rootVisualElement.Q<ObjectField>(nameof(PreviewCharacterObjectField));
        SkillConfigObjectField = rootVisualElement.Q<ObjectField>(nameof(SkillConfigObjectField));

        LoadEditorSceneButton.clicked += LoadEditorSceneButtonClick;
        LoadOldSceneButton.clicked += LoadOldSceneButtonClick;
        SkillBasicButton.clicked += SkillBasicButtonClick;

        PreviewCharacterPrefabObjectField.RegisterValueChangedCallback(PreviewCharacterPrefabObjectFieldChanged);
        PreviewCharacterObjectField.RegisterValueChangedCallback(PreviewCharacterObjectFielChanged);
        SkillConfigObjectField.RegisterValueChangedCallback(SkillConfigObjectFieldChanged);
    }



    /// <summary>
    /// ���ر༭������
    /// </summary>
    private void LoadEditorSceneButtonClick()
    {
        string currentpath = EditorSceneManager.GetActiveScene().path;
        if (currentpath == skillEditorScenePath) return;

        oldScenePath = currentpath;
        EditorSceneManager.OpenScene(skillEditorScenePath);
    }

    /// <summary>
    /// �ع�ɳ���
    /// </summary>
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

    /// <summary>
    /// �鿴���ܻ�����Ϣ
    /// </summary>
    private void SkillBasicButtonClick()
    {
        if (skillConfig != null)
        {
            Selection.activeObject = skillConfig;
        }
    }

    /// <summary>
    /// ��ɫԤ�����޸�
    /// </summary>
    /// <param name="evt"></param>
    private void PreviewCharacterPrefabObjectFieldChanged(ChangeEvent<UnityEngine.Object> evt)
    {
        //��������������ʵ����
        string currentpath = EditorSceneManager.GetActiveScene().path;
        if (currentpath != skillEditorScenePath)
        {
            PreviewCharacterPrefabObjectField.value = null;
            return;
        }

        if (evt.newValue == currentPreviewCharacterPrefab) return;
        currentPreviewCharacterPrefab = (GameObject)evt.newValue;

        //���پɵ�
        if (currentPreviewCharacterObj != null) DestroyImmediate(currentPreviewCharacterObj);

        Transform parent = GameObject.Find(PreviewCharacterParentPath).transform;
        if (parent != null && parent.childCount > 0)
        {
            DestroyImmediate(parent.GetChild(0).gameObject);
        }
        //ʵ�����µ�
        if (evt.newValue != null)
        {
            currentPreviewCharacterObj = Instantiate(evt.newValue as GameObject, Vector3.zero, Quaternion.identity, parent);
            currentPreviewCharacterObj.transform.localEulerAngles = Vector3.zero;
            PreviewCharacterObjectField.value = currentPreviewCharacterObj;
        }
    }


    /// <summary>
    /// ��ɫԤ�������޸�
    /// </summary>
    /// <param name="evt"></param>
    private void PreviewCharacterObjectFielChanged(ChangeEvent<UnityEngine.Object> evt)
    {
        currentPreviewCharacterObj = (GameObject)evt.newValue;
    }



    /// <summary>
    /// ���������޸�
    /// </summary>
    /// <param name="evt"></param>
    private void SkillConfigObjectFieldChanged(ChangeEvent<UnityEngine.Object> evt)
    {
        skillConfig = evt.newValue as SkillConfig;

        CurrentSelectFrameIndex = 0;
        if (skillConfig == null)
        {
            CurrentFrameCount = 100;
        }
        else
        {
            CurrentFrameCount = skillConfig.FrameCount;
        }

        //ˢ�¹��
        ResetTrack();

    }

    #endregion Config

    #region TimeShaft
    private IMGUIContainer timeShaft;//ʱ��������
    private IMGUIContainer selectLine;// 
    private VisualElement contentContainer;// ScrollView ����,����ó�  ScrollView ��������ק�ĳߴ����� 
    private VisualElement contentViewPort; //ʱ���ߵ���ʾ����  

    private int currentSelectFrameIndex = -1;
    /// <summary>
    /// ���λ��+�·�������λ��
    /// </summary>
    private int CurrentSelectFrameIndex
    {
        get => currentSelectFrameIndex;
        set
        {
            //���������Χ���������֡
            if (value > CurrentFrameCount) CurrentFrameCount = value;
            currentSelectFrameIndex = Mathf.Clamp(value, 0, CurrentFrameCount);
            CurrentFrameTextField.value = currentSelectFrameIndex;
            UpdateTimerShaftView();

            TickSkill();
        }
    }

    private int currentFrameCount;
    public int CurrentFrameCount
    {
        get => currentFrameCount;
        set
        {
            //if (currentFrameCount == value) return;

            currentFrameCount = value;
            FrameCountTextField.value = currentFrameCount;

            //ͬ���� skillConfig
            if (skillConfig != null)
            {
                skillConfig.FrameCount = currentFrameCount;
            }

            //Content ����ĳߴ�仯
            UpdateContentSize();
        }
    }

    /// <summary>
    /// ��ǰ���������ƫ������
    /// </summary>
    private float contentOffsetPos { get => Mathf.Abs(contentContainer.transform.position.x); }
    /// <summary>
    /// ��ǰ֡��ʱ�������������λ�ã����λ��+�·��������ƶ�λ�ã�
    /// </summary>
    private float currentSelectFramePos { get => CurrentSelectFrameIndex * skillEditorConfig.FrameUnitWidth; }


    private bool timeShaftIsMouseEnter = false;

    private void InitTimeShaft()
    {
        ScrollView MainContentView = rootVisualElement.Q<ScrollView>("MainContentView");
        contentContainer = MainContentView.Q<VisualElement>("unity-content-container");
        contentViewPort = MainContentView.Q<VisualElement>("unity-content-viewport");

        timeShaft = rootVisualElement.Q<IMGUIContainer>("TimeShaft");
        selectLine = rootVisualElement.Q<IMGUIContainer>("SelectLine");


        timeShaft.onGUIHandler = DrawTimeShaft;
        timeShaft.RegisterCallback<WheelEvent>(TimeShaftWheel);
        timeShaft.RegisterCallback<MouseDownEvent>(TimeShaftMouseDown);
        timeShaft.RegisterCallback<MouseMoveEvent>(TimeShaftMouseMove);
        timeShaft.RegisterCallback<MouseUpEvent>(TimeShaftMouseUp);
        timeShaft.RegisterCallback<MouseOutEvent>(TimeShaftMouseOut);

        selectLine.onGUIHandler = DrawSelectLine;
    }

    private void DrawTimeShaft()
    {
        Handles.BeginGUI();
        Handles.color = Color.white;
        Rect rect = timeShaft.contentRect; //ʱ����ĳߴ�

        //��ʼ����
        int index = Mathf.CeilToInt(contentOffsetPos / skillEditorConfig.FrameUnitWidth);
        //�����������ƫ��
        float startOffset = 0;
        //10-(98 % 10)
        //=10-8=2
        if (index > 0) startOffset = skillEditorConfig.FrameUnitWidth - (contentOffsetPos % skillEditorConfig.FrameUnitWidth);

        int tickStep = SkillEditorConfig.MaxFrameWidthLV + 1 - (skillEditorConfig.FrameUnitWidth / SkillEditorConfig.StandframeUnitWidth);
        //tickStep = 10+1-(100/10)=1
        //tickStep = 11-9=2
        //tickStep = 11-8=3
        //tickStep = 11-1=10

        tickStep = Mathf.Clamp(tickStep / 2, 1, SkillEditorConfig.MaxFrameWidthLV);

        for (float i = startOffset; i < rect.width; i += skillEditorConfig.FrameUnitWidth)
        {
            //���Ƴ��������ı�
            if (index % tickStep == 0)
            {
                Handles.DrawLine(new Vector3(i, rect.height - 10), new Vector3(i, rect.height));
                string indexStr = index.ToString();
                GUI.Label(new Rect(i - indexStr.Length * 4.5f, 0, 35, 20), indexStr);
            }
            else
            {
                Handles.DrawLine(new Vector3(i, rect.height - 5), new Vector3(i, rect.height));
            }

            index += 1;
        }
        Handles.EndGUI();
    }

    private void TimeShaftWheel(WheelEvent evt)
    {
        int delta = (int)evt.delta.y;
        skillEditorConfig.FrameUnitWidth = Mathf.Clamp(skillEditorConfig.FrameUnitWidth - delta,
            SkillEditorConfig.StandframeUnitWidth, SkillEditorConfig.MaxFrameWidthLV * SkillEditorConfig.StandframeUnitWidth);

        UpdateTimerShaftView();
        UpdateContentSize();

        //TrackItem ��ResetView
        ResetTrack();
    }


    private void TimeShaftMouseDown(MouseDownEvent evt)
    {
        //��ѡ����λ�ÿ���֡��λ����
        timeShaftIsMouseEnter = true;
        IsPlaying = false;
        int newValue = GetFrameIndexByMousePos(evt.localMousePosition.x);
        if (CurrentSelectFrameIndex != newValue)
        {
            CurrentSelectFrameIndex = newValue;
        }

    }
    private void TimeShaftMouseMove(MouseMoveEvent evt)
    {
        if (timeShaftIsMouseEnter)
        {
            int newValue = GetFrameIndexByMousePos(evt.localMousePosition.x);
            if (CurrentSelectFrameIndex != newValue)
            {
                CurrentSelectFrameIndex = newValue;
            }
        }
    }

    private void TimeShaftMouseUp(MouseUpEvent evt)
    {
        timeShaftIsMouseEnter = false;
    }

    private void TimeShaftMouseOut(MouseOutEvent evt)
    {
        timeShaftIsMouseEnter = false;
    }

    /// <summary>
    /// ������������ȡ֡����������
    /// ���λ��+�·�������λ��
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    private int GetFrameIndexByMousePos(float x)
    {
        return GetFrameIndexByPos(x + contentOffsetPos);
    }

    public int GetFrameIndexByPos(float x)
    {
        return Mathf.RoundToInt(x / skillEditorConfig.FrameUnitWidth);
    }


    private void DrawSelectLine()
    {
        //�жϵ�ǰѡ��֡�Ƿ�����ͼ��Χ��
        if (currentSelectFramePos >= contentOffsetPos)
        {
            Handles.BeginGUI();
            Handles.color = Color.white;
            float x = currentSelectFramePos - contentOffsetPos;
            Handles.DrawLine(new Vector3(x, 0), new Vector3(x, contentViewPort.contentRect.height + timeShaft.contentRect.height));
            Handles.EndGUI();
        }
    }

    private void UpdateTimerShaftView()
    {
        timeShaft.MarkDirtyLayout();//���Ϊ��Ҫ�������»��Ƶ�
        selectLine.MarkDirtyLayout();//���Ϊ��Ҫ�������»��Ƶ�
    }

    #endregion

    #region Console
    private Button PreviouFrameButton;
    private Button PlayButton;
    private Button NextFrameButton;
    private IntegerField CurrentFrameTextField;
    private IntegerField FrameCountTextField;

    private void InitConsole()
    {
        PreviouFrameButton = rootVisualElement.Q<Button>(nameof(PreviouFrameButton));
        PlayButton = rootVisualElement.Q<Button>(nameof(PlayButton));
        NextFrameButton = rootVisualElement.Q<Button>(nameof(NextFrameButton));

        CurrentFrameTextField = rootVisualElement.Q<IntegerField>(nameof(CurrentFrameTextField));
        FrameCountTextField = rootVisualElement.Q<IntegerField>(nameof(FrameCountTextField));

        PreviouFrameButton.clicked += PreviouFrameButtonClicked;
        PlayButton.clicked += PlayButtonClicked;
        NextFrameButton.clicked += NextFrameButtonClicked;

        CurrentFrameTextField.RegisterValueChangedCallback(CurrentFrameTextFieldValueChanged);
        FrameCountTextField.RegisterValueChangedCallback(FrameCountTextFieldValueChanged);

    }

    private void PreviouFrameButtonClicked()
    {
        IsPlaying = false;
        CurrentSelectFrameIndex -= 1;
    }

    private void PlayButtonClicked()
    {
        IsPlaying = !IsPlaying;
    }

    private void NextFrameButtonClicked()
    {
        IsPlaying = false;
        CurrentSelectFrameIndex += 1;
    }

    private void CurrentFrameTextFieldValueChanged(ChangeEvent<int> evt)
    {
        if (CurrentSelectFrameIndex != evt.newValue) CurrentSelectFrameIndex = evt.newValue;
    }
    private void FrameCountTextFieldValueChanged(ChangeEvent<int> evt)
    {
        if (CurrentFrameCount != evt.newValue) CurrentFrameCount = evt.newValue;
    }


    #endregion

    #region config
    private SkillConfig skillConfig;
    public SkillConfig SkillConfig { get => skillConfig; }
    private SkillEditorConfig skillEditorConfig = new SkillEditorConfig();

    public void SaveConfig()
    {
        if (skillConfig != null)
        {
            EditorUtility.SetDirty(skillConfig);
            AssetDatabase.SaveAssetIfDirty(skillConfig);
            ResetTrackData();
        }
    }

    private void ResetTrackData()
    {
        //��������һ������
        for (int i = 0; i < trackList.Count; i++)
        {
            trackList[i].OnConfigChanged();
        }
    }

    #endregion

    #region  Track
    private VisualElement trackMenuParent;
    private VisualElement ContentListView;
    private ScrollView MainContentView;
    private List<SkillTrackBase> trackList = new List<SkillTrackBase>();

    private void InitContent()
    {
        trackMenuParent = rootVisualElement.Q<VisualElement>("TrackMenuList");
        ContentListView = rootVisualElement.Q<VisualElement>(nameof(ContentListView));
        MainContentView = rootVisualElement.Q<ScrollView>(nameof(MainContentView));
        MainContentView.verticalScroller.valueChanged += MainContentViewverticalValueChanged;
        UpdateContentSize();

        InitTrack();
    }

    private void MainContentViewverticalValueChanged(float obj)
    {
        Vector3 pos = trackMenuParent.transform.position;
        pos.y = contentContainer.transform.position.y;
        trackMenuParent.transform.position = pos;
    }

    private void InitTrack()
    {
        if (skillConfig == null) return;
        InitAnimationTrack();

        //��Ч����Ч���....
        InitAudioTrack();
    }

    private void InitAnimationTrack()
    {
        AnimationTrack animationTrack = new AnimationTrack();
        animationTrack.Init(trackMenuParent, ContentListView, skillEditorConfig.FrameUnitWidth);
        trackList.Add(animationTrack);
    }

    private void InitAudioTrack()
    {
        AudioTrack audioTrack = new AudioTrack();
        audioTrack.Init(trackMenuParent, ContentListView, skillEditorConfig.FrameUnitWidth);
        trackList.Add(audioTrack);
    }

    private void ResetTrack()
    {
        if (skillConfig == null)
        {
            //��������й��
            DestoryTracks();
        }
        else
        {
            //�������б�����û�����ݣ�˵��û�й�����������õģ���Ҫ��ʼ��
            if (trackList.Count == 0)
            {
                InitTrack();
            }

            //������ͼ
            for (int i = 0; i < trackList.Count; i++)
            {
                trackList[i].ResetView(skillEditorConfig.FrameUnitWidth);
            }
        }
    }

    private void DestoryTracks()
    {
        for (int i = 0; i < trackList.Count; i++)
        {
            trackList[i].Destory();
        }
        trackList.Clear();
    }

    /// <summary>
    /// Content ����ĳߴ�仯
    /// </summary>
    private void UpdateContentSize()
    {
        ContentListView.style.width = skillEditorConfig.FrameUnitWidth * CurrentFrameCount;
    }




    public void ShowTrackItemOnInspector(TrackItemBase trackItem, SkillTrackBase track)
    {
        SkillEditorInspector.SetTrackItem(trackItem, track);
        Selection.activeObject = this;
    }
    #endregion

    #region Preview
    private bool isPlaying;
    public bool IsPlaying
    {
        get => isPlaying;
        set
        {
            isPlaying = value;
            if (isPlaying)
            {
                startTime = DateTime.Now;
                startFrameIndex = currentSelectFrameIndex;
            }
        }
    }

    private DateTime startTime;
    private int startFrameIndex;


    private void Update()
    {
        if (IsPlaying)
        {
            //�õ�ʱ���
            float time = (float)DateTime.Now.Subtract(startTime).TotalSeconds;

            //ȷ��ʱ�����֡��
            float frameRate = skillConfig != null ? skillConfig.FrameRate : skillEditorConfig.DefaultFrameRate;

            //����ʱ�����㵱ǰ��ѡ��֡
            CurrentSelectFrameIndex = (int)((time * frameRate) + startFrameIndex);

            //�������һ֡�Զ���ͣ
            if (CurrentSelectFrameIndex == CurrentFrameCount)
            {
                IsPlaying = false;
            }
        }
    }

    private void TickSkill()
    {
        //�������ܱ���
        if (skillConfig != null && currentPreviewCharacterObj != null)
        {
            //������������
            for (int i = 0; i < trackList.Count; i++)
            {
                trackList[i].TickView(currentSelectFrameIndex);
            }
        }
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