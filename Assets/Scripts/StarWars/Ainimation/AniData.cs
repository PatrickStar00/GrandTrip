using System;
using UnityEngine;

[Serializable]
public struct DamageBox
{
    public Vector3 StartPosition;
    public Vector3 OverPosition;
}

[Serializable]
public struct FrameData
{
    public int ImgIndex;
    public string ImgPath;
    public float Delay;
    public Vector2 Position;
    public DamageBox[] DamageBox;

}

[CreateAssetMenu(menuName = "Patrick/AniData", fileName = "AniData")]
public class AniData : ScriptableObject
{
    public bool loop = false;
    public int max {
        get {
            return frames.Length;
        }
    }

    public FrameData[] frames = new FrameData[1];

    public AniData()
    {
    }

    public void SetAnimation(string v)
    {
        throw new NotImplementedException();
    }

}