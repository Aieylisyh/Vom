
namespace vom
{
    public class EnemyAnimationEventResolver : VomEnemyComponent
    {
        public void Moved()
        {
            host.move.Moved();
        }

        public void Attacked()
        {
            UnityEngine.Debug.Log("Attacked");
            host.attack.Attacked();
        }
    }
}