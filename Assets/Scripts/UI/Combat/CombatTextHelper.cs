using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTextHelper : MonoBehaviour
{
    public enum TxtType
    {
        COMBAT_TEXT,
        ALERT_TEXT
    }

    [SerializeField] private TxtType txtType;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            OnAnimationEnd();
        }
    }

    public void OnAnimationEnd()
    {
        if(txtType == TxtType.COMBAT_TEXT)
            GetComponentInParent<MCombatAnimationHandler>().CombatTextAnimationEnded(gameObject);
        else if(txtType == TxtType.ALERT_TEXT)
            GetComponentInParent<MCombatAnimationHandler>().CombatTextAnimationEnded(gameObject);
    }
}
