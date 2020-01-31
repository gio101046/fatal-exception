/// <summary>
/// RetroSuite-3D by oxysoft
/// https://github.com/oxysoft/RetroSuite3D
/// </summary>

using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Retro Size")]
public class RetroSize : MonoBehaviour
{
    [Header("Resolution")]
    public int horizontalResolution = 160;
    public int verticalResolution = 144;

    public void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        horizontalResolution = Mathf.Clamp(horizontalResolution, 1, 2048);
        verticalResolution = Mathf.Clamp(verticalResolution, 1, 2048);

        var scaled = RenderTexture.GetTemporary(horizontalResolution, verticalResolution);
        scaled.filterMode = FilterMode.Point;

        Graphics.Blit(src, scaled);
        Graphics.Blit(scaled, dest);
        RenderTexture.ReleaseTemporary(scaled);
    }
}
