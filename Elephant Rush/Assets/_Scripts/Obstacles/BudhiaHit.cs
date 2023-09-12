using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
            rigidbody.AddForce(Vector3.forward * force);
            animator.SetTrigger("Hit");
            StartCoroutine(QuitBudhiaMovement());
        }
    }

    private IEnumerator QuitBudhiaMovement()
    {
        yield return new WaitForSeconds(1f);
        rigidbody.velocity = Vector3.zero;
    }
}
