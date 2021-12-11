using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Orbit : MonoBehaviour
{
    [SerializeField]
    public Transform focus = default;

    [SerializeField, Range(1f, 20f)]
    float distance = 10f;

    [SerializeField, Min(0f)]
    float focusRadius = 1f;

    [SerializeField, Range(0f, 1f)]
    float focusCenter = 0.5f;

    [SerializeField, Range(0f, 360f)]
    float rotationSpeed = 90f;

    [SerializeField]
    Vector3 orbitAngles = new Vector3(45f, 0f, 0f);

    Vector3 focusPoint;

    private void Update()
    {
        if (World.me.player == null)
            return;

        focusPoint = World.me.player.transform.position;

        UpdateFocusPoint();
        ManualRotation();

        Quaternion lookRotation = Quaternion.Euler(orbitAngles);
        Vector3 lookDiraction = lookRotation * Vector3.forward;
        Vector3 lookPosition = focusPoint - lookDiraction * distance;
        transform.SetPositionAndRotation(lookPosition, lookRotation);
    }

    private void UpdateFocusPoint()
    {
        if (focus == null)
            return;

        Vector3 targetPoint = focus.position;

        if (focusRadius > 0f)
        {
            float distance = Vector3.Distance(targetPoint, focusPoint);
            float t = 1f;

            if (distance > 0.01f && focusCenter > 0f)
            {
                t = Mathf.Pow(1f - focusCenter, Time.unscaledDeltaTime);
            }

            if (distance > focusRadius)
            {
                t = Mathf.Min(t, focusRadius / distance);
            }

            focusPoint = Vector3.Lerp(targetPoint, focusPoint, t);
        }
        else
        {
            focusPoint = targetPoint;
        }
    }

    void ManualRotation()
    {
        Vector3 input = new Vector3(
            Input.GetKey(KeyCode.W) ? 1f : Input.GetKey(KeyCode.S) ? -1f : 0f,
            Input.GetKey(KeyCode.A) ? 1f : Input.GetKey(KeyCode.D) ? -1f : 0f,
            0f
            );

        orbitAngles += rotationSpeed * Time.unscaledDeltaTime * input;
    }
}