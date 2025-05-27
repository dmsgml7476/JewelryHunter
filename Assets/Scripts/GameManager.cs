using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // �� ���� ���ӽ����̽� �߰�

public class GameManager : MonoBehaviour
{
    public GameObject mainImage;  // image�� ���� GameObject
    public Sprite gameOverSpr; // ���� ���� �̹���
    public Sprite gameClearSpr; // ���� Ŭ���� �̹���
    public GameObject panel;// �г� GameObject
    public GameObject restartButton; // Restart ��ư GameObject
    public GameObject nextButton; // Next ��ư GameObject

    Image titleImage; // Ÿ��Ʋ �̹��� ������Ʈ

    public GameObject timeBar;
    public GameObject timeText;
    TimeController timeCnt;

    // ���� �߰�
    public GameObject scoreText;
    public static int totalScore = 0;
    public int stageScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("InactiveImage", 1.0f); // �̹��� �����
        panel.SetActive(false); // ��ư(�г�)�� �����

        // ��ư �̺�Ʈ ���

        restartButton.GetComponent<Button>().onClick.AddListener(HandleRestartButton);
        nextButton.GetComponent<Button>().onClick.AddListener(HandleNextButton);

        timeCnt = GetComponent<TimeController>();
        if (timeCnt != null)
        {
            if(timeCnt.gameTime == 0.0f)
            {
                timeBar.SetActive(false); // Ÿ�ӹ� �����
            }
        }
        UpdateScore(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.gameState == "gameclear")
        {
            mainImage.SetActive(true); //�̹��� ǥ��
            panel.SetActive(true);
            // Restart ��ư ��Ȱ��ȭ
            Button bt = restartButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameClearSpr; // ���� Ŭ���� �̹����� ����
            PlayerController.gameState = "gameend";

            // �ð� ���� �߰�

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
            // Next ��ư ��Ȱ��ȭ
            Button bt = nextButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameOverSpr; // ���� ���� �̹����� ����
            PlayerController.gameState = "gameend";

            // �ð� ���� �߰�

            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true;
            }
        }
        else if (PlayerController.gameState == "Playing")
        {
            // ������
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            PlayerController playerCnt = player.GetComponent<PlayerController>();
            // �ð� ����

            if (timeCnt != null)
            {
                if (timeCnt.gameTime > 0.0f)
                {
                    int time = (int)timeCnt.displayTime; 
                    timeText.GetComponent<Text>().text = time.ToString(); // �ð� ǥ��
                    if (time == 0)
                    {
                        playerCnt.GameOver(); // �ð� �ʰ��� ���� ����
                    }
                }
            }

            if (playerCnt.score != 0)
            {
                stageScore += playerCnt.score;
                playerCnt.score = 0;
                UpdateScore(); // ���� ����;
            }
        }


    }

    private void OnDestroy()
    {
        // ��ư �̺�Ʈ ���� (�̺�Ʈ�� ����ߴٸ� �����ϴ� �ڵ嵵 �ִ°� ����.)
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
        scoreText.GetComponent<Text>().text = score.ToString(); // ���� ǥ��
    }

    void HandleRestartButton()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex; 
        SceneManager.LoadScene(currentScene);
        Debug.Log("����� ��" + currentScene);
    }

    void HandleNextButton()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int lastScene = SceneManager.sceneCountInBuildSettings - 1;

        if (currentScene == lastScene)
        {
            Debug.Log("���� ���������� ������ ���������Դϴ�.");
            SceneManager.LoadScene(0);// ù ��° ������ �̵�
        }
        else
        {
            SceneManager.LoadScene(currentScene + 1); // ���� ������ �̵�
            Debug.Log("���� ������ �̵�: " + (currentScene + 1));
        }
    }
}
