using UnityEngine;

public class TrailController : MonoBehaviour
{
    public enum TrailID
    {
        eLeftClaw,
        eRightClaw
    }

    [SerializeField] private TrailRenderer[] trails;

    private void Start()
    {
        for(int i = 0; i < trails.Length; i++)
        {
            trails[i].enabled = false;
        }
    }

    public void OnTrail(TrailID id)
    {
        if (!trails[(int)id]) { return; }

        trails[(int)id].enabled = true;
    }

    public void OnTrailFinished(TrailID id)
    {
        if (!trails[(int)id]) { return; }

        trails[(int)id].enabled = false;
    }
}
