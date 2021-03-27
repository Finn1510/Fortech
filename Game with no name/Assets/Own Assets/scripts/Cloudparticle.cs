using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cloudparticle : MonoBehaviour
{
    [SerializeField] float Windspeed = 1;
    
    ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        ps.Simulate(5000);
        ps.Play();
    }

    public void ChangeWindSpeed(float newWindspeed, float ChangeDuration)
    {
        ps.playbackSpeed = newWindspeed;
        DOTween.To(() => ps.playbackSpeed, x => ps.playbackSpeed = x, newWindspeed, ChangeDuration);
    }
}
