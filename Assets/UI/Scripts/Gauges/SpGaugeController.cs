using UnityEngine;

public class SpGaugeController : FillerController
{
    [SerializeField] private PlayerStatus player;

    private void Start()
    {
    }

    private void Update()
    {
        UpdateGauge(image, player.Sp, player.MaxSp);
    }
}
