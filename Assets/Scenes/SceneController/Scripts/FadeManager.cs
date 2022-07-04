using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static bool isFadeInstance = false; // Canvas�����t���O

    [SerializeField] private float fadeSpeed = 0.2f;
    [SerializeField] private Image image;
    private float alpha = 0.0f;
    private bool isFadeIn = false;
    private bool isFadeOut = false;

    private void Start()
    {
        // �N����
        if(!isFadeInstance)
        {
            DontDestroyOnLoad(this);
            isFadeInstance = true;
        }
        else
        {
            // �N�����ȊO�͏d�����Ȃ��悤�ɂ���
            Destroy(this);
        }
    }

    private void Update()
    {
        if(!isFadeIn && !isFadeOut) { return; }

        if(isFadeIn)
        {
            alpha -= Time.deltaTime / fadeSpeed;
            
            // �����ɂȂ�����A�t�F�[�h�C���I��
            if(alpha <= 0.0f)
            {
                isFadeIn = false;
                alpha = 0.0f;
            }
        }
        else if(isFadeOut)
        {
            alpha += Time.deltaTime / fadeSpeed;

            // �^�����ɂȂ�����A�t�F�[�h�A�E�g�I��
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
