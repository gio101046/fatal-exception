using UnityEngine;

public static class Easing
{
    public static float QuadIn(float t) => t * t;

    public static float QuadOut(float t) => t * (2f - t);

    public static float QuadInOut(float t)
    {
        return (t *= 2f) < 1f
            ? .5f * t * t
            : -.5f * ((t -= 1f) * (t - 2f) - 1f);
    }

    public static float CubeIn(float t) => t * t * t;

    public static float CubeOut(float t) => 1f + ((t -= 1f) * t * t);

    public static float CubeInOut(float t)
    {
        return (t *= 2f) < 1f
            ? .5f * t * t * t
            : .5f * ((t -= 2f) * t * t + 2f);
    }

    public static float QuartIn(float t) => t * t * t * t;

    public static float QuartOut(float t) => 1f - ((t -= 1f) * t * t * t);

    public static float QuartInOut(float t)
    {
        return (t *= 2f) < 1f
            ? .5f * t * t * t * t
            : -.5f * ((t -= 2f) * t * t * t - 2f);
    }

    public static float QuintIn(float t) => t * t * t * t * t;

    public static float QuintOut(float t) => 1f + ((t -= 1f) * t * t * t * t);

    public static float QuintInOut(float t)
    {
        return (t *= 2f) < 1f
            ? .5f * t * t * t * t * t
            : .5f * ((t -= 2f) * t * t * t * t + 2f);
    }

    public static float SineIn(float t) => 1f - Mathf.Cos(t * Mathf.PI / 2f);

    public static float SineOut(float t) => Mathf.Sin(t * Mathf.PI / 2f);

    public static float SineInOut(float t) => .5f * (1f - Mathf.Cos(Mathf.PI * t));

    public static float ExpoIn(float t) => t == 0f ? 0f : Mathf.Pow(1024f, t - 1f);

    public static float ExpoOut(float t) => t == 1f ? 1f : 1f - Mathf.Pow(2f, -10f * t);

    public static float ExpoInOut(float t)
    {
        if (t == 0f || t == 1f) return t;

        return (t *= 2f) < 1f
            ? .5f * Mathf.Pow(1024f, t - 1f)
            : .5f * (-Mathf.Pow(2f, -10f * (t - 1f)) + 2f);
    }

    public static float CircIn(float t) => 1f - Mathf.Sqrt(1f - t * t);

    public static float CircOut(float t) => Mathf.Sqrt(1f - ((t -= 1f) * t));

    public static float CircInOut(float t)
    {
        return (t *= 2f) < 1f
            ? -.5f * (Mathf.Sqrt(1f - t * t) - 1)
            : .5f * (Mathf.Sqrt(1f - (t -= 2f) * t) + 1f);
    }

    public static float ElastIn(float t)
    {
        if (t == 0f || t == 1f) return t;

        return -Mathf.Pow(2f, 10f * (t -= 1f))
            * Mathf.Sin((t - 0.1f) * (2f * Mathf.PI) / .4f);
    }

    public static float ElastOut(float t)
    {
        if (t == 0f || t == 1f) return t;

        return Mathf.Pow(2f, -10f * t)
            * Mathf.Sin((t - .1f) * (2f * Mathf.PI) / .4f) + 1f;
    }

    public static float ElastInOut(float t)
    {
        return (t *= 2f) < 1f
            ? -.5f * Mathf.Pow(2f, 10f * (t -= 1f))
                * Mathf.Sin((t - .1f) * (2f * Mathf.PI) / .4f)
            : Mathf.Pow(2f, -10f * (t -= 1f))
                * Mathf.Sin((t - .1f) * (2f * Mathf.PI) / .4f) * .5f + 1f;
    }

    public static float BackIn(float t) => t * t * (2.70158f * t - 1.70158f);

    public static float BackOut(float t) => (t -= 1f) * t * (2.70158f * t + 1.70158f) + 1f;

    public static float BackInOut(float t)
    {
        return (t *= 2f) < 1f
            ? .5f * (t * t * ((2.5949095f + 1f) * t - 2.5949095f))
            : .5f * ((t -= 2f) * t * ((2.5949095f + 1f) * t + 2.5949095f) + 2f);
    }

    public static float BounceIn(float t) => 1f - BounceOut(1f - t);

    public static float BounceOut(float t)
    {
        if (t < (1f / 2.75f)) return 7.5625f * t * t;
        else if (t < (2f / 2.75f)) return 7.5625f * (t -= 1.5f / 2.75f) * t + .75f;
        else if (t < (2.5f / 2.75f)) return 7.5625f * (t -= 2.25f / 2.75f) * t + .9375f;
        else return 7.5625f * (t -= 2.625f / 2.75f) * t + .984375f;
    }

    public static float BounceInOut(float t)
    {
        return t < 0.5f
            ? BounceIn(t * 2f) * .5f
            : BounceOut(t * 2f - 1f) * .5f + .5f;
    }
}
