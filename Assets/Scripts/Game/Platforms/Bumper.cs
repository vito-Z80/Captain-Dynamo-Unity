using System.Collections;
using UnityEngine;

namespace Game.Platforms
{
    public class Bumper : MonoBehaviour
    {
        public float bounceForce = 100f;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            StartCoroutine(ActiveState(collision));
            // Определите направление от bumper к персонажу
            Vector3 bounceDirection = (collision.gameObject.transform.position - transform.position).normalized;

            // Примените силу отскока к персонажу
            collision.rigidbody.velocity = bounceDirection * bounceForce;
        }


        private IEnumerator ActiveState(Collision2D collision)
        {
            var hc = collision.gameObject.GetComponent<HeroController>();
            hc.isActive = false;
            yield return new WaitForSeconds(0.2f);
            hc.isActive = true;
            yield return null;
        }
    }
}