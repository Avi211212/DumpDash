using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firecracker : MonoBehaviour
{
    [SerializeField] private GameObject meshRenderer;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        meshRenderer.SetActive(false);
    }
}