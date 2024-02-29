using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class AnimationTrack : SkillTrackBase
{
    private SkillSingleLineTrackStyle trackStyle;

    private Dictionary<int, AnimationTrackItem> trackItemDic = new Dictionary<int, AnimationTrackItem>();
    public SkillAnimationData AnimationData { get => SkillEditorWindow.Instance.SkillConfig.SkillAnimationData; }

    public override void Init(VisualElement menuParent, VisualElement trackParent, float frameWidth)
    {
        base.Init(menuParent, trackParent, frameWidth);
        trackStyle = new SkillSingleLineTrackStyle();
        trackStyle.Init(menuParent, trackParent, "��������");
        trackStyle.contentRoot.RegisterCallback<DragUpdatedEvent>(OnDragUpdatedEvent);
        trackStyle.contentRoot.RegisterCallback<DragExitedEvent>(OnDragExitedEvent);

        ResetView();
    }

    public override void ResetView(float frameWidth)
    {
        base.ResetView(frameWidth);
        //���ٵ�ǰ����
        foreach (var item in trackItemDic)
        {
            trackStyle.DeleteItem(item.Value.itemStyle.root);
        }

        trackItemDic.Clear();
        if (SkillEditorWindow.Instance.SkillConfig == null) return;

        //�������ݻ��� TrackItem
        foreach (var item in AnimationData.FrameDataDic)
        {
            CreateItem(item.Key, item.Value);
        }
    }

    private void CreateItem(int frameIndex, SkillAnimationEvent skillAnimationEvent)
    {
        AnimationTrackItem trackItem = new AnimationTrackItem();
        trackItem.Init(this, trackStyle, frameIndex, frameWidth, skillAnimationEvent);
        trackItemDic.Add(frameIndex, trackItem);
    }


    #region  ��ק��Դ
    private void OnDragUpdatedEvent(DragUpdatedEvent evt)
    {
        UnityEngine.Object[] objs = DragAndDrop.objectReferences;
        AnimationClip clip = objs[0] as AnimationClip;
        if (clip != null)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
        }
    }

    private void OnDragExitedEvent(DragExitedEvent evt)
    {
        UnityEngine.Object[] objs = DragAndDrop.objectReferences;
        AnimationClip clip = objs[0] as AnimationClip;
        if (clip != null)
        {
            //���ö�����Դ

            //��ǰѡ�е�֡��λ�� ����Ƿ��ܷ��ö���
            int selectFrameIndex = SkillEditorWindow.Instance.GetFrameIndexByPos(evt.localMousePosition.x);
            bool canPlace = true;
            int durationFrame = -1;//-1 ���������ԭ�� AnimationClip �ĳ���ʱ��
            int clipFrameCount = (int)(clip.length * clip.frameRate);
            int nextTrackItem = -1;
            int currentOffset = int.MaxValue;

            foreach (var item in AnimationData.FrameDataDic)
            {
                //������ѡ��֡�� TrackItem �м䣨�����¼�����㵽�����յ�֮�䣩
                if (selectFrameIndex > item.Key && selectFrameIndex < item.Key + item.Value.DurationFrame)
                {
                    //���ܷ���
                    canPlace = false;
                    break;
                }

                //�ҵ��Ҳ����� TrackItem
                if (item.Key > selectFrameIndex)
                {
                    int tempOffset = item.Key - selectFrameIndex;
                    if (tempOffset < currentOffset)
                    {
                        currentOffset = tempOffset;
                        nextTrackItem = item.Key;
                    }
                }
            }

            //ʵ�ʵķ���
            if (canPlace)
            {
                // �ұ������� TrackItem ��Ҫ���� Track �����ص�������
                if (nextTrackItem != -1)
                {
                    int offset = clipFrameCount - currentOffset;
                    durationFrame = offset < 0 ? clipFrameCount : currentOffset; //��������ռ��ܲ�������������Ƭ�ηŽ�ȥ
                }
                else
                {
                    //�Ҳ�ɶ��û��
                    durationFrame = clipFrameCount;
                }

                //������������
                SkillAnimationEvent animationEvent = new SkillAnimationEvent()
                {
                    AnimationClip = clip,
                    DurationFrame = durationFrame,
                    TransitionTime = 0.25f
                };

                //���������Ķ�������
                AnimationData.FrameDataDic.Add(selectFrameIndex, animationEvent);
                SkillEditorWindow.Instance.SaveConfig();

                //����һ��Item
                CreateItem(selectFrameIndex, animationEvent);
            }
        }
    }

    #endregion

    public bool CheckFrameIndexOnDrag(int targetindex, int selfIndex, bool isLeft)
    {
        foreach (var item in AnimationData.FrameDataDic)
        {
            //��קʱ���������
            if (item.Key == selfIndex) continue;

            //�����ƶ�&&ԭ�����ұ�&&Ŀ��û���ص�
            if (isLeft && selfIndex > item.Key && targetindex < item.Key + item.Value.DurationFrame)
            {
                return false;
            }
            //�����ƶ�&&ԭ�������&&Ŀ��û���ص�
            else if (!isLeft && selfIndex < item.Key && targetindex > item.Key)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// �� oldIndex �����ݱ�Ϊ newIndex
    /// </summary>
    public void SetFrameIndex(int oldIndex, int newIndex)
    {
        if (AnimationData.FrameDataDic.Remove(oldIndex, out SkillAnimationEvent animationEvent))
        {
            AnimationData.FrameDataDic.Add(newIndex, animationEvent);
            trackItemDic.Remove(oldIndex, out AnimationTrackItem animationTrackItem);
            trackItemDic.Add(newIndex, animationTrackItem);

            SkillEditorWindow.Instance.SaveConfig();
        }
    }

    public override void DeleteTrackItem(int frameIndex)
    {
        //�Ƴ�����
        AnimationData.FrameDataDic.Remove(frameIndex);
        if (trackItemDic.Remove(frameIndex, out AnimationTrackItem item))
        {
            //�Ƴ���ͼ
            trackStyle.DeleteItem(item.itemStyle.root);
        }
        SkillEditorWindow.Instance.SaveConfig();
    }

    public override void OnConfigChanged()
    {
        foreach (var item in trackItemDic.Values)
        {
            item.OnConfigChanged();
        }
    }

    public override void TickView(int frameIndex)
    {
        base.TickView(frameIndex);

        GameObject previewGameObject = SkillEditorWindow.Instance.PreviewCharacterObj;
        Animator animator = previewGameObject.GetComponent<Animator>();

        //����֡�ҵ�Ŀǰ���ĸ�����
        Dictionary<int, SkillAnimationEvent> frameDateDic = AnimationData.FrameDataDic;

        #region ���ڸ��˶�����
        SortedDictionary<int, SkillAnimationEvent> frameDataSortedDic = new SortedDictionary<int, SkillAnimationEvent>(frameDateDic);
        int[] keys = frameDataSortedDic.Keys.ToArray();
        Vector3 rootMotionTotalPos = Vector3.zero;

        //�ӵ�0֡��ʼ�ۼ�λ������
        for (int i = 0; i < keys.Length; i++)
        {
            int key = keys[i]; //��ǰ��������ʼ֡��
            SkillAnimationEvent animationEvent = frameDataSortedDic[key];

            //ֻ���Ǹ��˶����õĶ���
            if (animationEvent.ApplyRootMotion == false) continue;

            //�ҵ���һ��������֡��ʼλ��
            int nextKeyFrame = i + 1 < keys.Length ? keys[i + 1] : SkillEditorWindow.Instance.SkillConfig.FrameCount;//���һ������

            //��������һ�β���
            bool isBreak = false;
            //��һ֡���ڵ�ǰѡ��֡��֡���ۼ���ɣ�����ֹͣ�ۼ�����ı�־��
            if (nextKeyFrame > frameIndex)
            {
                nextKeyFrame = frameIndex;
                isBreak = true;
            }

            //����֡��=��һ��������֡��  -  ��������Ŀ�ʼʱ��
            int durationFrameCount = nextKeyFrame - key;
            if (durationFrameCount > 0)
            {
                //������Դ����֡��
                float clipFrameCount = animationEvent.AnimationClip.length * SkillEditorWindow.Instance.SkillConfig.FrameRate;
                //�����ܵĲ��Ž���
                float totalProgress = durationFrameCount / clipFrameCount;
                //���Ŵ���
                int playTimes = 0;
                //���ղ�������һ�β��ŵĽ���
                float lastProgress = 0;
                //ֻ��ѭ����������Ҫ�������
                if (animationEvent.AnimationClip.isLooping)
                {
                    playTimes = (int)totalProgress;
                    lastProgress = totalProgress - (int)totalProgress;
                }
                else
                {
                    // ��ѭ���Ķ�����������Ž��ȳ���1��Լ��Ϊ1
                    if (totalProgress >= 1)
                    {
                        playTimes = 1;
                        lastProgress = 0;
                    }
                    else if (totalProgress < 1)
                    {
                        lastProgress = totalProgress; // ��Ϊ�ܽ���С��1�����Ա���������һ�β���
                        playTimes = 0;
                    }
                }

                //��������
                animator.applyRootMotion = true;
                if (playTimes >= 1)
                {
                    //����һ�ζ�������������
                    animationEvent.AnimationClip.SampleAnimation(previewGameObject, animationEvent.AnimationClip.length);
                    Vector3 samplePos = previewGameObject.transform.position;
                    rootMotionTotalPos += samplePos * playTimes;
                }

                if (lastProgress > 0)
                {
                    //����һ�ζ����Ĳ���������
                    animationEvent.AnimationClip.SampleAnimation(previewGameObject, lastProgress * animationEvent.AnimationClip.length);
                    Vector3 samplePos = previewGameObject.transform.position;
                    rootMotionTotalPos += samplePos;
                }
            }

            if (isBreak) break;
        }
        #endregion

        #region ���ڵ�ǰ֡����̬
        //�ҵ�������һ֡��������һ��������Ҳ���ǵ�ǰҪ���ŵĶ���
        int currentOffset = int.MaxValue;  //������������뵱ǰѡ��֡�Ĳ��
        int animtionEventIndex = -1;
        foreach (var item in frameDateDic)
        {
            int tempOffset = frameIndex - item.Key;
            if (tempOffset > 0 && tempOffset < currentOffset)
            {
                currentOffset = tempOffset;
                animtionEventIndex = item.Key;
            }
        }

        if (animtionEventIndex != -1)
        {
            SkillAnimationEvent animationEvent = frameDateDic[animtionEventIndex];
            //������Դ��֡��
            float clipFrameCount = animationEvent.AnimationClip.length * animationEvent.AnimationClip.frameRate;
            //���㵱ǰ�Ĳ��Ž���
            float progress = currentOffset / clipFrameCount;
            //ѭ�������Ĵ���
            if (progress > 1 && animationEvent.AnimationClip.isLooping)
            {
                progress -= (int)progress;//ֻ����С���㲿��
            }

            //���˴����޸Ľ�ɫλ�ã�
            animator.applyRootMotion = animationEvent.ApplyRootMotion;
            animationEvent.AnimationClip.SampleAnimation(previewGameObject, progress * animationEvent.AnimationClip.length);
        }
        #endregion

        //����ɫ����ʵ��λ��
        previewGameObject.transform.position = rootMotionTotalPos;
    }

    public override void Destory()
    {
        trackStyle.Destory();
    }

}
