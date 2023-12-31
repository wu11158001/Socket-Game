using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    private AttackRequest attackRequest;

    private BoxCollider2D box2D;
    private AnimationEvent self;

    private float initPosX;

    //紀錄被攻擊玩家
    [SerializeField]
    private List<string> hitList = new List<string>();
    private List<string> GetHitList { get { return hitList; } }

    private void Start()
    {
        attackRequest = GetComponent<AttackRequest>();
        box2D = GetComponent<BoxCollider2D>();
        self = transform.parent.Find("Body").GetComponent<AnimationEvent>();

        initPosX = box2D.offset.x;
        box2D.enabled = false;
    }

    /// <summary>
    /// 開啟攻擊框
    /// </summary>
    /// <param name="dir">面相方向(true=右)</param>
    public void OpenBox(bool dir)
    {
        if (dir) box2D.offset = new Vector2(initPosX, box2D.offset.y);
        else box2D.offset = new Vector2(-initPosX, box2D.offset.y);

        box2D.enabled = true;

        Invoke(nameof(OnSendResponse), 0.1f);
    }

    /// <summary>
    /// 發送協議
    /// </summary>
    void OnSendResponse()
    {
        box2D.enabled = false;

        if (hitList.Count > 0)
        {
            attackRequest.SendRequest(hitList);
            hitList.Clear();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AnimationEvent hitCharacter = collision.GetComponent<AnimationEvent>();
        if (hitCharacter != null && hitCharacter != self)
        {        
            string hit = hitCharacter.userName;
            hitList.Add(hit);
        }
    }
}
