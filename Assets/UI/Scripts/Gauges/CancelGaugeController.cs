using UnityEngine;

public class CancelGaugeController : FillerController
{
    [SerializeField] private EnemyWeakPoint weakPoint;
    private float maxWeakPointHP = 0.0f;

    public void SetMaxWeakPointHP(float max) => maxWeakPointHP = max;

    private void Update()
    {
        if (!weakPoint.IsExecution) 
        {
            UpdateGauge(image, 0.0f, 0.0f);
            return;
        }

        UpdateGauge(image, weakPoint.Hp(), maxWeakPointHP);
    }
}
