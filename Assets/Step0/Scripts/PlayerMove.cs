using UnityEngine;

namespace Step0
{
    // 用方向鍵讓角色移動，並把輸入值交給 Animator 播放對應的動作
    public class PlayerMove : MonoBehaviour
    {
        public float speed = 2.6f;          // 移動速度(每秒幾公尺)
        public Animator animator;           // 角色身上的 Animator
        public Transform cameraTransform;   // 攝影機，角色會面向它看的方向

        void Update()
        {
            // 讀取輸入：GetAxis 的值會在 -1 ~ 1 之間慢慢變化，不是一按就跳到 1
            float h = Input.GetAxis("Horizontal");   // 左 -1、右 +1
            float v = Input.GetAxis("Vertical");     // 下 -1、上 +1

            // 角色轉向攝影機看的方向(只轉水平，不會往上下傾斜)
            transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);

            // 移動方向：x 是左右、z 是前後
            Vector3 direction = new Vector3(h, 0, v);
            direction = Vector3.ClampMagnitude(direction, 1f);   // 斜走時長度不超過 1，才不會比直走快

            // Translate 預設以角色自己的方向移動，所以「前」就是角色面對的方向
            transform.Translate(direction * speed * Time.deltaTime);

            // 把兩個值交給 Animator，Blend Tree 會依照它們混合動作
            animator.SetFloat("Horizontal", h);
            animator.SetFloat("Vertical", v);
        }
    }
}
