using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAttribute : MonoBehaviour
{
    public int spellValue;

    [ColorUsage(true, true)]
    public Color water, emissionWater, fire, emissionFire, darkVoid, emissionVoid;

    public AudioClip fireAudio, waterAudio, voidAudio;

    public bool isShooted;

    float lifetime;

    void Start()
    {
        if (spellValue == 1)
        {
            GetComponent<Renderer>().material.SetColor("Color_615E7439", water);
            GetComponent<Renderer>().material.SetColor("Color_8693C08C", emissionWater);
            GetComponent<AudioSource>().clip = waterAudio;
            GetComponent<AudioSource>().Play();
        }

        else if(spellValue == 2)
        {
            GetComponent<Renderer>().material.SetColor("Color_615E7439", fire);
            GetComponent<Renderer>().material.SetColor("Color_8693C08C", emissionFire);
            GetComponent<AudioSource>().clip = fireAudio;
            GetComponent<AudioSource>().Play();
        }

        else if (spellValue == 3)
        {
            GetComponent<Renderer>().material.SetColor("Color_615E7439", darkVoid);
            GetComponent<Renderer>().material.SetColor("Color_8693C08C", emissionVoid);
            GetComponent<AudioSource>().clip = voidAudio;
            GetComponent<AudioSource>().Play();
        }

        this.transform.GetChild(spellValue).gameObject.SetActive(true);
    }

    void Update()
    {
        if (isShooted)
        {
            lifetime += Time.deltaTime;

            if (lifetime > 3)
            {
                Destroy(this.gameObject);
            }
        }
        
    }
}
