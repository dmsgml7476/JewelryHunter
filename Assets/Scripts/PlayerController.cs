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

    // 이중 점프 - 시작 박은희 2023.10.16
    int jumpCount = 0; // 점프 횟수 초기화
    public int maxJumpCount = 2; // 최대 점프 횟수


    //애니메이션

    Animator animator;  // 애니메이션을 제어하는 컴포넌트. 애니메이션을 제어하기 위한 변수.
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

        gameState = "Playing"; // 게임 중
    }

    // Update is called once per frame
    void Update()
    {

        if (gameState != "Playing")
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
        if (gameState != "Playing")
        {
            return;
        }
        //// 착지 판정 코드 1
        //onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.1f), groundLayer); // Linecast : 두 점을 연결하는 선을 그리는 메소드. transform.position : 현재 오브젝트의 위치를 의미. transform.up : 오브젝트의 위쪽 방향을 의미. 0.1f : 0.1만큼 아래로 이동.
        //// 타겟지점과 포지션의 거리(transform.position - (transform.up * 0.1f))를 비교하여 땅에 닿아있는지 확인하는 것.
        //// transform.up : 오브젝트의 위쪽 방향을 의미. 0.1f : 0.1만큼 아래로 이동.
        //// 단위벡터(1과 0으로 이루어져서 방향만 존재하는 벡터) * speed: 진짜 벡터(방향과 크기를 모두 가진 벡터)로 변환하는 것.


        // 착지 판정 코드 2

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer); // RaycastHit2D : 레이캐스트의 충돌 정보를 저장하는 구조체. Physics2D.Raycast : 레이캐스트를 쏘는 메소드. transform.position : 현재 오브젝트의 위치를 의미. transform.down : 오브젝트의 아래쪽 방향을 의미. 0.1f : 0.1만큼 아래로 이동.
        onGround = hit.collider != null;
        // hit.collider : 레이캐스트가 충돌한 콜라이더를 의미. != null : 충돌한 콜라이더가 없으면 false, 있으면 true를 의미.
        // 충돌한 콜라이더가 무엇인지를 알려줄 수 있어서 활용도가 더 높다.

        if (onGround || axisH != 0)
        {
            // 속도 갱신하기
            rbody.velocity = new Vector2(speed * axisH, rbody.velocity.y); // rbody : Rigidbody2D형 변수. velocity : 속도를 의미. new Vector2 : 2차원 벡터를 생성하는 메소드. axisH * 3.0f : 수평축의 속도를 3배로 증가시키는 것. rbody.velocity.y : y축의 속도.
            // y의 값은 여기서 조정하지 않고 다른곳에서 적용하는 y벡터값을 그대로 사용하라는 뜻
            // y는 기존값 유지!!!

        }
        //if (onGround && goJump)
        //{

        //    Debug.Log("점프");
        //    Vector2 jumpPw = new Vector2(0, jump);
        //    rbody.AddForce(jumpPw, ForceMode2D.Impulse);
        //    goJump = false;
        //}

        // 이중점프

        if (onGround)
        {
            jumpCount = 0;
        }


        //if (goJump && jumpCount < maxJumpCount)
        //{
        //    rbody.AddForce(new Vector2(0, jump), ForceMode2D.Impulse);
        //    goJump = false; // 점프가 끝나면 goJump를 false로 바꿈
        //    jumpCount++; // 점프 횟수 증가
        //}

        if (goJump)
        {
            rbody.AddForce(new Vector2(0, jump), ForceMode2D.Impulse);
            goJump = false; // 점프가 끝나면 goJump를 false로 바꿈
        }

        if (onGround)
        {
            if (axisH == 0)
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
        //animator.Play(nowAnime);

        if (nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime);
        }

    }

    public void Jump()
    {
        //goJump = true;
        //Debug.Log("점프버튼 눌림!");

        jumpCount++; // 점프 횟수 증가
        if (jumpCount < maxJumpCount)
        {
            goJump= true; // 점프를 의미.
        }
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            Goal();
        }
        else if (collision.gameObject.tag == "Dead")
        {
            GameOver();
        }

    }

    // 위에는 isTrigger를 사용한 충돌 처리 코드입니다.
    // 아래는 isTrigger가 아닌 일반적인 충돌 처리도 가능하도록 추가한 코드입니다.
    // 충돌 감지와 더불어 이동을 막아줌. 하지만 비쌈. 시스템에 부하를 줌

    //public void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log("OnCollisionEnter2D 충돌 이벤트 발생");
    //    if (collision.gameObject.tag == "Goal")
    //    {
    //        Goal();
    //    }
    //    else if (collision.gameObject.tag == "Dead")
    //    {
    //        GameOver();
    //    }
    //}

    public void Goal()
    {
        animator.Play(goalAnime);
        gameState = "gameclear"; // 게임 클리어
        GameStop(); // 게임 정지
    }

    public void GameOver()
    {
        animator.Play(deadAnime); 

        gameState = "gameover";
        GameStop(); // 게임 정지

        // 게임 오버 연출

        GetComponent<CapsuleCollider2D>().enabled = false;

        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
    }

    void GameStop()
    {
        //Rigidbody2D 가져오기
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        rbody.velocity = new Vector2(0, 0);
    }
}

