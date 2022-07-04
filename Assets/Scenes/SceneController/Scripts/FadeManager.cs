using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static bool isFadeInstance = false; // Canvas召喚フラグ

    [SerializeField] private float fadeSpeed = 0.2f;
    [SerializeField] private Image image;
    private float alpha = 0.0f;
    private bool isFadeIn = false;
    private bool isFadeOut = false;

    private void Start()
    {
        // 起動時
        if(!isFadeInstance)
        {
            DontDestroyOnLoad(this);
            isFadeInstance = true;
        }
        else
        {
            // 起動時以外は重複しないようにする
            Destroy(this);
        }
    }

    private void Update()
    {
        if(!isFadeIn && !isFadeOut) { return; }

        if(isFadeIn)
        {
            alpha -= Time.deltaTime / fadeSpeed;
            
            // 透明になったら、フェードイン終了
            if(alpha <= 0.0f)
            {
                isFadeIn = false;
                alpha = 0.0f;
            }
        }
        else if(isFadeOut)
        {
            alpha += Time.deltaTime / fadeSpeed;

            // 真っ黒になったら、フェードアウト終了
            if(alpha >= 1.0f)
            {
                isFadeOut = false;
                alpha = 1.0f;
            }
        }

        image.color = new Color(0.0f, 0.0f, 0.0f, alpha);
    }

    public void FadeIn()
    {
        isFadeIn = true;
        isFadeOut = false;
    }

    public void FadeOut()
    {
        isFadeIn = false;
        isFadeOut = true;
    }
}
