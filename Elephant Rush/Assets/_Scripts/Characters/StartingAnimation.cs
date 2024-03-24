using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_ANALYTICS
using UnityEngine.Analytics;
#endif
#if UNITY_PURCHASING
using UnityEngine.Purchasing;
#endif


public class StartingAnimation : MonoBehaviour
{
    [SerializeField] private GameObject effSleep;
    [SerializeField] private SkinnedMeshRenderer eyeMesh;
    [SerializeField] private Material eyeMaterialCrying;
    [SerializeField] private Animator startingAnimator;
    [SerializeField] private Animator logoImageAnimator;
    [SerializeField] private Slider loadingSlider;

    private float runningSpeed = 0.0f;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        startingAnimator.SetTrigger("PatakaPhoota");
        effSleep.SetActive(false);
        eyeMesh.material = eyeMaterialCrying;
        logoImageAnimator.SetTrigger("StartGame");
        StartCoroutine(TriggerElephant());
    }

    IEnumerator TriggerElephant()
    {
        yield return new WaitForSeconds(0.4f);
        runningSpeed = 2.5f;
        StartCoroutine(StartGame());
    }

    private void Update()
    {
        transform.position += transform.forward * runningSpeed * Time.deltaTime; 
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1.0f);

        loadingSlider.gameObject.SetActive(true);

        if (PlayerData.instance.ftueLevel == 0)
        {
            PlayerData.instance.ftueLevel = 1;
            PlayerData.instance.Save();
#if UNITY_ANALYTICS
            AnalyticsEvent.FirstInteraction("start_button_pressed");
#endif
        }

#if UNITY_PURCHASING
        var module = StandardPurchasingModule.Instance();
#endif

        StartCoroutine(LoadAsynchronously());
    }

    IEnumerator LoadAsynchronously()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("main");
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            loadingSlider.value = progress;

            yield return null;
        }
    }
}
