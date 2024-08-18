using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Image Effects Ultra/Interference", order = 1)]
public class InterferenceEffect : BaseEffect
{
    [SerializeField]
    private Texture2D interferenceTex = null;

    [SerializeField]
    private float speed = 0.5f;

    [SerializeField]
    private float fuzzStrength = 4.0f;
    public static float time = 0f;

    // Find the Interference shader source.
    public override void OnCreate()
    {
        if (interferenceTex == null)
        {
            interferenceTex = Texture2D.whiteTexture;
        }

        baseMaterial.SetTexture("_InterferenceTex", interferenceTex);
        baseMaterial.SetFloat("_Speed", speed);
        baseMaterial.SetFloat("_FuzzStrength", fuzzStrength);
    }

    public override void Render(RenderTexture src, RenderTexture dst)
    {
        baseMaterial.SetFloat("_UnscaledTime", time);
        time += Time.unscaledDeltaTime;
        Graphics.Blit(src, dst, baseMaterial);
    }
}