using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject loadingPanel;

    [SerializeField] private Color defaultColor;
    [SerializeField] private Color noneColor;

    public void Initialize()
    {
        StartCoroutine(Out());
    }

    public void ClickLoadSceneButton(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator Out()
    {
        loadingPanel.SetActive(true);
        loadingPanel.GetComponent<Image>().color = defaultColor;
        yield return new WaitForSeconds(1);
        loadingPanel.GetComponent<Image>().color = noneColor;
        loadingPanel.SetActive(false);
    }

    private IEnumerator LoadSceneAsync(string sceneToLoad)
    {
        StartCoroutine(Out());
        loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);

        while (!operation.isDone)
        {
            float progress = Mathf.Round(operation.progress * 100f);
            Debug.Log(progress);

            yield return null;
        }
    }
}