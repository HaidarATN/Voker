using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvent : MonoBehaviour
{
    public EnemyController controller;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AttackEvent()
    {
        controller.DealDamage();
    }

    void StartChaseEvent()
    {
        controller.StartChasing();
    }

    void StartWandering()
    {
        controller.Wandering();
    }

    void OnDead()
    {
        controller.DestroyOnDead();
    }


    void PlaySoundFX(AudioClip clip)
    {
        controller.PlaySound(clip);
    }
}
