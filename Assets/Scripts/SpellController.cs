using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    public int spellIndex;
    [Tooltip("Value 1 = Water, Value 2 = Fire")]
    public int[] spellValue = new int[2];
    public GameObject spellPrefab;
    public Camera cam;
    public Transform spellPosition;
    public float projectileSpeed = 20, invisibleEffectDuration;
    public ParticleSystem spellCastParticleLH, spellCastParticleRH, spellInvokeParticle;
    public GameObject[] leftHandSpellOrb, rightHandSpellOrb, playerBodyModel;
    public AudioClip invokeSound, shootSpellSound, castingSound, invisSound;
    public Animator playerAnimator;
    public PlayerAttribute playerAtt;

    int maxSpellIndex = 2;
    Vector3 rayDir;
    GameObject tempSpell;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            //Quas
            Spell(1);    
        }

        else if(Input.GetKeyDown(KeyCode.E))
        {
            //Exort
            Spell(2);
        }

        //else if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    //Invoke
        //    InvokeSpell();
        //}

        else if (Input.GetButtonDown("Fire1"))
        {
            if(tempSpell != null)
            {
                ShootSpell();
            }

            else
            {
                InvokeSpell();
            }
            
        }

        else if (Input.GetKeyDown(KeyCode.Space))
        {
            InvisiblePower();
        }
    }

    void Spell(int value)
    {
        spellValue[spellIndex] = value;
        if (spellIndex + 1 >= maxSpellIndex)
        {
            SpawnSpellCastingParticle(value);
            spellIndex = 0;
        }

        else
        {
            SpawnSpellCastingParticle(value);
            spellIndex++;
        }

        PlaySoundEffect(castingSound);
    }

    void InvokeSpell()
    {
        int currentSpellValue = spellValue[0] + spellValue[1];

        if(currentSpellValue == 2)
        {
            //Instantiate Water Element
            SpawnSpell(1);
        }

        else if(currentSpellValue == 4)
        {
            //Instantiate Fire Element
            SpawnSpell(2);
        }

        else if(currentSpellValue == 3)
        {
            //Instantiate Void Element
            SpawnSpell(3);
        }

        spellInvokeParticle.Play();
        PlaySoundEffect(invokeSound);
    }

    void SpawnSpell(int invokedSpellValue)
    {
        if(tempSpell == null)
        {
            tempSpell = Instantiate(spellPrefab, spellPosition);

            if (tempSpell.GetComponent<SpellAttribute>())
            {
                tempSpell.GetComponent<SpellAttribute>().spellValue = invokedSpellValue;
            }
        }

        else
        {
            if (tempSpell.GetComponent<SpellAttribute>())
            {
                tempSpell.GetComponent<SpellAttribute>().spellValue = invokedSpellValue;
            }
        }


        //tempSpell = null;
    }

    void ShootSpell()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.55f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            rayDir = hit.point;
        }

        else
        {
            rayDir = ray.GetPoint(1000);
        }

        tempSpell.transform.parent = null;

        tempSpell.GetComponent<Rigidbody>().isKinematic = false;

        tempSpell.GetComponent<Rigidbody>().velocity = (rayDir - spellPosition.position).normalized * projectileSpeed;

        tempSpell.GetComponent<SpellAttribute>().isShooted = true;

        tempSpell = null;

        PlaySoundEffect(shootSpellSound);
        //playerAnimator.SetLayerWeight(2, Input.GetAxis("Fire1"));
    }

    void SpawnSpellCastingParticle(int spellOrb)
    {
        if (spellIndex == 0)
        {
            spellCastParticleLH.Play();

            for(int i = 0; i < leftHandSpellOrb.Length; i++)
            {
                if(i == spellOrb - 1)
                {
                    leftHandSpellOrb[i].SetActive(true);
                }

                else
                {
                    leftHandSpellOrb[i].SetActive(false);
                }
            }
            
        }

        else
        {
            spellCastParticleRH.Play();

            for (int i = 0; i < leftHandSpellOrb.Length; i++)
            {
                if (i == spellOrb - 1)
                {
                    rightHandSpellOrb[i].SetActive(true);
                }

                else
                {
                    rightHandSpellOrb[i].SetActive(false);
                }
            }
        }
    }

    void InvisiblePower()
    {
        if(playerAtt.invisEnergy >= playerAtt.maxInvisEnergy)
        {
            playerAtt.invisEnergy -= playerAtt.maxInvisEnergy;
            playerAtt.isInvis = true;

            StartCoroutine(InvisibleDuration());
            StartCoroutine(InvisibleDissolvingEffect(0,1, playerAtt.isInvis));
        }
    }

    IEnumerator InvisibleDuration()
    {
        float duration = 0;

        while (true)
        {
            if (duration < playerAtt.invisDuration)
            {
                duration++;
                yield return new WaitForSeconds(1f);
            }

            else
            {
                break;
            }
        }

        playerAtt.isInvis = false;
        StartCoroutine(InvisibleDissolvingEffect(1, 0, playerAtt.isInvis));
        yield break;
        
    }

    IEnumerator InvisibleDissolvingEffect(float startAmount, float endAmount, bool hideBody)
    {
        float dur = 0, dissolveAmount = 0;
        PlaySoundEffect(invisSound);
        while (true)
        {
            //dissolveAmount += 0.01f;
            if (dur <= invisibleEffectDuration)
            {
                dissolveAmount = SuperLerp(startAmount, endAmount, 0, invisibleEffectDuration, dur);

                //dissolveAmount = Mathf.Lerp(0, 1, Mathf.InverseLerp(0, invisibleEffectDuration, 0.01f));
                //print(result);
                foreach(GameObject go in playerBodyModel)
                {
                    if (go.GetComponent<Renderer>().material.HasProperty("Vector1_E919662"))
                    {
                        go.GetComponent<Renderer>().material.SetFloat("Vector1_E919662", dissolveAmount);
                    }

                    else
                    {
                        go.SetActive(!hideBody);
                    }
                }
            }

            else
            {
                break;
            }

            dur += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    float SuperLerp(float from, float to, float from2, float to2, float value)
    {
        if (value <= from2)
            return from;
        else if (value >= to2)
            return to;
        return (to - from) * ((value - from2) / (to2 - from2)) + from;
    }


    void PlaySoundEffect(AudioClip clip)
    {
        //GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().PlayOneShot(clip);
    }
}
