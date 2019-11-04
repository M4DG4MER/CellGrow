using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleAttack : MonoBehaviour
{
    Cell  cell ;
    CellLife cellLife;
    public ADNstat DamageDependency;
    public ADNstat RangeDependency;

    public DamageType type;
    public float damage;
    public float minDamage;
    public float range = 3;
    public float minRange = 3;
    public float rangeMultiplier = .3f;

    private SphereCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        cellLife = GetComponentInParent<CellLife>();
        cell = GetComponentInParent<Cell>();
        cell.onMutate += new CellEvent(RecalculateAttack);
    }


    public void RecalculateAttack(Cell cell, float value)
    {
        damage = cell.RecalculateStats(DamageDependency);
        //range = minRange + cell.RecalculateStats(RangeDependency) * rangeMultiplier;
        //_collider.radius = range;
    }

    Dictionary<Collider, CellLife> OnToHit = new Dictionary<Collider, CellLife>();


    
    private void OnTriggerStay(Collider other)
    {
        CellLife otherCell;
        if (!OnToHit.ContainsKey(other))
        {
            otherCell = other.GetComponent<CellLife>();
            if (otherCell != null && otherCell != cellLife)
            {
                OnToHit.Add(other, otherCell);
                MakeDamage(otherCell);
            }
        }
        else
        {
            MakeDamage(OnToHit[other]);
        }

    }

    void MakeDamage(CellLife otherCell)
    {
        var heading = otherCell.mtransform.position - cellLife.mtransform.position;
        otherCell.HandleDamage(this.damage + minDamage + cell.CellSize()/2f, type, heading * 100);
    }
}
