using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JKFrame;
using Sirenix.OdinInspector;
using System;
using Sirenix.Serialization;

[CreateAssetMenu(menuName = "Config/SkillConfig", fileName = "SkillConfig")]
public class SkillConfig : ConfigBase
{
    [LabelText("��������")] public string SkillName;
    [LabelText("֡������")] public int FrameCount = 100;
    [LabelText("֡��")] public int FrameRate = 30;

    [NonSerialized, OdinSerialize]
    public SkillAnimationData SkillAnimationData = new SkillAnimationData();

#if UNITY_EDITOR
    private static Action onSkillConfigValidate;

    public static void SetValidateAction(Action action)
    {
        onSkillConfigValidate = action;
    }

    private void OnValidate()
    {
        onSkillConfigValidate?.Invoke();
    }
#endif

}

/// <summary>
/// ���ܶ�������
/// </summary>
[Serializable]
public class SkillAnimationData
{
    /// <summary>
    /// ����֡�¼�
    /// key:֡��
    /// value���¼�����
    /// </summary>
    [NonSerialized, OdinSerialize]//��ͨ��Unity�����л�����Odin �����л�
    [DictionaryDrawerSettings(KeyLabel = "֡��", ValueLabel = "��������")]
    public Dictionary<int, SkillAnimationEvent> FrameDataDic = new Dictionary<int, SkillAnimationEvent>();
}

/// <summary>
/// ֡�¼�����
/// </summary>
[Serializable]
public abstract class SkillFrameEventBase
{

}

public class SkillAnimationEvent : SkillFrameEventBase
{
    public AnimationClip AnimationClip;
    public float TransitionTime = 0.25f;
    public bool ApplyRootMotion;

#if UNITY_EDITOR
    public int DurationFrame;

#endif

}