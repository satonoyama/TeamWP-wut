using UnityEngine;

public class CancelGaugeController : FillerController
{
    [SerializeField] private EnemyWeakPoint weakPoint;

    private void Update()
    {
        if (!weakPoint.IsExecution) { return; }

        UpdateGauge(image, weakPoint.Hp(), weakPoint.MaxHp());
    }
}
