using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusOverTime : MonoBehaviour
{
    public AnimationCurve AnimationSize;

    public float delta;
    public float TimeComplete;

    private SphereCollider sphere;

    private void Awake()
    {
        sphere = GetComponent<SphereCollider>();
    }

    private void FixedUpdate()
    {
        delta += Time.deltaTime;
        sphere.radius = AnimationSize.Evaluate(delta % TimeComplete);
    }

}
