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
        // ���\�p
        // TODO : ���X�ɑ�������悤�ɕύX����
        UpdateGauge(image, status.Hp, status.MaxHp);
    }
}
