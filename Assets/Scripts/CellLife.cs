using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellLife : IReseteable
{
    internal Rigidbody rg;
    internal Transform mtransform;
    internal Cell cell;
    public ADNstat LifeDependency;
    public ADNstat RegenerationDependency;
    public ADNstat ArmorDependency;

    public float MinLife = 50;
    public float Life;
    public float LifeReg;
    public float MaxLife;
    private float delta;
    private float deltaReg;

    public float DamageRatio = 1f;
    public float FoodRatio = 0.5f;
    public float RegRatio = 0.5f;
    public float RegRatioTime = 1f;

    public Food FoodPrefab;
    public Vector2 FoodSprayRange;
    public GameObject DamageFxInstance;

    public float Armor;

    public List<Resistence> resistences = new List<Resistence>();

    private void Awake()
    {
        Life = MaxLife = MinLife;
        cell = GetComponent<Cell>();
        rg = GetComponent<Rigidbody>();
        mtransform = transform;

        if (cell != null)
        {
            cell.Life = this;
            cell.onMutate += new CellEvent(RecalculateStats);
            cell.ThrowLifeEvent(Life);
        }
        
    }

    public override void ResetThis(Vector3 prandom)
    {
        base.ResetThis(prandom);
        Life = MaxLife = MinLife;
    }

    public void RecalculateStats(Cell cell, float value)
    {
        Life = MaxLife = MinLife + cell.RecalculateStats(LifeDependency);
        Armor = cell.RecalculateStats(ArmorDependency);
        LifeReg = cell.RecalculateStats(RegenerationDependency);

        cell.ThrowLifeEvent(Life);

        resistences = cell.ADNs.Where(adn => adn.ADN is CellResistence).
            Select(adn => new Resistence() {
                type = (adn.ADN as CellResistence).type,
                value = cell.RecalculateStats(adn.ADN.Stat)
            }).ToList();
    }

    private void FixedUpdate()
    {
        delta -= Time.deltaTime;
        deltaReg += Time.deltaTime;

        if (deltaReg > RegRatioTime)
        {
            Life = Mathf.Clamp((int)(Life + LifeReg * RegRatio), 0, MaxLife);
            deltaReg = 0;
        }

    }


    public void HandleDamage(float damage, DamageType type, Vector3 impactForce = new Vector3())
    {
        if (delta <= 0)
        {

            delta = DamageRatio;
            Resistence r = resistences.FirstOrDefault(rest => rest.type == type);
            float rValue = r?.value ?? 0;

            float finalDamage = damage * Mathf.Clamp(type.Multiplier, 0.1f, 99999) - (rValue + (cell?.CellSize() ?? 0 )/2f );

            Life = (int)(Life - finalDamage);
            
            rg.AddForce(impactForce);
            DamageFxInstance.SetActive(true);

            cell?.ThrowLifeEvent(Life);

            if (Life <= 0)
                Die();
        }
    }

    
    public void Die()
    {
        for (int i = 0; i < (MaxLife* FoodRatio); i++)
        {
            Instantiate(FoodPrefab, this.transform.position + new Vector3(
                Random.Range(-FoodSprayRange.x, FoodSprayRange.x),
                0,
                Random.Range(-FoodSprayRange.y, FoodSprayRange.y)
                ), Quaternion.identity).thrower = this.cell;
        }
        cell?.ThrowDieEvent();
        this.gameObject.SetActive(false);
    }


}


public class Resistence
{
    public DamageType type;
    public float value;
}