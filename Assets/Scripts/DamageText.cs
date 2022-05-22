using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{

    public LeanTweenType easeType;
    public Text text;
    public int damage;
    void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        Constants.damageTextCount++;
        Destroy(gameObject,0.75f);
        LeanTween.move(gameObject,new Vector3(transform.position.x + Random.Range(-1f,1f),transform.position.y+1,transform.position.z  + Random.Range(-1f,1f)),0.75f).setEase(easeType);
    }

    void Update()
    {
        transform.LookAt(Camera.main.transform.position,Vector3.up);
    }

    void OnDestroy()
    {
        Constants.damageTextCount--;
    }
}
