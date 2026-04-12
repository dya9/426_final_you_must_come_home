using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArrowPointer : MonoBehaviour
{
    public static List<energyDrink> ActiveDrinks = new List<energyDrink>();

    [Header("Rotation")]
    public float rotationSpeed = 250f;

    [Header("A* Refresh")]
    public float pathRefreshInterval = 0.5f;

    [Header("Colors")]
    public Color hasTargetColor = Color.green;
    public Color noTargetColor = Color.white;

    private energyDrink lockedTarget;
    private float refreshTimer;
    private NavMeshPath navPath;

    private Renderer arrowRenderer;

    void Awake()
    {
        navPath = new NavMeshPath();
        arrowRenderer = GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        // ── COLOR CHANGE ─────────────────────────────
        if (arrowRenderer != null)
        {
            arrowRenderer.material.color = (lockedTarget != null)
                ? hasTargetColor
                : noTargetColor;
        }

        // ── refresh target ───────────────────────────
        refreshTimer -= Time.deltaTime;
        if (refreshTimer <= 0f)
        {
            refreshTimer = pathRefreshInterval;
            PickNearestDrink();
        }

        if (lockedTarget == null) return;

        // ── direction to target ──────────────────────
        Vector3 toTarget = lockedTarget.transform.position - transform.position;
        toTarget.y = 0f;

        if (toTarget.sqrMagnitude < 0.001f) return;

        // ── angle calculation ────────────────────────
        float angle = Mathf.Atan2(toTarget.x, toTarget.z) * Mathf.Rad2Deg;

        // clamp to your required range
        //angle = Mathf.Clamp(angle, 120f, 240f);

        // ── flat rotation (locked X = -90) ───────────
        Quaternion targetRot = Quaternion.Euler(-90f, angle, 0f);

        transform.localRotation = Quaternion.RotateTowards(
            transform.localRotation,
            targetRot,
            rotationSpeed * Time.deltaTime
        );
    }

  
    void PickNearestDrink()
    {
        ActiveDrinks.RemoveAll(d => d == null);

        if (ActiveDrinks.Count == 0)
        {
            lockedTarget = null;
            return;
        }

        energyDrink best = null;
        float bestCost = Mathf.Infinity;

        foreach (energyDrink drink in ActiveDrinks)
        {
            if (drink == null) continue;

            bool found = NavMesh.CalculatePath(
                transform.position,
                drink.transform.position,
                NavMesh.AllAreas,
                navPath
            );

            if (!found || navPath.status == NavMeshPathStatus.PathInvalid)
                continue;

            float cost = GetPathLength(navPath);

            if (cost < bestCost)
            {
                bestCost = cost;
                best = drink;
            }
        }

        lockedTarget = best;
    }
   


    static float GetPathLength(NavMeshPath path)
    {
        float length = 0f;
        for (int i = 1; i < path.corners.Length; i++)
            length += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        return length;
    }
}