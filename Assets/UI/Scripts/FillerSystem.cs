using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UI使うときは忘れずに！
using UnityEngine.UI;

public class FillerSystem : MonoBehaviour
{

    [SerializeField] Image image;

    void Start()
    {
        //ImageをGameObjectとして取得
        //image = GameObject.Find("Filler");
    }

    //()の中身は引数、他のところから数値を得て{}の中で使う
    public void GaugeDown(float current, float max)
    {
        //ImageというコンポーネントのfillAmountを取得して操作する
        //image.GetComponent<Image>().fillAmount = current / max;
        image.fillAmount = current / max;
    }
}