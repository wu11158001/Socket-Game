using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager
{
    protected GameFace gameFace;

    public BaseManager(GameFace gameFace)
    {
        this.gameFace = gameFace;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void OnInit()
    {

    }

    public virtual void OnDestroy()
    {

    }
}
