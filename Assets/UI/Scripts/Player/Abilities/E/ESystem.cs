using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UI�g���Ƃ��͖Y�ꂸ�ɁI
using UnityEngine.UI;

public class ESystem : MonoBehaviour
{

    //�ő�Gauge
    [SerializeField]
    float maxGauge = 100.0f;
    //���݂�HP
    [SerializeField]
    float currentGauge;

    [SerializeField] GameObject textObj;
    [SerializeField] Text text;
    [SerializeField] GameObject fillerSystem;
    [SerializeField] GameObject fillerBaseSystem;

    void Start()
    {
        //CoolTime��GameObject�Ƃ��Ď擾����
        //textObj = GameObject.Find("CoolTime");
        //FillerSystem���擾����
        //fillerSystem = GameObject.Find("FillerSystem");
    }

    void Update()
    {
        //Text��Text�R���|�[�l���g�ɃA�N�Z�X
        //(int)��float�𐮐��ŕ\�����邽�߂̂���
        textObj.GetComponent<Text>().text = ((int)currentGauge).ToString();

        if (0 >= currentGauge)
        {
            textObj.GetComponent<Text>().gameObject.SetActive(false);
            fillerBaseSystem.GetComponent<Image>().gameObject.SetActive(false);
        }

        //HPSystem�̃X�N���v�g��GaugeDown�֐���2�̐��l�𑗂�
        fillerSystem.GetComponent<FillerSystem>().GaugeDown(currentGauge, maxGauge);
    }

    //FixedUpdate�͈��ɌĂ΂��̂Ŏ����I�Ɏg�������ɗǂ��炵��
    void FixedUpdate()
    {
        //currentGauge��0�ȏ�Ȃ�True
        if (0 <= currentGauge)
        {
            //maxGauge����b���i�~10�j������������currentGauge
            currentGauge = maxGauge - Time.time * 10.0f;
        }
    }
}
