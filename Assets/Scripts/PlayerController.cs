using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rbody;  //Rigidbody2D : ���� ������ ����ϱ� ���� ������Ʈ Rigidbody2D�� ����
    float axisH = 0.0f;  // float�� ���� axisH �ʱ�ȭ
    public float speed = 3.0f; // public : �ٸ� ��ũ��Ʈ���� ���� ����. float�� ���� speed �ʱ�ȭ

    public float jump = 9.0f;
    public LayerMask groundLayer; // LayerMask : ���̾ ����ϱ� ���� ������Ʈ. groundLayer : �� ���̾ �ǹ�.
    bool goJump = false;
    bool onGround = false; // onGround : ���� �ִ��� ���θ� �ǹ�.

    // ���� ���� - ���� ������ 2023.10.16
    int jumpCount = 0; // ���� Ƚ�� �ʱ�ȭ
    public int maxJumpCount = 2; // �ִ� ���� Ƚ��


    //�ִϸ��̼�

    Animator animator;  // �ִϸ��̼��� �����ϴ� ������Ʈ. �ִϸ��̼��� �����ϱ� ���� ����.
    public string stopAnime = "PlayerStop";
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    public string goalAnime = "PlayerGoal";
    public string deadAnime = "PlayerOver";
    string nowAnime = "";
    string oldAnime = "";

    public static string gameState = "Playing";

    // Start is called before the first frame update
    // : ��� �ǹ�. using : ����Ʈ.
    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>(); // this : ���� ��ũ��Ʈ�� �پ��ִ� ������Ʈ�� �ǹ�. GetComponent : ������Ʈ�� �������� �޼ҵ�.  
        // Animator : �ִϸ��̼��� �����ϴ� ������Ʈ. GetComponent : ������Ʈ�� �������� �޼ҵ�.
        animator = GetComponent<Animator>();
        nowAnime = stopAnime;
        oldAnime = stopAnime;

        gameState = "Playing"; // ���� ��
    }

    // Update is called once per frame
    void Update()
    {

        if (gameState != "Playing")
        {
            return;
        }

        axisH = Input.GetAxisRaw("Horizontal"); // Input : ����Ƽ���� �����ϴ� �Է��� �޴� Ŭ����. GetAxis : �Է��� �޾ƿ��� �޼ҵ�. "Horizontal" : �������� �ǹ�.
        // ��������
        if (axisH > 0.0f) // axisH�� 0���� ũ��
        {
            Debug.Log("���������� �̵�");
            transform.localScale = new Vector2(1, 1);// transform : ������Ʈ�� ��ġ, ȸ��, ũ�⸦ �����ϴ� ������Ʈ. localScale : ���� �������� �ǹ�. new Vector3 : 3���� ���͸� �����ϴ� �޼ҵ�.
        }
        else if (axisH < 0.0f) // axisH�� 0���� ������
        {
            Debug.Log("�������� �̵�");
            transform.localScale = new Vector2(-1, 1); // x�� ������ �ݴ�� �ٲ�
        }

        // ĳ���� ����

        if (Input.GetButtonDown("Jump"))
        {
            Jump(); // jump�� true�� ������ �ǹ�.
        }


    }

    // FixedUpdate�� ���� ������ ���� �޼ҵ��, �� �����Ӹ��� ȣ���
    void FixedUpdate()
    {
        if (gameState != "Playing")
        {
            return;
        }
        //// ���� ���� �ڵ� 1
        //onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.1f), groundLayer); // Linecast : �� ���� �����ϴ� ���� �׸��� �޼ҵ�. transform.position : ���� ������Ʈ�� ��ġ�� �ǹ�. transform.up : ������Ʈ�� ���� ������ �ǹ�. 0.1f : 0.1��ŭ �Ʒ��� �̵�.
        //// Ÿ�������� �������� �Ÿ�(transform.position - (transform.up * 0.1f))�� ���Ͽ� ���� ����ִ��� Ȯ���ϴ� ��.
        //// transform.up : ������Ʈ�� ���� ������ �ǹ�. 0.1f : 0.1��ŭ �Ʒ��� �̵�.
        //// ��������(1�� 0���� �̷������ ���⸸ �����ϴ� ����) * speed: ��¥ ����(����� ũ�⸦ ��� ���� ����)�� ��ȯ�ϴ� ��.


        // ���� ���� �ڵ� 2

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer); // RaycastHit2D : ����ĳ��Ʈ�� �浹 ������ �����ϴ� ����ü. Physics2D.Raycast : ����ĳ��Ʈ�� ��� �޼ҵ�. transform.position : ���� ������Ʈ�� ��ġ�� �ǹ�. transform.down : ������Ʈ�� �Ʒ��� ������ �ǹ�. 0.1f : 0.1��ŭ �Ʒ��� �̵�.
        onGround = hit.collider != null;
        // hit.collider : ����ĳ��Ʈ�� �浹�� �ݶ��̴��� �ǹ�. != null : �浹�� �ݶ��̴��� ������ false, ������ true�� �ǹ�.
        // �浹�� �ݶ��̴��� ���������� �˷��� �� �־ Ȱ�뵵�� �� ����.

        if (onGround || axisH != 0)
        {
            // �ӵ� �����ϱ�
            rbody.velocity = new Vector2(speed * axisH, rbody.velocity.y); // rbody : Rigidbody2D�� ����. velocity : �ӵ��� �ǹ�. new Vector2 : 2���� ���͸� �����ϴ� �޼ҵ�. axisH * 3.0f : �������� �ӵ��� 3��� ������Ű�� ��. rbody.velocity.y : y���� �ӵ�.
            // y�� ���� ���⼭ �������� �ʰ� �ٸ������� �����ϴ� y���Ͱ��� �״�� ����϶�� ��
            // y�� ������ ����!!!

        }
        //if (onGround && goJump)
        //{

        //    Debug.Log("����");
        //    Vector2 jumpPw = new Vector2(0, jump);
        //    rbody.AddForce(jumpPw, ForceMode2D.Impulse);
        //    goJump = false;
        //}

        // ��������

        if (onGround)
        {
            jumpCount = 0;
        }


        //if (goJump && jumpCount < maxJumpCount)
        //{
        //    rbody.AddForce(new Vector2(0, jump), ForceMode2D.Impulse);
        //    goJump = false; // ������ ������ goJump�� false�� �ٲ�
        //    jumpCount++; // ���� Ƚ�� ����
        //}

        if (goJump)
        {
            rbody.AddForce(new Vector2(0, jump), ForceMode2D.Impulse);
            goJump = false; // ������ ������ goJump�� false�� �ٲ�
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
        //Debug.Log("������ư ����!");

        jumpCount++; // ���� Ƚ�� ����
        if (jumpCount < maxJumpCount)
        {
            goJump= true; // ������ �ǹ�.
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

    // ������ isTrigger�� ����� �浹 ó�� �ڵ��Դϴ�.
    // �Ʒ��� isTrigger�� �ƴ� �Ϲ����� �浹 ó���� �����ϵ��� �߰��� �ڵ��Դϴ�.
    // �浹 ������ ���Ҿ� �̵��� ������. ������ ���. �ý��ۿ� ���ϸ� ��

    //public void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log("OnCollisionEnter2D �浹 �̺�Ʈ �߻�");
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
        gameState = "gameclear"; // ���� Ŭ����
        GameStop(); // ���� ����
    }

    public void GameOver()
    {
        animator.Play(deadAnime); 

        gameState = "gameover";
        GameStop(); // ���� ����

        // ���� ���� ����

        GetComponent<CapsuleCollider2D>().enabled = false;

        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
    }

    void GameStop()
    {
        //Rigidbody2D ��������
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        rbody.velocity = new Vector2(0, 0);
    }
}

