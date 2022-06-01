using UnityEngine;

public class SpGaugeController : FillerController
{
    [SerializeField] private PlayerStatus player;

    private void Start()
    {
        maxGauge = player.MaxSp;
        currentGauge = maxGauge;
    }

    private void Update()
    {
        UpdateGauge(image, player.Sp, player.MaxSp);
    }
}
