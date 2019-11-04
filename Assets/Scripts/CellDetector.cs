using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellDetector : MonoBehaviour
{
    static float accumulatedFrameLapse = 0f;
    static float frameRateLapse = 0.05f;
    static float GetNextFrameDetectionValue()
    {
        return (accumulatedFrameLapse += frameRateLapse) % 1f;
    }

    internal Cell cell;
    Transform mtransform;
    Transform[] mchild;

    float delta;
    const float SearchRate = 1f;
    float deltaRandomOffset = 0f;

    public List<Detector> detectors;
    InputHandler iHandler;



    private void Awake()
    {
        cell = GetComponentInParent<Cell>();
        cell.onMutate += Cell_onMutate;
        mtransform = transform;
        mchild = GetComponentsInChildren<Transform>();
        deltaRandomOffset = GetNextFrameDetectionValue();
        iHandler = GetComponent<InputHandler>();
    }

    private void Cell_onMutate(Cell cell, float value)
    {
        detectors.ForEach(d => d.Range =  10 + cell.RecalculateStats(d.DetectionDependency) + cell.RecalculateStats(d.ViewDependency));
    }

    private void FixedUpdate()
    {
        delta += Time.deltaTime;
        float sx = cell.CellSize();

        if (delta > (SearchRate + deltaRandomOffset + sx))
        {
            StartCoroutine(CheckCoroutine(sx));
            delta = deltaRandomOffset;
        }
    }

    private IEnumerator CheckCoroutine(float sx)
    {
        float maxRange = detectors.Max(d => d.Range);
        var layers = detectors.Select(d => d.Definition.SearchLayer).Aggregate((a,b) => a | b);
        yield return null;
        var hits = Physics.SphereCastAll(mtransform.position, maxRange, Vector3.up, 0, layers, QueryTriggerInteraction.Collide);

        yield return null;
        var founded = hits.
            Where(h => { 
                if (h.transform == null || h.transform.gameObject == null) return false;
                if (h.transform.gameObject == cell.gameObject) return false;
                if (h.transform.In(mchild)) return false;
                var attack = h.transform.gameObject.GetComponent<Attack>();
                if (attack == null) return true;
                return attack.Shooter != cell;
            });

        yield return null;
        var p = cell.mtransform.position;

        var ieunumerators = detectors.Select(d => d.Check(founded, sx, p, iHandler.isAI));

        foreach (var ie in ieunumerators)
            while (ie.MoveNext())
                yield return ie.Current;

        yield return null;
    }

    private void OnDrawGizmos()
    {
        if (detectors != null && cell != null)
        {
            var p = cell.mtransform.position;
            detectors.ForEach(d => d.Draw(p));
        }
    }


}

[System.Serializable]
public class Detector
{
    public DetectorStats Definition = new DetectorStats();
    
    public ADNstat DetectionDependency;
    public ADNstat ViewDependency;
    public float Range = 100;

    public bool foundedOne = false;

    public IEnumerable<Vector3> nearests;

    public Vector3 Closest;

    public Transform Indicator;

    public bool ToDraw = false;
    private bool disabled = false;

    public IEnumerator Check(IEnumerable<RaycastHit> hits, float thisScale, Vector3 tpos, bool isIA)
    {
        var selection = hits.Where(h => {
            if (h.transform.gameObject.tag != Definition.SearchTag)
                return false;

            if (Definition.SizeUmbral)
                return Definition.BiggerThanUs ? h.transform.localScale.x > thisScale : h.transform.localScale.x <= thisScale;
            return true;
        });
        yield return null;

        nearests = selection.Select(h => h.transform.position).Where(v => (v  - tpos).magnitude<= Range);
        yield return null;

        if (nearests != null && nearests.Any())
        {
            foundedOne = true;
            Vector3 f = nearests.OrderBy(v => (v - tpos).magnitude).First();
            f.y = 0;

            yield return null;
            if (!isIA)
            {
                Indicator.gameObject.SetActive(true);
                Indicator.LookAt(Closest = f);
                disabled = false;
            }
            else if(!disabled)
            {
                disabled = true;
                Indicator.gameObject.SetActive(false);
            }
        } else
        {
            foundedOne = false;
            Closest = Vector3.zero;
            Indicator.gameObject.SetActive(false);
        }
    }

    public void Draw(Vector3 pos)
    {
        if (!ToDraw || nearests == null) return;

        Gizmos.color = Color.yellow;

        foreach (var n in nearests)
            Gizmos.DrawWireSphere(n, 4);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Closest, 4);
    }

    public override string ToString()
    {
        return Definition.ToString();
    }
}


[System.Serializable]
public class DetectorStats
{
    public string SearchTag;
    public LayerMask SearchLayer;
    public bool SizeUmbral = false;
    public bool BiggerThanUs = true;

    public static bool operator ==(DetectorStats a, DetectorStats b)
    {
        return a.SearchTag == b.SearchTag && a.SizeUmbral == b.SizeUmbral && a.BiggerThanUs == b.BiggerThanUs;
    }
    public static bool operator !=(DetectorStats a, DetectorStats b)
    {
        return a.SearchTag != b.SearchTag || a.SizeUmbral != b.SizeUmbral || a.BiggerThanUs != b.BiggerThanUs;
    }
    public override bool Equals(object obj)
    {
        if (!(obj is DetectorStats)) return false;
        DetectorStats b = (obj as DetectorStats);
        return this.SearchTag == b.SearchTag && this.SizeUmbral == b.SizeUmbral && this.BiggerThanUs == b.BiggerThanUs;
    }
}


