using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellShooter : MonoBehaviour
{
    Cell cell;
    public ADNstat DamageDependency;
    public ADNstat RateDependency;
    
    public float damage;
    public float attackRate = 10;

    public Attack AttackInstance;

    public string AttackAxis;
    private float delta;
    internal InputHandler inputHandler;

    private void Awake()
    {
        cell = GetComponent<Cell>();
        cell.onMutate += new CellEvent(RecalculateAttack);
    }

    public void RecalculateAttack(Cell cell, float value)
    {
        damage = cell.RecalculateStats(DamageDependency);
        attackRate = 10 - cell.RecalculateStats(RateDependency);
    }

    private void FixedUpdate()
    {
        if (damage > 0 && delta <= 0  && inputHandler.GetAxis(AttackAxis) >= 1)
        {
            delta = attackRate;
            Shoot();
            inputHandler.SetAxis(AttackAxis,0f);
        }

        delta -= Time.deltaTime;
    }

    public void Shoot()
    {
        AttackInstance.Set(this.transform.position, this.transform.rotation, cell, this.damage);

    }


}
