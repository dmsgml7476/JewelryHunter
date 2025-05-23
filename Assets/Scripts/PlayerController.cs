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


    //�ִϸ��̼�

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
    // : ��� �ǹ�. using : ����Ʈ.
    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>(); // this : ���� ��ũ��Ʈ�� �پ��ִ� ������Ʈ�� �ǹ�. GetComponent : ������Ʈ�� �������� �޼ҵ�.  
        // Animator : �ִϸ��̼��� �����ϴ� ������Ʈ. GetComponent : ������Ʈ�� �������� �޼ҵ�.
        animator = GetComponent<Animator>();
        nowAnime = stopAnime;
        oldAnime = stopAnime;

        gameState = "playing"; // ���� ��
    }

    // Update is called once per frame
    void Update()
    {

        if (gameState == "playing")
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
        if (gameState != "playing")
        {
            return;
        }
        // ���� ����
        onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.1f), groundLayer); // Linecast : �� ���� �����ϴ� ���� �׸��� �޼ҵ�. transform.position : ���� ������Ʈ�� ��ġ�� �ǹ�. transform.up : ������Ʈ�� ���� ������ �ǹ�. 0.1f : 0.1��ŭ �Ʒ��� �̵�.
      
        if(onGround || axisH != 0)
        {
            // �ӵ� �����ϱ�
            rbody.velocity = new Vector2(speed * axisH, rbody.velocity.y); // rbody : Rigidbody2D�� ����. velocity : �ӵ��� �ǹ�. new Vector2 : 2���� ���͸� �����ϴ� �޼ҵ�. axisH * 3.0f : �������� �ӵ��� 3��� ������Ű�� ��. rbody.velocity.y : y���� �ӵ�.
            // y�� ���� ���⼭ �������� �ʰ� �ٸ������� �����ϴ� y���Ͱ��� �״�� ����϶�� ��
            // y�� ������ ����!!!
        }
        if(onGround && goJump)
        {

            Debug.Log("����");
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
        Debug.Log("������ư ����!");
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

