using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackHoleHotKeyController : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;
    private Transform myEnemy;
    private BlackHoleSkillController blackHole;

    public void SetupHotKey(KeyCode _myNewhotKey, Transform _myEnemy, BlackHoleSkillController _myBlackHole )
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();

        myEnemy = _myEnemy;
        blackHole = _myBlackHole;

        myHotKey = _myNewhotKey;
        myText.text = _myNewhotKey.ToString();
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
           blackHole.AddEnemyToList(myEnemy);

            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }


}
