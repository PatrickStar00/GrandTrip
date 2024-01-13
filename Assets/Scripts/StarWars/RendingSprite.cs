using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class RendingSprite : MonoBehaviour
{
    //public Sprite[] images;
    public SpriteRenderer spriteRenderer;
    public AniData aniData;

    [HideInInspector] public Sprite[] part_Sprite;
    [HideInInspector] public Texture2D[] part_Tex;
    public List<Vector2> pivots = new List<Vector2>();

    public int A;
    public float B;

    public int Xoffset;
    public int Yoffset;

    int frameID = 0;

    public CharacterView characterView { get; set; }

    public BoxCollider2D[] boxColliders { get; set; }

    void Awake()
    {
        //images = new Sprite[210];
        part_Sprite = new Sprite[210];
        //part_Tex  = new Texture2D[210];
        boxColliders = GetComponents<BoxCollider2D>();
    }

    void Start()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        //images = new Sprite[aniData.frames.Length];
        for (int i = 0; i < aniData.frames.Length; i++)
        {
            //print(aniData.frames[i].ImgPath + "/" + aniData.frames[i].ImgIndex.ToString());
            //var sp = Resources.Load<Sprite>(aniData.frames[i].ImgPath + "/" + aniData.frames[i].ImgIndex.ToString());
            //part_Sprite[i] = sp;
            var tx = Resources.Load<Sprite>("Frame/sm_body80000/" + aniData.frames[i].ImgIndex.ToString());
            //part_Tex[i] = tx;

            //Vector2 pivot = new Vector2(0.5f - ((aniData.frames[i].Position.x - Xoffset + images[i].rect.width / 2) / images[i].rect.width),
            //0.5f + ((aniData.frames[i].Position.y - Yoffset + images[i].rect.height / 2) / images[i].rect.height));

            //var rect = part_Sprite[i].rect;
            //var psoition = aniData.frames[i].Position;
            // Vector2 pivot = new Vector2(0.5f - ((psoition.x - Xoffset + rect.width / 2) / rect.width),
            //    0.5f + ((psoition.y - Yoffset + rect.height / 2) / rect.height));
            //part_Sprite[i] = Sprite.Create(part_Tex[i], rect, pivot, B);
            //pivots.Add(pivot);

            //var psoition = aniData.frames[i].Position;

            //Rect rect = new Rect
            //{
            //    width = tx.width,
            //    height = tx.height
            //};


            ////DNF默认左上方为原点 图片左上为锚点
            ////Unity默认左下为原点 pivot基于Sprite做下
            //float px =     (Xoffset - psoition.x) / rect.width;
            //float py = 1 - (Yoffset - psoition.y) / rect.height;
            //Debug.LogError(px + " +++++++++++ " + py);
            //Vector2 pivot = new Vector2(px, py);
            //part_Sprite[i] = Sprite.Create(tx, rect, pivot);
            //pivots.Add(pivot);
            part_Sprite[i] = tx;
            //https://zhuanlan.zhihu.com/p/48921252?utm_id=0
        }
    }

    public void Update()
    {
        if (characterView != null)
        {
            frameID = characterView.GetCurFrameId();

            var data = characterView.aniData.frames[frameID];

            for (int i = 0; i < boxColliders.Length; i++)
            {

                if (i >= data.DamageBox.Length)
                {
                    boxColliders[i].enabled = false;
                }
                else
                {
                    var box1 = data.DamageBox[i];
                    var center = new Vector2(box1.OverPosition.x * 0.5f + box1.StartPosition.x, box1.OverPosition.z * 0.5f + box1.StartPosition.z);
                    var size = new Vector2(box1.OverPosition.x, box1.OverPosition.z);
                    Bounds bounds = new Bounds(center, size);
         
                    //boxColliders[i].size = 
                    //boxColliders[i].offset = 
                }

            }


        }
        //var position = aniData.frames[frameID].Position;
        //transform.position = new Vector2(position.x / 100, position.y /100);
        spriteRenderer.sprite = part_Sprite[frameID];
    }

    private void OnGUI()
    {
 
    }
}