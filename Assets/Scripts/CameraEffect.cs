using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraEffect : MonoBehaviour
{
    public Animator PostProcessAnimator;
    public PlayerAttribute playerAtt;
    // Start is called before the first frame update
    void Start()
    {
        //PostProcessAnimator.Play("OpeningFade");
    }

    // Update is called once per frame
    void Update()
    {
        if(playerAtt.healthPoints <= (playerAtt.maxHealthPoints * 0.3))
        {
            PostProcessAnimator.SetBool("isDying", true);
        }

        else
        {
            PostProcessAnimator.SetBool("isDying", false);
        }
    }

}
