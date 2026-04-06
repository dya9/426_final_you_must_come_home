using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light lampLight;            
    public MeshRenderer lampRenderer; 
    public int materialIndex = 0;      

    [Header("Flicker Settings")]
    public float minIntensity = 1000.1f;
    public float maxIntensity = 2000.5f;
    public float flickerSpeed = 0.05f;

    private float targetIntensity;
    private Material lampMaterial;

    void Start()
    {
        if (lampRenderer != null)
            lampMaterial = lampRenderer.materials[materialIndex];
            
        targetIntensity = maxIntensity;
        InvokeRepeating(nameof(DoFlicker), 0, flickerSpeed);
    }

    void DoFlicker()
    {
        // Randomly decide if we stay bright or drop to min
        float newIntensity = Random.Range(0f, 1f) > 0.9f ? minIntensity : maxIntensity;
        lampLight.intensity = newIntensity;

        // Sync the visual bulb "glow" if a renderer is assigned
        if (lampMaterial != null)
        {
            if (newIntensity <= minIntensity)
                lampMaterial.DisableKeyword("_EMISSION");
            else
                lampMaterial.EnableKeyword("_EMISSION");
        }
    }
}
