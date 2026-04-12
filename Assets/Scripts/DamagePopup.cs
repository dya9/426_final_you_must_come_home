using UnityEngine;
using TMPro;
using System;

public class DamagePopup : MonoBehaviour
{
    [Header("Animation")]
    public float floatSpeed = 1.5f;
    public float lifetime = 1.2f;
    public float randomXSpread = 0.4f;

    private TextMeshPro tmp;
    private float timer;
    private Color startColor;
    private Vector3 moveDir;

    public void Setup(float damage)
    {
        tmp = GetComponentInChildren<TextMeshPro>();
        if (tmp == null)
        {
            Debug.LogError("[DamagePopup] No TextMeshPro found on prefab!");
            Destroy(gameObject);
            return;
        }

        tmp.text = $"-{Mathf.RoundToInt(damage)}";

        tmp.color = damage >= 50f ? Color.red
                  : damage >= 25f ? new Color(1f, 0.5f, 0f)
                  : Color.yellow;

        startColor = tmp.color;

        // FIX: explicitly use UnityEngine.Random to avoid System.Random ambiguity
        float randomX = UnityEngine.Random.Range(-randomXSpread, randomXSpread);
        moveDir = new Vector3(randomX, floatSpeed, 0f);

        if (Camera.main != null)
            transform.rotation = Camera.main.transform.rotation;

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += moveDir * Time.deltaTime;

        timer += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, timer / lifetime);

        if (tmp != null)
        {
            Color c = startColor;
            c.a = alpha;
            tmp.color = c;
        }

        if (Camera.main != null)
            transform.rotation = Camera.main.transform.rotation;
    }
}