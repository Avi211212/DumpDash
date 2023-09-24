using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StartingAnimation : MonoBehaviour
{
    [SerializeField] private GameObject effSleep;
    [SerializeField] private SkinnedMeshRenderer eyeMesh;
    [SerializeField] private Material eyeMaterialCrying;
    [SerializeField] private Animator startingAnimator;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(7);
        startingAnimator.SetTrigger("PatakaPhoota");
        effSleep.SetActive(false);
        eyeMesh.material = eyeMaterialCrying;
    }
}
