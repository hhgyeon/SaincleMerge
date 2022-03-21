using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{

    public SpriteRenderer stSr;

    public GameObject panel;
    public Sprite[] teams;
    public Image teamImage;
    public Text nickname;
    public Text team;
    string[] teamName =
        { "백노바", "강초당", "구라베", "망난이", "송몽숙", "장폰주", "사인클", "레이첼박사"};
    int teamIdx;
    // Start is called before the first frame update
    void Start()
    {

    }
    void Awake()
    {

        //Screen.fullScreen = !Screen.fullScreen;
    }

    // Update is called once per frame
    void Update()
    {
        Color c = stSr.material.color;
        c.a = Mathf.Abs(Mathf.Sin(2 * Time.time));
        stSr.material.color = c;

    }

    public void GameStart()
    {
        teamIdx = Random.Range(0, 6);
        switch (nickname.text)
        {
            case "P_Nova":
            case "류마티스":
                teamIdx = 0;
                break;

            case "76_ChoDang":
            case "디스크":
                teamIdx = 1;
                break;

            case "Ra_Ve":
            case "테니스엘보":
                teamIdx = 2;
                break;

            case "Mang_nan2":
            case "분쇄골절":
                teamIdx = 3;
                break;

            case "Mong_Suk":
            case "건초염":
                teamIdx = 4;
                break;

            case "Pon_Z00":
            case "타박상":
                teamIdx = 5;
                break;

            case "사인클":
            case "횐":
                teamIdx = 6;
                break;

            case "레이첼박사":
            case "닥터레이첼":
                teamIdx = 7;
                break;

        }
        team.text = "횐님은\n" + teamName[teamIdx] + " 팀 입니다.";
        teamImage.sprite = teams[teamIdx];
        panel.SetActive(true);
    }

    public void GoMain()
    {
        PlayerPrefs.SetInt("team", teamIdx);
        //PlayerPrefs.SetString("nickname", nickname.text);
        SceneManager.LoadScene("Main");
    }

}
