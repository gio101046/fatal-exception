/// <summary>
/// RetroSuite-3D by oxysoft
/// https://github.com/oxysoft/RetroSuite3D
/// </summary>

using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Dither")]
public class Dither : MonoBehaviour
{
    Material mMaterial;
    Shader shader;

    public Texture2D pattern;
    [Range(0f, 1f)]
    public float threshold = .45f;
    [Range(0f, 1f)]
    public float strength = .45f;

    Material Material
    {
        get
        {
            if (mMaterial == null)
            {
                shader = Shader.Find("Oxysoft/Dither");
                mMaterial = new Material(shader) { hideFlags = HideFlags.DontSave };
            }

            return mMaterial;
        }
    }

    public void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (Material)
        {
            Material.SetTexture("_Dither", pattern);
            Material.SetInt("_Width", pattern.width);
            Material.SetInt("_Height", pattern.height);
            Material.SetFloat("_Threshold", threshold);
            Material.SetFloat("_Strength", strength);

            Graphics.Blit(src, dest, Material);
        }
    }

    void OnDisable()
    {
        if (mMaterial)
            DestroyImmediate(mMaterial);
    }
}
