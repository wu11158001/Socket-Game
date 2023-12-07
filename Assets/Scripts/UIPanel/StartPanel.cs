using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : BasePanel
{
    [SerializeField] private Button startBtn;
    [SerializeField] private Text start_Txt;

    private float fadeDuration = 0.85f;

    private float textAhpha;

    /// <summary>
    /// UI面板開始
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
        Entter();
    }

    /// <summary>
    /// UI面板退出
    /// </summary>
    public override void OnExit()
    {
        base.OnExit();
        Exit();
    }

    /// <summary>
    /// UI面板暫停
    /// </summary>
    public override void OnPause()
    {
        base.OnPause();
        Exit();
    }

    /// <summary>
    /// UI面板繼續
    /// </summary>
    public override void OnRecovery()
    {
        base.OnRecovery();
        Entter();
    }

    /// <summary>
    /// 進入
    /// </summary>
    void Entter()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 離開
    /// </summary>
    void Exit()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        startBtn.onClick.AddListener(StartBtnClick);

        StartCoroutine(FadeText());
    }

    private void Update()
    {
        start_Txt.color = new Color(start_Txt.color.r, start_Txt.color.g, start_Txt.color.b, textAhpha);

        if (Input.anyKeyDown) StartBtnClick();
    }

    /// <summary>
    /// 開始按鈕按下
    /// </summary>
    void StartBtnClick()
    {
        gameFace.ButtonClick();
        uiManager.PushPanel(PanelType.Login);
    }

    private IEnumerator FadeText()
    {
        while (true)
        {
            yield return StartCoroutine(FadeIn());
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            textAhpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            textAhpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
