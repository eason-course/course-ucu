using UnityEngine;
using UnityEngine.UI;

namespace Step2
{
    // 敵人：被打會扣血，頭上的血條跟著變短，血量歸零就消失
    public class Enemy : MonoBehaviour
    {
        public int maxHp = 100;   // 最大血量
        public Slider hpBar;      // 頭上的血條

        int hp;   // 目前血量

        void Start()
        {
            hp = maxHp;
            hpBar.maxValue = maxHp;
            hpBar.value = hp;
        }

        // 讓血條永遠面向攝影機
        void LateUpdate()
        {
            hpBar.transform.forward = Camera.main.transform.forward;
        }

        // 被打的時候由別人呼叫
        public void TakeDamage(int damage)
        {
            hp -= damage;
            hpBar.value = hp;

            if (hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
