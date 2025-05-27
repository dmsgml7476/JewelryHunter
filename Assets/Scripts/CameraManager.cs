using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float leftLimit = 0.0f;
    public float rightLimit = 0.0f;
    public float topLimit = 0.0f;
    public float bottomLimit = 0.0f;

    public GameObject subScreen;

    public bool isForceScrollX = false;
    public float forceScrollSpeedX = 0.5f;
    public bool isForceScrollY = false;
    public float forceScrollSpeedY = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float x = player.transform.position.x;
            float y = player.transform.position.y;
            float z = transform.position.z;

            if (isForceScrollX)
            {
                x = transform.position.x + (forceScrollSpeedX * Time.deltaTime);
                // Time.deltaTime 이란? 프레임 간격을 나타내는 값으로, 게임의 프레임 레이트에 따라 달라진다. 
                // 즉, 프레임 레이트가 높을수록 Time.deltaTime 값은 작아지고, 프레임 레이트가 낮을수록 Time.deltaTime 값은 커진다.
                // 따라서, Time.deltaTime을 곱해주면 프레임 레이트에 상관없이 일정한 속도로 움직일 수 있다.
                // 컴퓨터의 성능에 따라 프레임 레이트가 달라지기 때문에, Time.deltaTime을 사용하여 게임의 움직임을 일정하게 유지할 수 있다.
                // 즉, 컴퓨터의 성능이 좋은 사람은 60fps로 게임을 실행할 수 있고, 성능이 낮은 사람은 30fps로 게임을 실행할때 deltaTime을 사용하면 움직이는 횟수는 다르지만
                // 속도는 동일하게 유지할 수 있다.
            }

            if (x < leftLimit)
            {
                x = leftLimit;
            }

            else if (x > rightLimit)
            {
                x = rightLimit;
            }

            if (isForceScrollY)
            {
                y = transform.position.y + (forceScrollSpeedY * Time.deltaTime);
            }

            if (y < bottomLimit)
            {
                y = bottomLimit;
            }
            else if (y > topLimit)
            {
                y = topLimit;
            }

            Vector3 v3 = new Vector3(x, y, z);
            transform.position = v3;

            if (subScreen != null)
            {
                y = subScreen.transform.position.y;
                z = subScreen.transform.position.z;
                Vector3 v = new Vector3(x / 2.0f, y, z);
                subScreen.transform.position = v;
            }
        }

    }
}
