using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class AnimationTrackItem : TrackItemBase<AnimationTrack>
{
    private SkillAnimationEvent animationEvent;
    public SkillAnimationEvent AnimationEvent { get => animationEvent; }
    private SkillAnimationTrackItemStyle trackItemStyle;

    public void Init(AnimationTrack animationTrack, SkillTrackStyleBase parentTrackStyle, int startFrameIndex, float frameUnitWidth, SkillAnimationEvent animationEvent)
    {
        track = animationTrack;
        this.frameIndex = startFrameIndex;
        this.frameUnitWidth = frameUnitWidth;
        this.animationEvent = animationEvent;

        itemStyle = trackItemStyle = new SkillAnimationTrackItemStyle();
        trackItemStyle.Init(parentTrackStyle, startFrameIndex, frameUnitWidth);



        normalColor = new Color(0.388f, 0.850f, 0.905f, 0.5f);
        selectColor = new Color(0.388f, 0.850f, 0.905f, 1f);
        OnUnSelect();

        //���¼�
        trackItemStyle.mainDragArea.RegisterCallback<MouseDownEvent>(OnMouseDownEvent);
        trackItemStyle.mainDragArea.RegisterCallback<MouseUpEvent>(OnMouseUpEvent);
        trackItemStyle.mainDragArea.RegisterCallback<MouseOutEvent>(OnMouseOutEvent);
        trackItemStyle.mainDragArea.RegisterCallback<MouseMoveEvent>(OnMouseMoveEvent);

        ResetView(frameUnitWidth);
    }

    public override void ResetView(float frameUnitWidth)
    {
        base.ResetView(frameUnitWidth);

        this.frameUnitWidth = frameUnitWidth;
        trackItemStyle.SetTitle(animationEvent.AnimationClip.name);

        //λ�ü���
        trackItemStyle.SetPosition(frameIndex * frameUnitWidth);
        trackItemStyle.SetWidth(animationEvent.DurationFrame * frameUnitWidth);

        //���㶯�������ߵ�λ��
        int animationClipFrameCount = (int)(animationEvent.AnimationClip.length * animationEvent.AnimationClip.frameRate);
        if (animationClipFrameCount > animationEvent.DurationFrame)
        {
            trackItemStyle.animationOverLine.style.display = DisplayStyle.None;
        }
        else
        {
            trackItemStyle.animationOverLine.style.display = DisplayStyle.Flex;
            Vector3 overLinePos = trackItemStyle.animationOverLine.transform.position;
            //overLinePos.x = animationClipFrameCount * frameUnitWidth - animationOverLine.style.width.value.value / 2;
            overLinePos.x = animationClipFrameCount * frameUnitWidth - 1; //�������Ϊ2��ȡһ��
            trackItemStyle.animationOverLine.transform.position = overLinePos;
        }

    }


    #region  ��꽻��
    private bool mouseDrag = false;
    private float startDragPosX;
    private int startDragFrameIndex;

    private void OnMouseDownEvent(MouseDownEvent evt)
    {
        startDragPosX = evt.mousePosition.x;
        startDragFrameIndex = frameIndex;
        mouseDrag = true;

        Select();
    }

    private void OnMouseUpEvent(MouseUpEvent evt)
    {
        if (mouseDrag) ApplyDrag();
        mouseDrag = false;
    }

    private void OnMouseOutEvent(MouseOutEvent evt)
    {
        if (mouseDrag) ApplyDrag();
        mouseDrag = false;
    }

    private void OnMouseMoveEvent(MouseMoveEvent evt)
    {
        if (mouseDrag)
        {
            float offsetPos = evt.mousePosition.x - startDragPosX;
            int offsetFrame = Mathf.RoundToInt(offsetPos / frameUnitWidth);
            int targetFrameIndex = startDragFrameIndex + offsetFrame;
            bool checkDrag = false;

            if (targetFrameIndex < 0) return; //��������ק�����������

            if (offsetFrame < 0)
            {
                checkDrag = track.CheckFrameIndexOnDrag(targetFrameIndex, startDragFrameIndex, true);
            }
            else if (offsetFrame > 0)
            {
                checkDrag = track.CheckFrameIndexOnDrag(targetFrameIndex + animationEvent.DurationFrame, startDragFrameIndex, false);
            }
            else return;

            if (checkDrag)
            {
                //ȷ���޸ĵ�����
                frameIndex = targetFrameIndex;

                //��������Ҳ�߽磬��չ�߽�
                CheckFrameCount();

                //ˢ����ͼ
                ResetView(frameUnitWidth);
            }
        }
    }

    /// <summary>
    /// ��������Ҳ�߽磬��չ�߽�
    /// </summary>
    public void CheckFrameCount()
    {
        if (frameIndex + animationEvent.DurationFrame > SkillEditorWindow.Instance.SkillConfig.FrameCount)
        {
            //�������õ��¶�����Ч����������
            SkillEditorWindow.Instance.CurrentFrameCount = frameIndex + animationEvent.DurationFrame;
        }
    }

    private void ApplyDrag()
    {
        if (startDragFrameIndex != frameIndex)
        {
            track.SetFrameIndex(startDragFrameIndex, frameIndex);
            SkillEditorInspector.Instance.SetTrackItemFrameIndex(frameIndex);
        }
    }

    #endregion

    public override void OnConfigChanged()
    {
        animationEvent = track.AnimationData.FrameDataDic[frameIndex];
    }

}
