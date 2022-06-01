using UnityEngine;
using UnityEngine.UI;

public abstract class FillerController : MonoBehaviour
{
    [SerializeField] protected Image image;
    protected bool isStop = false;

    protected virtual void UpdateGauge(Image image, float current, float max)
    {
        image.fillAmount = current / max;
    }
}
