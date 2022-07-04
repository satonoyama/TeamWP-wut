using UnityEngine;
using UnityEngine.UI;

// TODO : マジックナンバーの撲滅

public class LogoController : MonoBehaviour
{
    [SerializeField] private float maxPosY;
    Image image;
    RectTransform rect;
    private bool isMove = true;

    private void Start()
    {
        image = GetComponent<Image>();
        image.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (!isMove) { return; }

        {
            var alpha = 0.5f * Time.deltaTime;
            
            image.color += new Color(0.0f, 0.0f, 0.0f, alpha);
            if (image.color.a >= 1.0f)
            {
                image.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }

        rect.localPosition += new Vector3(0.0f, 0.5f, 0.0f);

        if(rect.localPosition.y >= maxPosY)
        {
            rect.localPosition = new Vector3(0.0f, maxPosY, 0.0f);
        }

        if (image.color.a >= 1.0f &&
            rect.localPosition.y >= maxPosY)
        {
            isMove = false;
        }
    }
}
