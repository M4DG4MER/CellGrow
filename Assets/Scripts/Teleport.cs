using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Teleport GoTo;


    private Transform mtr;
    public AudioSource source;
    public ParticleSystem fx;
    public GameObject NewAreaText;

    private void Awake()
    {
        mtr = transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        float x = (mtr.position - other.transform.position).x;
        fx.transform.position = other.transform.position;
        fx.gameObject.SetActive(true);
        fx.Play();

        GoTo.Receive(other.transform, x, other.transform.position.y);
        other.attachedRigidbody.velocity = Vector3.zero;
    }

    public void Receive(Transform tr, float x, float h)
    {
        Vector3 v = (mtr.position + mtr.right * x);
        v.y = h;
        fx.transform.position = tr.position = v;
        source.Play();
        fx.gameObject.SetActive(true);
        fx.Play();

        

        var inptu = tr.GetComponent<InputHandler>();
        if (inptu != null && !inptu.isAI)
            StartCoroutine(ShowTPText());
    }

    IEnumerator ShowTPText()
    {
        NewAreaText.SetActive(true);
        yield return new WaitForSeconds(5f);
        NewAreaText.SetActive(false);
    }





}
