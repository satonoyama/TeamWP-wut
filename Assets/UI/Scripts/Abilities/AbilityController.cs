using UnityEngine;
using UnityEngine.UI;

public class AbilityController : FillerController
{
    [SerializeField] private Image baseImage;
    [SerializeField] private Text text;

    [SerializeField] private float maxGauge = 0.0f;
    private float currentGauge = 0.0f;

    // クールタイムが有効になっているかどうか( falseでスキル使用可能 )
    public bool IsCooltimeStop => isStop;

    private void Start()
    {
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
        }
    }

    public void OnActive()
    {
        text.gameObject.SetActive(true);
        baseImage.gameObject.SetActive(true);

        isStop = false;

        currentGauge = maxGauge;
        text.text = ((int)currentGauge).ToString();
    }
}
