/// <summary>
/// RetroSuite-3D by oxysoft
/// https://github.com/oxysoft/RetroSuite3D
/// </summary>

using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Retro Pixel Max")]
public class RetroPixelMax : MonoBehaviour
{
    Material mMaterial;
    Shader shader;

    [Header("Colors")]
    public Color[] colors;

    Material Material
    {
        get
        {
            if (mMaterial == null)
            {
                shader = Shader.Find("Oxysoft/RetroPixelMax");
                mMaterial = new Material(shader) { hideFlags = HideFlags.DontSave };
            }

            return mMaterial;
        }
    }

    public void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (Material && colors.Length > 0)
        {
            Material.SetInt("_ColorCount", colors.Length);
            Material.SetColorArray("_Colors", colors);

            Graphics.Blit(src, dest, Material);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }

    void OnDisable()
    {
        if (mMaterial)
            DestroyImmediate(mMaterial);
    }
}
