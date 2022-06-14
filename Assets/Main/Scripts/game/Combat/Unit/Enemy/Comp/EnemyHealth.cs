using UnityEngine;
using com;
namespace game
{
    public class EnemyHealth : UnitHealth
    {
        private RectTransform hbRect;
        private float _sizeFactor;
        private int _hitByTorCount;
        private int _hitByBombCount;
        private float _dotTimer;
        private int _dotDamagePerTick;

        public override void ResetState()
        {
            _hitByTorCount = 0;
            _hitByBombCount = 0;
            _dotTimer = 0;
            if (hb == null)
            {
                var go = PoolingService.instance.GetInstance("health bar enemy");
                hb = go.GetComponent<HealthBar>();
                go.transform.SetParent(self.transform);
            }
            if (hb != null)
            {
                hbRect = hb.GetComponent<RectTransform>();
                hbRect.localPosition = new Vector3(0, 0.3f + 0.5f * _sizeFactor, 0);
                hbRect.localScale = new Vector3(0.01f * _sizeFactor, 0.01f, 0.01f);
            }

            base.ResetState();
        }

        protected override void Tick()
        {
            base.Tick();
            RegTick();
            CheckDot();
        }

        public override void RegTick()
        {
            base.RegTick();
        }

        public void Init(float sizeFactor, int hpMax)
        {
            this.hpMax = hpMax;
            this._sizeFactor = sizeFactor;
            _hitByTorCount = 0;
            _hitByBombCount = 0;
            _dotTimer = 0;
        }

        protected override int RefineDamageValue(Damage damage)
        {
            var playerAttri = CombatService.instance.playerAttri;
            //Enemy enemy = self as Enemy;
            var enemyProto = EnemyService.instance.GetPrototype(self);
            int totalAddPercent = 0;
            if (enemyProto.Category.tier == EnemyPrototype.EnemyCategory.Tier.Heavy || enemyProto.Category.tier == EnemyPrototype.EnemyCategory.Tier.Boss)
            {
                if (damage.type == DamageType.Bomb)
                {
                    totalAddPercent += playerAttri.ttModifier.bombDmgAdd_heavyTier;
                }
            }
            else if (enemyProto.Category.tier == EnemyPrototype.EnemyCategory.Tier.Light)
            {
                if (damage.type == DamageType.Bomb)
                {
                    totalAddPercent += playerAttri.ttModifier.bombDmgAdd_lightTier;
                }
            }

            if (damage.type == DamageType.Torpedo)
            {
                //Debug.Log("HealthRatio " + HealthRatio);
                //Debug.Log("torDmgAdd_below40p " + playerAttri.ttModifier.torDmgAdd_below40p);
                //Debug.Log("torDmgAdd_multiHit " + playerAttri.ttModifier.torDmgAdd_multiHit);
                if (HealthRatio <= 0.5f && playerAttri.ttModifier.torDmgAdd_below50p > 0)
                {
                    totalAddPercent += playerAttri.ttModifier.torDmgAdd_below50p;
                }
                if (_hitByTorCount > 0 && playerAttri.ttModifier.torDmgAdd_multiHit > 0)
                {
                    totalAddPercent += playerAttri.ttModifier.torDmgAdd_multiHit;
                }
                _hitByTorCount += 1;
            }

            if (damage.type == DamageType.Bomb)
            {
                if (playerAttri.ttModifier.bombAoeDmg > 0 && _hitByBombCount % 5 == 0)
                {
                    var range = ConfigService.instance.combatConfig.aoeParam.rangeExplodeBomb;
                    CreateAoeDamage(range, MathGame.GetPercentage(damage.value, playerAttri.ttModifier.bombAoeDmg));
                }

                if (HealthRatio > 0.5f)
                {
                    //Debug.Log("bombToFullDmgAdd trigger " + playerAttri.cabModifier.bombToFullDmgAdd);
                    totalAddPercent += playerAttri.cabModifier.detonateDmgAdd;
                }

                _hitByBombCount += 1;
            }

            damage.ModifyPercentage(totalAddPercent);
            var delta = GetDamageAfterArmor(damage);
            return delta;
        }

        protected void CreatePenetrateEffect()
        {
            var pos = self.move.transform.position;
            var go = PoolingService.instance.GetInstance("exp penetrate");
            go.transform.position = pos;
        }

        protected override void postDamaged(Damage damage, int delta)
        {
            OnHitFeedBack(delta);
            var playerAttri = CombatService.instance.playerAttri;
            if (hp <= 0)
            {
                if (damage.type == DamageType.Torpedo)
                {
                    if (playerAttri.ttModifier.torAoeMaxHit > 0)
                    {
                        var rangeTor = ConfigService.instance.combatConfig.aoeParam.rangeTor;
                        CreateAoeToProjectiles(rangeTor, playerAttri.ttModifier.torAoeMaxHit);
                    }
                }
                if (damage.type == DamageType.Bomb)
                {
                    if (playerAttri.ttModifier.bombAoeMaxHit > 0)
                    {
                        var rangeBomb = ConfigService.instance.combatConfig.aoeParam.rangeBomb;
                        CreateAoeToProjectiles(rangeBomb, playerAttri.ttModifier.bombAoeMaxHit);
                    }
                }
            }
            else
            {
                if (damage.type == DamageType.Torpedo)
                {
                    if (playerAttri.ttModifier.torDotPercent > 0)
                    {
                        var dmgDotTotal = (float)damage.value * playerAttri.ttModifier.torDotPercent / 100f;
                        CreateDot(4, dmgDotTotal);
                    }
                }
            }
        }

        protected void CreateAoeDamage(float range, int aoeDmgValue)
        {
            var pos = self.move.transform.position;
            var go = PoolingService.instance.GetInstance("exp small aoe");
            go.transform.position = pos;
            go.transform.rotation = Quaternion.identity;

            var aoeDmg = new Damage(null, aoeDmgValue, DamageType.None, true);
            CombatService.instance.AoeDamageToEnemy(false, aoeDmg, range, pos, self);
        }

        protected void CreateAoeToProjectiles(float range, int maxHit)
        {
            var pos = self.move.transform.position;
            var go = PoolingService.instance.GetInstance("exp aoe");
            go.transform.position = pos;
            go.transform.rotation = Quaternion.identity;
            CombatService.instance.AoeDamageToProjectiles(pos, range, maxHit);
        }

        protected void CreateDot(float sec, float totalDamage)
        {
            //Debug.Log("CreateDot " + sec + " " + totalDamage);
            _dotTimer = sec;
            var dmg = totalDamage / (_dotTimer / TickTime);
            _dotDamagePerTick = Mathf.RoundToInt(dmg);
            //Debug.Log("_dotDamagePerTick " + _dotDamagePerTick);
            if (_dotDamagePerTick < 1)
            {
                _dotDamagePerTick = 1;
            }
        }

        protected void CheckDot()
        {
            if (self.death.isDead)
                return;

            if (_dotTimer <= 0)
                return;

            _dotTimer -= TickTime;
            OnHealthChange(-_dotDamagePerTick);
            shakeBehaviour?.Shake();
        }
    }
}
