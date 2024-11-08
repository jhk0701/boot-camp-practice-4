using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DamageIndicator : MonoBehaviour
{
    [SerializeField] float blinkSpeed = 0.5f;
    Image image;
    Color startColor;
    [SerializeField] AnimationCurve curve;

    void Start()
    {
        CharacterManager.Instance.Player.condition.OnPlayerDamaged += Blink;

        image = GetComponent<Image>();

        startColor = image.color;
        
        image.enabled = false;
    }

    void Blink()
    {
        if(BlickHandler != null)
            StopCoroutine(BlickHandler);

        BlickHandler = StartCoroutine(BlickProcess());
    }

    Coroutine BlickHandler;
    IEnumerator BlickProcess()
    {
        float progress = 0f;
        image.enabled = true;
        Color col = image.color;

        while(progress <= 1f)
        {
            col.a = curve.Evaluate(progress);
            image.color = col;
            
            yield return null;

            progress += blinkSpeed * Time.deltaTime;
        }

        image.enabled = false;
        image.color = startColor;
    }

}
