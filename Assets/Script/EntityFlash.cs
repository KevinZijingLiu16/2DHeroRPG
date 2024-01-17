using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFlash : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash Effect")]
    [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMaterial;
    private Material defaultMaterial;


    [Header("Aliment color")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[]  igniteColor;
    [SerializeField] private Color[]  shockColor;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        defaultMaterial = sr.material;


    }
   

    private IEnumerator FlashEffect()
    {
        sr.material = hitMaterial;
        Color currentColor = sr.color;
        sr.color = Color.white;

        yield return new WaitForSeconds(0.2f);

        sr.color = currentColor;
        sr.material = defaultMaterial;
    }

    private void RedColorBlink()
    {
        if(sr.color != Color.white)
        {
            sr.color = Color.white;
        }
        else
        {
            sr.color = Color.red;
        }
    }
    private void CancelColorChange()
    {
      CancelInvoke();
        sr.color = Color.white;

    }

    public void IgniteFxFor(float _seconds)
    {
        InvokeRepeating("IgniteColorFX", 0, 0.3f);
        Invoke("CancelColorChange", _seconds);
    }
    private void IgniteColorFX()
    {
        if(sr.color != igniteColor[0])
        {
            sr.color = igniteColor[0];
        }
        else
        {
            sr.color = igniteColor[1];
        }
    }

    public void ChillFxFor(float _seconds)
    {
        InvokeRepeating("ChillColorFX", 0, 0.5f);
        Invoke("CancelColorChange", _seconds);
        
    }
    private void ChillColorFX()
    {
        if (sr.color != chillColor[0])
        {
            sr.color = chillColor[0];
        }
        else
        {
            sr.color = chillColor[1];
        }
    }
    public void ShockFxFor(float _seconds)
    {
        InvokeRepeating("ShockColorFX", 0, 0.1f);
        Invoke("CancelColorChange", _seconds);

        
    }



    private void ShockColorFX()
    {
        if(sr.color != shockColor[0])
        {
            sr.color = shockColor[0];
        }
        else
        {
            sr.color = shockColor[1];
        }
    }
    
    
}
