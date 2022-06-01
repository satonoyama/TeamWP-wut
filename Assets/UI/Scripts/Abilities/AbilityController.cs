using UnityEngine;
using UnityEngine.UI;

public class AbilityController : FillerController
{
    [SerializeField] private Image baseImage;
    [SerializeField] private Text text;

    [SerializeField] private float max;

    private void Start()
    {
        maxGauge = max;
        currentGauge = maxGauge;
    }

    private void Update()
    {
        if (isStop) { return; }

        text.text = ((int)currentGauge).ToString();

        UpdateGauge(image, currentGauge, maxGauge);
    }

    private void FixedUpdate()
    {
        if (isStop) { return; }

        currentGauge = maxGauge - Time.time;

        if (currentGauge < 1.0f)
        {
            text.gameObject.SetActive(false);
            baseImage.gameObject.SetActive(false);

            currentGauge = 0.0f;
            isStop = true;
            return;
        }
    }
}
