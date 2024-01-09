using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFlash : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash Effect")]
    [SerializeField] private Material hitMaterial;
    private Material defaultMaterial;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        defaultMaterial = sr.material;


    }
   

    private IEnumerator FlashEffect()
    {
        sr.material = hitMaterial;
        yield return new WaitForSeconds(0.2f);
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
    private void CancelRedColorBlink()
    {
      CancelInvoke();
        sr.color = Color.white;

    }
    
}
