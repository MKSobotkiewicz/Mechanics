using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Mechanics
{
    public class Projectile : MonoBehaviour
    {
        public float Velocity;
        public float Penetration;
        public GameObject Explosion;

        private new Rigidbody rigidbody;
        private Vector3 enterPoint = new Vector3();
        private Vector3 exitPoint = new Vector3();
        private float remainingPenetration;

        public void Start()
        {
            transform.parent = null;
            rigidbody = GetComponent<Rigidbody>();
            rigidbody.AddRelativeForce(new Vector3(0, 0, Velocity),ForceMode.VelocityChange);
            rigidbody.AddRelativeTorque(new Vector3(0, 0, Velocity/10), ForceMode.VelocityChange);
            remainingPenetration = Penetration;
        }

        public void Explode(Vector3 upNormal)
        {
            var explosion = Instantiate(Explosion, transform);
            explosion.transform.rotation = Quaternion.LookRotation(transform.forward, upNormal);
            explosion.transform.parent = null;
            Destroy(gameObject);
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.gameObject.layer== LayerMask.NameToLayer("Armor"))
            {
                var contact = collision.GetContact(0);
                var point = contact.point;
                for (int i = 0; i < 100; i++)
                {
                    var newPoint = point - contact.normal * 0.01f;
                    if (collision.collider.bounds.Contains(newPoint))
                    {
                        point = newPoint;
                    }
                    else
                    {
                        transform.position = newPoint;
                        break;
                    }
                }
                var depth = Vector3.Distance(contact.point,point);
                remainingPenetration -= depth;
                if (remainingPenetration < 0)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Explode(collision.GetContact(0).normal);
            }
        }
    }
}
