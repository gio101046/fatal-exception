/// <summary>
/// RetroSuite-3D by oxysoft
/// https://github.com/oxysoft/RetroSuite3D
/// </summary>

using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Posterize")]
public class Posterize : MonoBehaviour
{
    Material mMaterial;
    Shader shader;

    public int redComponent = 8;
    public int greenComponent = 8;
    public int blueComponent = 8;

    Material Material
    {
        get
        {
            if (mMaterial == null)
            {
                shader = Shader.Find("Oxysoft/Posterize");
                mMaterial = new Material(shader) { hideFlags = HideFlags.DontSave };
            }

            return mMaterial;
        }
    }

    public void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (Material)
        {
            Material.SetInt("_Red", redComponent);
            Material.SetInt("_Green", greenComponent);
            Material.SetInt("_Blue", blueComponent);

            Graphics.Blit(src, dest, Material);
        }
    }

    void OnDisable()
    {
        if (mMaterial)
            DestroyImmediate(mMaterial);
    }
}
