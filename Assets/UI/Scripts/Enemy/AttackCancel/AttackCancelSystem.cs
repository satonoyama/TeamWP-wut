using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UI使うときは忘れずに！
using UnityEngine.UI;

public class AttackCancelSystem : MonoBehaviour
{

    //最大Gauge
    [SerializeField]
    int maxGauge = 100;
    //現在のGauge
    [SerializeField]
    float currentGauge;

    [SerializeField] GameObject fillerSystem;

    void Start()
    {
        //FillerSystemを取得する
        //fillerSystem = GameObject.Find("FillerSystem");
    }

    void Update()
    {
        //FillerSystemのスクリプトのGaugeDown関数に2つの数値を送る
        fillerSystem.GetComponent<FillerSystem>().GaugeDown(currentGauge, maxGauge);
    }

    //FixedUpdateは一定に呼ばれるので持続的に使う処理に良いらしい
    void FixedUpdate()
    {
        //currentGaugeが0以上ならTrue
        if (0 <= currentGauge)
        {
            //maxHPから秒数（×10）を引いた数がcurrentGauge
            currentGauge = maxGauge - Time.time * 10.0f;
        }
    }
}
