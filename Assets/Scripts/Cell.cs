using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cell : IReseteable
{
    public static event CellEvent onCreate;
    public static event CellEvent onDestroy;

    public CellLife Life { get; internal set; }
    public CellControl Control { get; internal set; }

    public List<CellADNValue> ADNs;

    public List<SkinnedMeshRenderer> skinns;

    public event CellEvent onMutate;
    public event CellEvent onMutatePointsChange;
    public event CellEvent onEat;
    public event CellEvent onLifeChange;
    public event CellEvent onDie;

    public AnimationCurve MutationProgresion;
    public float TimesMutated = 0;
    public float TimeMutateProgress = 0.1f;
    public float NextMutatePoints = 10;
    public float mutatePoints = 100;
    public float mutationProgress = 50f;
    public float minMutationProgress = 10f;
    public float mutateBlockTime = 2f;

    public float CellSize() { return mtransform.localScale.x; }

    public bool mutating = false;
    private float mutatingDelta = 0f;

    public float MutatePoints
    {
        get => mutatePoints;
        set
        {
            mutatePoints = value;
            onMutatePointsChange?.Invoke(this, mutatePoints);
        }
    }

    internal Transform mtransform;


    private void OnEnable()
    {
        onCreate?.Invoke(this, 0);
    }

    private void Awake()
    {
        mtransform = transform;
        onEat += Cell_onEat;
        onDie += Cell_onDie;
        CalculateProgression();
    }

    public override void ResetThis(Vector3 random)
    {
        mtransform.localScale = Vector3.one;
        ResetMutations();
        MutatePoints = 0f;
        TimesMutated = 0f;
        CalculateProgression();
        Life.ResetThis(random);
    }

    public void CalculateProgression()
    {
        TimesMutated += TimeMutateProgress;
        float ev = MutationProgresion.Evaluate(TimesMutated);

        NextMutatePoints = (int)(ev * mutationProgress + minMutationProgress);
        onMutatePointsChange?.Invoke(this, mutatePoints);
    }

    private void Cell_onEat(Cell cell, float value)
    {
        MutatePoints += value;
    }
    private void Cell_onDie(Cell cell, float value)
    {
        onDestroy?.Invoke(this, value);
    }

    public void Mutate()
    {
        foreach (CellADNValue prop in ADNs)
        {
            prop.Mutate(Random.Range(0, 100), this);
        }
        ThrowMutateEvent();
    }

    internal void ResetMutations()
    {
        foreach (var cadn in ADNs)
            cadn.Mutate(0, this);

        ThrowMutateEvent();
    }

    public void MutateWastePoints()
    {
        if ((int)MutatePoints <= 0) return;

        if (mutatingDelta < mutateBlockTime) return;

        mutatingDelta = 0;
        for (int i = 0; i < NextMutatePoints; i++)
        {
            CellADNValue randomProp = ADNs[Random.Range(0, ADNs.Count)];
            randomProp.Mutate(randomProp.value + 1, this);
        }
        MutatePoints -= NextMutatePoints;
        TimesMutated += TimeMutateProgress;
        CalculateProgression();
        ThrowMutateEvent();
    }

    public void ThrowMutateEvent()
    {
        onMutate?.Invoke(this, 0);
    }
    public void ThrowMutatePointsEvent()
    {
        onMutatePointsChange?.Invoke(this, 0);
    }
    public void ThrowEatEvent(float n)
    {
        onEat?.Invoke(this, n);
    }
    public void ThrowLifeEvent(float l)
    {
        onLifeChange?.Invoke(this, l);
    }
    public void ThrowDieEvent()
    {
        onDie?.Invoke(this, 0);
    }

    public float RecalculateStats(ADNstat stat)
    {
        return ADNs.Where(a => a.ADN.Stat == stat).Select(a => a.CalculateStat()).Aggregate((a, b) => a + b);
    }

    bool firstRecalculation = false;

    private void Update()
    {
        mutatingDelta += Time.deltaTime;
        if (!firstRecalculation)
        {
            firstRecalculation = true;
            ThrowMutateEvent();
        }
        
    }

}

public delegate void CellEvent(Cell cell, float value);

[System.Serializable]
public class CellADNValue
{
    public CellProperty ADN;
    public float value;


    public void Mutate(float newValue, Cell cell, bool throwEvent = false)
    {
        value = newValue;
        if (ADN is CellADN)
        {
            var adn = (ADN as CellADN);
            cell.skinns.ForEach(sk => {
                if (sk.sharedMesh == adn._Mesh)
                {
                    var index = adn._Mesh.GetBlendShapeIndex(adn.Blendshape);
                    if (index >= 0)
                        sk.SetBlendShapeWeight(index, value);
                }
            });
        }
        else if (ADN is CellColor)
        {
            var color = (ADN as CellColor);
            var evaluate = value / 100f;
            var colorFood = cell.Control.inputHandler.isAI ? Random.Range(0f, 1f) : 0f;

            cell.skinns.ForEach(sk => {
                if (sk.sharedMaterial == color.material)
                {
                    var block = new MaterialPropertyBlock();
                    Color a = color.gradient.Evaluate(evaluate);
                    block.SetColor("_Color", Color.Lerp(a, color.Multiplier, colorFood));

                    sk.SetPropertyBlock(block);
                }
            });
        }

        if (throwEvent)
            cell.ThrowMutateEvent();
    }


    public float CalculateStat()
    {
        return (ADN.Stat?.Multiplier ?? 1) * value;
    }

}
