using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class SoundManager : BaseManager
{
    public SoundManager(GameFace gameFace) : base(gameFace) { }

    private AudioSource BGMSource;

    private GameObject soundObj;
    private AudioClip[] soundClips;
    private AudioClip[] musicClips;

    private bool isSoundMute, isMusicMute;

    public override void OnInit()
    {
        base.OnInit();

        BGMSource = Camera.main.GetComponent<AudioSource>();
        soundObj = Resources.Load<GameObject>("Prefab/Sound");
        soundClips = Resources.LoadAll<AudioClip>("Sounds");
        musicClips = Resources.LoadAll<AudioClip>("BGM");

        PlayBGM("Hall");
    }

    /// <summary>
    /// 音樂/音效開關
    /// </summary>
    /// <param name="isSound">音效</param>
    /// <param name="isMusic">音樂</param>
    public void SoundSwitch(bool isSound, bool isMusic)
    {
        isSoundMute = !isSound;
        isMusicMute = !isMusic;

        BGMSource.mute = isMusicMute;
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="clipName"></param>
    public void PlaySound(string clipName)
    {
        if (isSoundMute) return;

        bool isHave = soundClips.ToList().Where(x => x.name == clipName).Count() > 0;
        if (isHave)
        {
            if (GameObject.Instantiate(soundObj).TryGetComponent<AudioSource>(out AudioSource source))
            {
                AudioClip clip = soundClips.ToList().Where(x => x.name == clipName).First();
                source.clip = clip;
                source.Play();

                RemoveSound(source);
            }
        }
        else
        {
            Debug.LogError("沒有找到音效:" + clipName);
        }        
    }

    /// <summary>
    /// 移除音效物件
    /// </summary>
    /// <param name="source"></param>
    async void RemoveSound(AudioSource source)
    {
        await Task.Delay((int)(source.clip.length * 1000));
        GameObject.Destroy(source.gameObject);
    }

    /// <summary>
    /// 撥放背景音樂
    /// </summary>
    /// <param name="clipName"></param>
    public void PlayBGM(string clipName)
    {
        if (isMusicMute) return;

        bool isHave = musicClips.ToList().Where(x => x.name == clipName).Count() > 0;
        if (isHave)
        {
            AudioClip clip = musicClips.ToList().Where(x => x.name == clipName).First();
            BGMSource.clip = clip;
            BGMSource.Play();
        }
        else
        {
            Debug.LogError("沒有找到音樂:" + clipName);
        }
    }

    /// <summary>
    /// 按鈕點擊音效
    /// </summary>
    public void ButtonClick()
    {
        PlaySound("ButtonClick");
    }
}
