using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
 
public class TunnelVision : MonoBehaviour
{
    [Header("References")]
    public HealthManager healthManager;
    public Volume volume;
 
    // ── LOW HEALTH — Tunnel Vision ────────────────────────────────────────────
 
    [Header("Low Health Trigger")]
    [Range(0f, 1f)] public float lowEffectStart = 0.5f;   // vignette starts at 50%
    [Range(0f, 1f)] public float lowEffectPeak  = 0.15f;  // full intensity at 15%
 
    [Header("Vignette Settings")]
    public float minVignette = 0.3f;
    public float maxVignette = 0.85f;
 
    [Header("Heartbeat Settings")]
    public float minHeartRate = 0.8f;
    public float maxHeartRate = 2.5f;
 
    // ── HEART ATTACK — Colour Effect ──────────────────────────────────────────
 
    [Header("Heart Attack Colour")]
    // Should match heartAttackThreshold / maxHealth in HealthManager (default 95/100 = 0.95)
    [Range(0f, 1f)] public float heartAttackPercent = 0.95f;
 
    [Header("Colour Adjustment Settings")]
    public float maxSaturation   = 80f;   // vivid neon colours
    public float maxHueShift     = 50f;   // world hue rotates
    public float maxPostExposure = 1.5f;  // screen blows out
 
    // ── Private ───────────────────────────────────────────────────────────────
 
    private Vignette         vignette;
    private ColorAdjustments colorAdjustments;
 
    private float pulseTimer = 0f;
    private float hueTimer   = 0f;
 
    void Start()
    {
        if (volume == null)
        {
            Debug.LogError("[TunnelVision] No Volume assigned!");
            return;
        }
 
        if (!volume.profile.TryGet(out vignette))
            Debug.LogError("[TunnelVision] Vignette not found — Add Override → Post-processing → Vignette.");
 
        if (!volume.profile.TryGet(out colorAdjustments))
            Debug.LogError("[TunnelVision] Color Adjustments not found — Add Override → Post-processing → Color Adjustments.");
 
        if (vignette != null)
        {
            vignette.intensity.overrideState = true;
            vignette.intensity.value = 0f;
        }
 
        if (colorAdjustments != null)
        {
            colorAdjustments.saturation.overrideState   = true;
            colorAdjustments.hueShift.overrideState     = true;
            colorAdjustments.postExposure.overrideState = true;
            colorAdjustments.saturation.value   = 0f;
            colorAdjustments.hueShift.value     = 0f;
            colorAdjustments.postExposure.value = 0f;
        }
    }
 
    void Update()
    {
        if (healthManager == null) return;
 
        float healthPercent = healthManager.CurrentHealth / healthManager.maxHealth;
 
        UpdateLowHealth(healthPercent);
        UpdateHeartAttackColour(healthPercent);
    }
 
    // ── LOW HEALTH: heartbeat vignette ────────────────────────────────────────
 
    void UpdateLowHealth(float healthPercent)
    {
        if (vignette == null) return;
 
        if (healthPercent >= lowEffectStart)
        {
            vignette.intensity.value = 0f;
            pulseTimer = 0f;
            return;
        }
 
        float danger    = Mathf.InverseLerp(lowEffectStart, lowEffectPeak, healthPercent);
        float heartRate = Mathf.Lerp(minHeartRate, maxHeartRate, danger);
        pulseTimer     += Time.deltaTime * heartRate;
 
        float beat           = GetHeartbeatValue(pulseTimer % 1f);
        float baseIntensity  = Mathf.Lerp(0f, minVignette, danger);
        float pulseIntensity = Mathf.Lerp(0f, maxVignette - minVignette, danger) * beat;
 
        vignette.intensity.value = Mathf.Clamp01(baseIntensity + pulseIntensity);
    }
 
    // ── HEART ATTACK: colour shift ────────────────────────────────────────────
 
    void UpdateHeartAttackColour(float healthPercent)
    {
        if (colorAdjustments == null) return;
 
        if (healthPercent < heartAttackPercent)
        {
            // Fade effects back out smoothly when health drops below threshold
            colorAdjustments.saturation.value   = Mathf.Lerp(colorAdjustments.saturation.value,   0f, Time.deltaTime * 3f);
            colorAdjustments.hueShift.value     = Mathf.Lerp(colorAdjustments.hueShift.value,     0f, Time.deltaTime * 3f);
            colorAdjustments.postExposure.value = Mathf.Lerp(colorAdjustments.postExposure.value, 0f, Time.deltaTime * 3f);
            hueTimer = 0f;
            return;
        }
 
        // How far past the threshold we are (0 = just hit it, 1 = at max health)
        float t = Mathf.InverseLerp(heartAttackPercent, 1f, healthPercent);
 
        // Colours become hyper-vivid and neon
        colorAdjustments.saturation.value = Mathf.Lerp(0f, maxSaturation, t);
 
        // Hue rotates faster as health climbs — world colours feel completely wrong
        hueTimer += Time.deltaTime * Mathf.Lerp(10f, 60f, t);
        colorAdjustments.hueShift.value = Mathf.Sin(hueTimer * Mathf.Deg2Rad) * maxHueShift * t;
 
        // Screen blows out — too bright, overwhelming
        colorAdjustments.postExposure.value = Mathf.Lerp(0f, maxPostExposure, t);
    }
 
    // ── Heartbeat curve ───────────────────────────────────────────────────────
 
    float GetHeartbeatValue(float t)
    {
        float beat1 = Mathf.Exp(-Mathf.Pow((t - 0.1f)  / 0.07f, 2f)) * 0.5f;
        float beat2 = Mathf.Exp(-Mathf.Pow((t - 0.25f) / 0.09f, 2f));
        return Mathf.Clamp01(beat1 + beat2);
    }
}