using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// �{�^���������ŘA�ˈ����ɂ���Interaction�N���X
/// </summary>
#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class BurstfireInteraction : IInputInteraction
{
#if UNITY_EDITOR
    static BurstfireInteraction()
    {
        Initialize();
    }
#endif

    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        InputSystem.RegisterInteraction<BurstfireInteraction>();
    }

    [Tooltip("�A�˂Ɣ��f����Ԋu")]
    public float duration = 0.2f;
    [Tooltip("�{�^�����������Ɣ��f���邵�����l")]
    public float pressPoint = 0.5f;


    public void Process(ref InputInteractionContext context)
    {

        //�A�˔��f�^�C�~���O�̏���
        if (context.timerHasExpired)
        {
            context.Canceled();
            if (context.ControlIsActuated(pressPoint))
            {
                context.Started();
                context.Performed();
                context.SetTimeout(duration);
            }
            return;
        }

        switch (context.phase)
        {
            case InputActionPhase.Waiting:
                //���͎�t��ԂɂȂ�����
                if (context.ControlIsActuated(pressPoint))
                {
                    //���s��Ԃɂ���
                    context.Started();
                    context.Performed();
                    //�A�˂̐U�镑��������^�C�~���O
                    context.SetTimeout(duration);
                }
                break;

            case InputActionPhase.Performed:

                if (!context.ControlIsActuated(pressPoint))
                {
                    //�{�^�������ꂽ��L�����Z���ɂ���B
                    context.Canceled();
                    return;
                }

                break;
        }
    }

    public void Reset()
    {

    }
}