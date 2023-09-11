using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BudhiaHit : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float force = 100f;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 newForce = new Vector3(0, 0, force);
            rigidbody.AddForce(newForce, ForceMode.Impulse);
            animator.SetTrigger("Hit");
        }
    }
}
