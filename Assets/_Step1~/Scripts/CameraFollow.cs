using UnityEngine;

namespace Step1
{
    // 攝影機跟著玩家，移動滑鼠可以繞著玩家轉
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;          // 要跟隨的玩家
        public float distance = 5f;       // 跟玩家的距離
        public float height = 1.5f;       // 看向玩家身上的高度(大約胸口)
        public float mouseSpeed = 3f;     // 滑鼠靈敏度
        public float minPitch = -10f;     // 最低仰角
        public float maxPitch = 60f;      // 最高仰角

        float yaw;           // 左右角度
        float pitch = 20f;   // 上下角度

        void Start()
        {
            yaw = target.eulerAngles.y;                  // 一開始從玩家背後看
            Cursor.lockState = CursorLockMode.Locked;    // 鎖定並隱藏滑鼠游標(按 Esc 解除)
        }

        // LateUpdate 在所有 Update 之後執行：玩家移動完才更新攝影機，畫面才不會抖
        void LateUpdate()
        {
            // 滑鼠左右 → 繞圈，滑鼠上下 → 仰角
            yaw += Input.GetAxis("Mouse X") * mouseSpeed;
            pitch -= Input.GetAxis("Mouse Y") * mouseSpeed;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);   // 限制角度，不會轉到地板下面

            // 從玩家胸口往後退 distance 的距離，再看向玩家
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            Vector3 lookPoint = target.position + Vector3.up * height;
            transform.position = lookPoint - rotation * Vector3.forward * distance;
            transform.rotation = rotation;
        }
    }
}
