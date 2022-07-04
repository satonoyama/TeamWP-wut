using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance
    {
        get { return instance; }
    }

    private static SceneController instance;

    [SerializeField] private GameObject fadeCanvas;  // ëÄçÏÇ∑ÇÈCanvas

    private void Awake()
    {
        instance = this;

        if (!FadeManager.isFadeInstance)
        {
            Instantiate(fadeCanvas);
        }

        Invoke("FindFadeObject", 0.02f);
    }

    private void FindFadeObject()
    {
        fadeCanvas = GameObject.FindGameObjectWithTag("Fade");
    }

    public async void SceneChange(string sceneName)
    {
        fadeCanvas.GetComponent<FadeManager>().FadeOut();

        // à√ì]Ç∑ÇÈÇ‹Ç≈ë“Ç¬
        await Task.Delay(200);

        SceneManager.LoadScene(sceneName);

        // ìßñæÇ…Ç»ÇÈÇ‹Ç≈ë“Ç¬
        await Task.Delay(200);

        fadeCanvas.GetComponent<FadeManager>().FadeIn();
    }
}
