using UnityEngine;

public class CancelGaugeController : FillerController
{
    [SerializeField] private EnemyWeakPoint weakPoint;

    private void Update()
    {
        UpdateGauge(image, weakPoint.Hp(), weakPoint.MaxHp());
    }
}
