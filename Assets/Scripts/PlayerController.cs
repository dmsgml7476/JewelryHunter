using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rbody;  //Rigidbody2D : 물리 엔진을 사용하기 위한 컴포넌트 Rigidbody2D형 변수
    float axisH = 0.0f;  // float형 변수 axisH 초기화
    public float speed = 3.0f; // public : 다른 스크립트에서 접근 가능. float형 변수 speed 초기화

    public float jump = 9.0f;
    public LayerMask groundLayer; // LayerMask : 레이어를 사용하기 위한 컴포넌트. groundLayer : 땅 레이어를 의미.
    bool goJump = false;
    bool onGround = false; // onGround : 땅에 있는지 여부를 의미.


    //애니메이션

    Animator animator;
    public string stopAnime = "PlayerStop";
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    public string goalAnime = "PlayerGoal";
    public string deadAnime = "PlayerOver";
    string nowAnime = "";
    string oldAnime = "";

    public static string gameState = "Playing";

    // Start is called before the first frame update
    // : 상속 의미. using : 임포트.
    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>(); // this : 현재 스크립트가 붙어있는 오브젝트를 의미. GetComponent : 컴포넌트를 가져오는 메소드.  
        // Animator : 애니메이션을 제어하는 컴포넌트. GetComponent : 컴포넌트를 가져오는 메소드.
        animator = GetComponent<Animator>();
        nowAnime = stopAnime;
        oldAnime = stopAnime;

        gameState = "playing"; // 게임 중
    }

    // Update is called once per frame
    void Update()
    {

        if (gameState == "playing")
        {
            return;
        }

        axisH = Input.GetAxisRaw("Horizontal"); // Input : 유니티에서 제공하는 입력을 받는 클래스. GetAxis : 입력을 받아오는 메소드. "Horizontal" : 수평축을 의미.
        // 방향조절
        if (axisH > 0.0f) // axisH가 0보다 크면
        {
            Debug.Log("오른쪽으로 이동");
            transform.localScale = new Vector2(1, 1);// transform : 오브젝트의 위치, 회전, 크기를 조절하는 컴포넌트. localScale : 로컬 스케일을 의미. new Vector3 : 3차원 벡터를 생성하는 메소드.
        }
        else if (axisH < 0.0f) // axisH가 0보다 작으면
        {
            Debug.Log("왼쪽으로 이동");
            transform.localScale = new Vector2(-1, 1); // x축 방향을 반대로 바꿈
        }

        // 캐릭터 점프

        if (Input.GetButtonDown("Jump"))
        {
            Jump(); // jump가 true면 점프를 의미.
        }

        
    }

    // FixedUpdate는 물리 연산을 위한 메소드로, 매 프레임마다 호출됨
    void FixedUpdate()
    {
        if (gameState != "playing")
        {
            return;
        }
        // 착지 판정
        onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.1f), groundLayer); // Linecast : 두 점을 연결하는 선을 그리는 메소드. transform.position : 현재 오브젝트의 위치를 의미. transform.up : 오브젝트의 위쪽 방향을 의미. 0.1f : 0.1만큼 아래로 이동.
      
        if(onGround || axisH != 0)
        {
            // 속도 갱신하기
            rbody.velocity = new Vector2(speed * axisH, rbody.velocity.y); // rbody : Rigidbody2D형 변수. velocity : 속도를 의미. new Vector2 : 2차원 벡터를 생성하는 메소드. axisH * 3.0f : 수평축의 속도를 3배로 증가시키는 것. rbody.velocity.y : y축의 속도.
            // y의 값은 여기서 조정하지 않고 다른곳에서 적용하는 y벡터값을 그대로 사용하라는 뜻
            // y는 기존값 유지!!!
        }
        if(onGround && goJump)
        {

            Debug.Log("점프");
            Vector2 jumpPw = new Vector2(0, jump);
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);
            goJump = false;
        }

        if (onGround)
        {
            if(axisH == 0)
            {
                nowAnime = stopAnime;
            }
            else
            {
                nowAnime = moveAnime;
            }
        }
        else
        {
            nowAnime = jumpAnime;
        }

        if (nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime);
        }
        
    }

    public void Jump()
    {
        goJump = true;
        Debug.Log("점프버튼 눌림!");
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Goal")
        {
            Goal();
        }
        else if(collision.gameObject.tag == "Dead")
        {
            GameOver();
        }

    }

    public void Goal()
    {
        animator.Play(goalAnime);
    }

    public void GameOver()
    {
        animator.Play(deadAnime); 
    }
}

