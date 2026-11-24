using UnityEngine;

namespace Step2
{
    // 掛在劍上：劍身碰到敵人就扣血
    // 劍上要有 Box Collider(勾 Is Trigger)和 Rigidbody(勾 Is Kinematic)
    // Collider 什麼時候打開由 PlayerAttack 決定，所以這裡只要管「碰到誰」
    public class SwordHit : MonoBehaviour
    {
        public int damage = 10;   // 每一刀扣多少血

        // 劍的 Trigger 碰到別的 Collider 時，Unity 會自動呼叫這個函式
        void OnTriggerEnter(Collider other)
        {
            // 碰到的東西(或它的父物件)是敵人才扣血
            Enemy enemy = other.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
