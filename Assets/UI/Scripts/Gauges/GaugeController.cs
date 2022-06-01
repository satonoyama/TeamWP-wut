using UnityEngine;
using UnityEngine.UI;

public class GaugeController : FillerController
{
    [SerializeField] private MobStatus status;
    [SerializeField] private bool isGradually = false;  // èôÅXÇ…ìÆÇ©Ç∑Ç©Ç«Ç§Ç©

    [SerializeField] private float graduallyMoveSpd = 1.0f;     // ìÆÇ©Ç∑ë¨ìx
    private float prevCurrent = 0.0f;

    private void Start()
    {
        prevCurrent = status.Hp;
    }

    private void Update()
    {
        UpdateGauge(image, status.Hp, status.MaxHp);
    }

    protected override void UpdateGauge(Image image, float current, float max)
    {
        var currentVal = current;

        if(isGradually)
        {
            if ((prevCurrent -= graduallyMoveSpd) <= current)
            {
                prevCurrent = current;
            }

            currentVal = prevCurrent;
        }

        base.UpdateGauge(image, currentVal, max);
    }
}
