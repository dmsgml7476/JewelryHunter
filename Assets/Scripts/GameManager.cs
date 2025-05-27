using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 씬 관리 네임스페이스 추가

public class GameManager : MonoBehaviour
{
    public GameObject mainImage;  // image를 담을 GameObject
    public Sprite gameOverSpr; // 게임 오버 이미지
    public Sprite gameClearSpr; // 게임 클리어 이미지
    public GameObject panel;// 패널 GameObject
    public GameObject restartButton; // Restart 버튼 GameObject
    public GameObject nextButton; // Next 버튼 GameObject

    Image titleImage; // 타이틀 이미지 컴포넌트

    public GameObject timeBar;
    public GameObject timeText;
    TimeController timeCnt;

    // 점수 추가
    public GameObject scoreText;
    public static int totalScore = 0;
    public int stageScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("InactiveImage", 1.0f); // 이미지 숨기기
        panel.SetActive(false); // 버튼(패널)을 숨기기

        // 버튼 이벤트 등록

        restartButton.GetComponent<Button>().onClick.AddListener(HandleRestartButton);
        nextButton.GetComponent<Button>().onClick.AddListener(HandleNextButton);

        timeCnt = GetComponent<TimeController>();
        if (timeCnt != null)
        {
            if(timeCnt.gameTime == 0.0f)
            {
                timeBar.SetActive(false); // 타임바 숨기기
            }
        }
        UpdateScore(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.gameState == "gameclear")
        {
            mainImage.SetActive(true); //이미지 표시
            panel.SetActive(true);
            // Restart 버튼 비활성화
            Button bt = restartButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameClearSpr; // 게임 클리어 이미지로 변경
            PlayerController.gameState = "gameend";

            // 시간 제한 추가

            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true;

                int time = (int)timeCnt.displayTime;
                totalScore += time * 10;
            }

            totalScore += stageScore;
            stageScore = 0;
            UpdateScore();

        }
        else if (PlayerController.gameState == "gameover")
        {
            mainImage.SetActive(true);
            panel.SetActive(true);
            // Next 버튼 비활성화
            Button bt = nextButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameOverSpr; // 게임 오버 이미지로 변경
            PlayerController.gameState = "gameend";

            // 시간 제한 추가

            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true;
            }
        }
        else if (PlayerController.gameState == "Playing")
        {
            // 게임중
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            PlayerController playerCnt = player.GetComponent<PlayerController>();
            // 시간 갱신

            if (timeCnt != null)
            {
                if (timeCnt.gameTime > 0.0f)
                {
                    int time = (int)timeCnt.displayTime; 
                    timeText.GetComponent<Text>().text = time.ToString(); // 시간 표시
                    if (time == 0)
                    {
                        playerCnt.GameOver(); // 시간 초과시 게임 오버
                    }
                }
            }

            if (playerCnt.score != 0)
            {
                stageScore += playerCnt.score;
                playerCnt.score = 0;
                UpdateScore(); // 점수 갱신;
            }
        }


    }

    private void OnDestroy()
    {
        // 버튼 이벤트 제거 (이벤트를 등록했다면 제거하는 코드도 넣는게 좋다.)
        restartButton.GetComponent<Button>().onClick.RemoveListener(HandleRestartButton);
        nextButton.GetComponent<Button>().onClick.RemoveListener(HandleNextButton);
    }

    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    void UpdateScore()
    {
        int score = stageScore + totalScore;
        scoreText.GetComponent<Text>().text = score.ToString(); // 점수 표시
    }

    void HandleRestartButton()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex; 
        SceneManager.LoadScene(currentScene);
        Debug.Log("재시작 씬" + currentScene);
    }

    void HandleNextButton()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int lastScene = SceneManager.sceneCountInBuildSettings - 1;

        if (currentScene == lastScene)
        {
            Debug.Log("현재 스테이지가 마지막 스테이지입니다.");
            SceneManager.LoadScene(0);// 첫 번째 씬으로 이동
        }
        else
        {
            SceneManager.LoadScene(currentScene + 1); // 다음 씬으로 이동
            Debug.Log("다음 씬으로 이동: " + (currentScene + 1));
        }
    }
}
