using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UI�g���Ƃ��͖Y�ꂸ�ɁI
using UnityEngine.UI;

public class FillerSystem : MonoBehaviour
{

    [SerializeField] Image image;

    void Start()
    {
        //Image��GameObject�Ƃ��Ď擾
        //image = GameObject.Find("Filler");
    }

    //()�̒��g�͈����A���̂Ƃ��납�琔�l�𓾂�{}�̒��Ŏg��
    public void GaugeDown(float current, float max)
    {
        //Image�Ƃ����R���|�[�l���g��fillAmount���擾���đ��삷��
        //image.GetComponent<Image>().fillAmount = current / max;
        image.fillAmount = current / max;
    }
}