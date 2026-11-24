using UnityEngine;

namespace Step2
{
    // 按滑鼠左鍵揮劍：連按可以接成三段攻擊(第幾段由 Animator 的轉場決定)
    // 劍的 Collider 平常關著，只有揮下去的那幾格才打開，由攻擊動作上的 Animation Event 控制
    public class PlayerAttack : MonoBehaviour
    {
        public Animator animator;        // 角色身上的 Animator
        public Collider swordCollider;   // 劍上的 Collider

        void Start()
        {
            swordCollider.enabled = false;   // 一開始先關掉，不是揮砍的時候碰到不算
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger("Attack");
            }
        }

        // Animation Event 呼叫：劍開始揮下去，打開判定
        public void HitStart()
        {
            swordCollider.enabled = true;
        }

        // Animation Event 呼叫：揮完了，關掉判定
        public void HitEnd()
        {
            swordCollider.enabled = false;
        }
    }
}
