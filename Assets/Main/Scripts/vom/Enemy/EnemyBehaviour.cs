using UnityEngine;
using com;

namespace vom
{
    public class EnemyBehaviour : MonoBehaviour
    {
        public Animator animator;

        public Transform circleTrans;

        public float range;
        public int dmg;
        public float attackInterval;
        private float _attackIntervalTimer;

        public Transform spawnSpace;
        public GameObject shootBullet;

        public Transform weaponPos;

        public void Start()
        {
            _attackIntervalTimer = 0;
        }

        public void OnHit(OrbBehaviour orb)
        {
            //CameraShake.instance.Shake(orb.hitShakeLevel);
            animator.SetTrigger("Wound");
        }

        private void Update()
        {
            if (_attackIntervalTimer > 0)
            {
                _attackIntervalTimer -= GameTime.deltaTime;
            }

            var dir = PlayerBehaviour.instance.transform.position - transform.position;
            if (dir.magnitude < range)
            {
                Attack();
            }
        }

        void Attack()
        {
            if (_attackIntervalTimer > 0)
            {
                return;
            }

            _attackIntervalTimer = attackInterval;
            animator.SetTrigger("MeleeAttack");
        }

        public void Attacked()
        {
            SpawnShoot(shootBullet, PlayerBehaviour.instance.transform.position);
        }

        void SpawnShoot(GameObject prefab, Vector3 targetPos)
        {
            GameObject shootGo = Instantiate(prefab, spawnSpace);
            shootGo.SetActive(true);
            shootGo.transform.position = weaponPos.position;

            var shoot = shootGo.GetComponent<OrbBehaviour>();
            shoot.isEnemyShoot = true;
            shoot.dmg = dmg;
            shoot.SetRelease(targetPos);
        }
    }
}