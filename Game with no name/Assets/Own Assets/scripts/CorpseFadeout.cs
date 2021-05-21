using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CorpseFadeout : MonoBehaviour
{
    [SerializeField] static float FadeoutDelay = 3;
    [SerializeField] static float FadeoutDuration = 0.5f;

    public void Die()
    {
        StartCoroutine(FadeDelay());
    }

    IEnumerator FadeDelay()
    {
        yield return new WaitForSeconds(FadeoutDelay);
        FadeOutCorpsePart();
    }

    void FadeOutCorpsePart()
    {
        gameObject.GetComponent<SpriteRenderer>().DOFade(0, FadeoutDuration);
        StartCoroutine(DeleteDelay());
    }

    IEnumerator DeleteDelay()
    {
        yield return new WaitForSeconds(FadeoutDuration);
        Destroy(gameObject);
    }
}
