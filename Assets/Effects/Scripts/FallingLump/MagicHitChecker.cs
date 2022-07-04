using UnityEngine;

public class MagicHitChecker : MonoBehaviour
{
    private FallingLump owner = null;

    public void SetOwner(FallingLump lump) => owner = lump;

    private void Update()
    {
        if(!owner.IsExecutable)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = owner.transform.position;
    }

    public void OnHitMagic(MagicalFX.MagicInfo info)
    {
        if (!info) { return; }

        owner.BreakFallingLump();
        Destroy(gameObject);
    }
}