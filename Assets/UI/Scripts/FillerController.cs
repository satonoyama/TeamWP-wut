using UnityEngine;
using UnityEngine.UI;

public abstract class FillerController : MonoBehaviour
{
    [SerializeField] protected Image image;
    protected float maxGauge = 0.0f;
    protected float currentGauge = 0.0f;
    protected bool isStop = false;

    protected virtual void UpdateGauge(Image image, float current, float max)
    {
        image.fillAmount = current / max;
    }
}
