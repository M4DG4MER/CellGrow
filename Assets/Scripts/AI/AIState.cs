using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AIState : StateMachineBehaviour
{
    protected Cell cell;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (cell == null)
            cell = animator.GetComponent<Cell>();
    }
}


/*
 * comprobar celula viva
 * => HAY HAMBRE (comida < umbral)
     * => analizar entonrno
        * detectar amenazas (veneno, celulas más grandes, ...)
             *  => huir de amenazas 
             *      -> buscar posición alejada (OnEnter)
             *      -> ir a posicion (OnUpdate)
             *          -> apuntar
             *          -> avanzar
        * detectar alimento (meat, vegetales, ...)
             *  => ir en busca
             *      -> buscar posición cercana (OnEnter)
             *      -> ir a posicion  (OnUpdate)
             *          -> apuntar
             *          -> avanzar
        * detectar presas (celulas más pequeñas, ...)
             *  => ir en busca
             *      -> buscar posición cercana 
                    *   -> apuntar
                    *   -> atacar
        * Vagar (Wandering)
            *  => elegir punto aleatorio
                *  -> ir a punto aleatorio
 * => NO HAY HAMBRE (comida > umbral)
    * =>  MUTAR -> al azar
 * 
 * 
 * 
 */
