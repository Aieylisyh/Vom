using UnityEngine;
using com;
namespace game
{
    public class PhantomEnemyMove : EnemyMove
    {
        public Transform trans_phantom;
        public RotateAlignMove rotateAlignMove_phantom;
        public float mirrorX;

       /* protected override void Translate(Vector3 d)
        {
            rotateAlignMove?.Rotate(d);
            transform.position += d * Speed * Time.deltaTime;

            var d_phantom = d;
            d_phantom.x = -d.x;
            rotateAlignMove_phantom?.Rotate(d_phantom);
            var p_phantom = transform.position;
            p_phantom.x = 2 * mirrorX - transform.position.x;
            trans_phantom.position = p_phantom;
        }*/
    }
}
