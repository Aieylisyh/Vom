
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
            host.attack.Attacked();
        }
    }
}