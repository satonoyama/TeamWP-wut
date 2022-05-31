using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UI�g���Ƃ��͖Y�ꂸ�ɁI
using UnityEngine.UI;

public class AttackCancelSystem : MonoBehaviour
{

    //�ő�Gauge
    [SerializeField]
    int maxGauge = 100;
    //���݂�Gauge
    [SerializeField]
    float currentGauge;

    [SerializeField] GameObject fillerSystem;

    void Start()
    {
        //FillerSystem���擾����
        //fillerSystem = GameObject.Find("FillerSystem");
    }

    void Update()
    {
        //FillerSystem�̃X�N���v�g��GaugeDown�֐���2�̐��l�𑗂�
        fillerSystem.GetComponent<FillerSystem>().GaugeDown(currentGauge, maxGauge);
    }

    //FixedUpdate�͈��ɌĂ΂��̂Ŏ����I�Ɏg�������ɗǂ��炵��
    void FixedUpdate()
    {
        //currentGauge��0�ȏ�Ȃ�True
        if (0 <= currentGauge)
        {
            //maxHP����b���i�~10�j������������currentGauge
            currentGauge = maxGauge - Time.time * 10.0f;
        }
    }
}
