using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnabler : MonoBehaviour
{
    public AudioSource source;

    private void OnEnable()
    {
        source.Play();
    }
}
