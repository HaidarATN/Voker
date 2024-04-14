using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttribute : MonoBehaviour
{
    [HideInInspector]
    public int healthPoints;

    public int maxHealthPoints;

    public int maxInvisEnergy = 30;

    public int invisDuration;

    public bool isInvis;

    [HideInInspector]
    public float invisEnergy;
    // Start is called before the first frame update
    void Start()
    {
        healthPoints = maxHealthPoints;
    }

    // Update is called once per frame
    void Update()
    {
        if(invisEnergy < maxInvisEnergy && !isInvis)
        {
            invisEnergy += Time.deltaTime;
        }
    }
}
