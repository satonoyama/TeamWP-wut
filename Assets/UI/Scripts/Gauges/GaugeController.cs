using UnityEngine;
using UnityEngine.UI;

public class GaugeController : FillerController
{
    [SerializeField] private MobStatus status;

    private void Start()
    {
        maxGauge = status.MaxHp;
        currentGauge = maxGauge;
    }

    private void Update()
    {
        // 発表用
        // TODO : 徐々に増減するように変更する
        UpdateGauge(image, status.Hp, status.MaxHp);
    }
}
