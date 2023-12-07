using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRequest : BaseRequest
{
    public override void Start()
    {
       
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="clipName"></param>
    public void PlaySound(string clipName)
    {
        gameFace.PlaySound(clipName);
    }
}
