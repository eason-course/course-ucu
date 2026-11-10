using UnityEngine;

// 用方向鍵讓角色移動、空白鍵跳躍，並把狀態交給 Animator 播放對應的動作
// 用 CharacterController 移動：會被箱子擋住，也能站在箱子上
[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    public float speed = 2.6f;          // 移動速度(每秒幾公尺)
    public float jumpSpeed = 4.7f;      // 起跳速度(每秒幾公尺)，越大跳越高
    public float gravity = 9.2f;        // 重力(每秒減少多少垂直速度)
    public Animator animator;           // 角色身上的 Animator
    public Transform cameraTransform;   // 攝影機，角色會面向它看的方向

    CharacterController controller;
    float verticalSpeed;     // 垂直速度：正的往上、負的往下

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 讀取輸入：GetAxis 的值會在 -1 ~ 1 之間慢慢變化，不是一按就跳到 1
        float h = Input.GetAxis("Horizontal");   // 左 -1、右 +1
        float v = Input.GetAxis("Vertical");     // 下 -1、上 +1

        // 角色轉向攝影機看的方向(只轉水平，不會往上下傾斜)
        transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);

        // 水平方向：以角色自己的方向為準，左右是 right、前後是 forward
        Vector3 direction = transform.right * h + transform.forward * v;
        direction = Vector3.ClampMagnitude(direction, 1f);   // 斜走時長度不超過 1，才不會比直走快

        // 站在地上(地板或箱子)
        if (controller.isGrounded)
        {
            verticalSpeed = -1f;   // 給一點向下的速度，讓角色貼住地面，isGrounded 才會穩定

            // 跳躍：按空白鍵給一個向上的初速
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalSpeed = jumpSpeed;
                animator.SetTrigger("Jump");
            }
        }

        // 重力：每幀扣掉一點垂直速度
        verticalSpeed -= gravity * Time.deltaTime;

        // 水平和垂直合在一起移動，撞到東西會被擋住
        Vector3 velocity = direction * speed + Vector3.up * verticalSpeed;
        controller.Move(velocity * Time.deltaTime);

        // 把狀態交給 Animator：Blend Tree 用 h、v 混合跑步動作，Grounded 決定何時落地
        animator.SetFloat("Horizontal", h);
        animator.SetFloat("Vertical", v);
        animator.SetBool("Grounded", controller.isGrounded);
    }
}
