using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UI使うときは忘れずに！
using UnityEngine.UI;

public class ESystem : MonoBehaviour
{

    //最大Gauge
    [SerializeField]
    float maxGauge = 100.0f;
    //現在のHP
    [SerializeField]
    float currentGauge;

    [SerializeField] GameObject textObj;
    [SerializeField] Text text;
    [SerializeField] GameObject fillerSystem;
    [SerializeField] GameObject fillerBaseSystem;

    void Start()
    {
        //CoolTimeをGameObjectとして取得する
        //textObj = GameObject.Find("CoolTime");
        //FillerSystemを取得する
        //fillerSystem = GameObject.Find("FillerSystem");
    }

    void Update()
    {
        //TextのTextコンポーネントにアクセス
        //(int)はfloatを整数で表示するためのもの
        textObj.GetComponent<Text>().text = ((int)currentGauge).ToString();

        if (0 >= currentGauge)
        {
            textObj.GetComponent<Text>().gameObject.SetActive(false);
            fillerBaseSystem.GetComponent<Image>().gameObject.SetActive(false);
        }

        //HPSystemのスクリプトのGaugeDown関数に2つの数値を送る
        fillerSystem.GetComponent<FillerSystem>().GaugeDown(currentGauge, maxGauge);
    }

    //FixedUpdateは一定に呼ばれるので持続的に使う処理に良いらしい
    void FixedUpdate()
    {
        //currentGaugeが0以上ならTrue
        if (0 <= currentGauge)
        {
            //maxGaugeから秒数（×10）を引いた数がcurrentGauge
            currentGauge = maxGauge - Time.time * 10.0f;
        }
    }
}
