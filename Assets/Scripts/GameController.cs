using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public SaincleController lastSaincle;
    public GameObject saincles;
    public Transform saincleGroup;
    public GameObject effectPrefab;
    public Transform effectGroup;

    public Sprite[] teams;
    public Image teamImage;
    public GameObject setPanel;

    public Text scoreText;
    public GameObject scorePanel;
    public Text scoreNoti;

    public int score;
    public int maxLevel;
    public bool isOver;

    void Start()
    {
        NextSaincle();
    }

    void Awake()
    {
        Application.targetFrameRate = 60;
        //Screen.SetResolution(1080, 2340, true);
        //Screen.fullScreen = !Screen.fullScreen;
        score = 0;
    }

    void Update()
    {
        scoreText.text = score.ToString();
    }

    SaincleController GetSaincle()
    {
        GameObject newEffect = Instantiate(effectPrefab, effectGroup);
        ParticleSystem newParticle = newEffect.GetComponent<ParticleSystem>();

        GameObject newSaincle = Instantiate(saincles, saincleGroup);
        SaincleController sC = newSaincle.GetComponent<SaincleController>();
        sC.effect = newParticle;

        return sC;
    }
    void NextSaincle() {
        if (isOver)
        {
            return;
        }
        lastSaincle = GetSaincle();
        lastSaincle.gC = this;
        lastSaincle.level = Random.Range(0, maxLevel);
        lastSaincle.gameObject.SetActive(true);
        StartCoroutine("WaitNext");
    }

    public void TouchDown()
    {
        if(lastSaincle == null)
        {
            return;
        }
        lastSaincle.Drag();
    }
    public void TouchUp()
    {
        if (lastSaincle == null)
        {
            return;
        }
        lastSaincle.Drop();
        lastSaincle = null;
        //NextSaincle();
    }

    IEnumerator WaitNext()
    {
        while (lastSaincle != null)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);
        NextSaincle();
    }

    public void GameOver()
    {
        if (isOver)
        {
            return;
        }
        isOver = true;
        StartCoroutine("GameOverRoutine");
    }

    IEnumerator GameOverRoutine()
    {

        SaincleController[] controllers = FindObjectsOfType<SaincleController>();

        for (int i = 0; i < controllers.Length; i++)
        {
            controllers[i].rgbd.simulated = false;
        }

        for (int i = 0; i < controllers.Length; i++)
        {
            controllers[i].Hide(Vector3.up * 100);
            yield return new WaitForSeconds(0.1f);
        }

        Invoke("NotiScore", 2);


    }

    void NotiScore()
    {
        int teamIdx = PlayerPrefs.GetInt("team");
        //string nickname = PlayerPrefs.GetString("nickname");
        teamImage.sprite = teams[teamIdx];
        scoreNoti.text = "횐님 점수는 " + score + "점 입니다.";
        scorePanel.SetActive(true);
    }

    public void OnRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        scorePanel.SetActive(false);
    }

    public void OnExit()
    {
        Application.Quit();
    }

    public void GoIntro()
    {
        SceneManager.LoadScene("Intro");
    }

    public void OnSetClick()
    {
        if (setPanel.activeSelf)
        {
            setPanel.SetActive(false);
        }
        else
        {
            setPanel.SetActive(true);
        }
    }

}
