using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    public bool isTriggered = false;
    public GameObject particleEffect, parentObj;
    //private List<GameObject> particleEffectList = new List<GameObject>();
  
    //private void Awake()
    //{
    //    for (int i = 0; i < 4; i++)
    //    {
    //        GameObject obj = Instantiate(particleEffect,parentObj.transform);
    //        obj.SetActive(false);
    //        particleEffectList.Add(obj);
    //    }
    //}
    #region Particle Effect Pooling

   
    #endregion
  
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.isTriggered = true;
            //GetPooledParticleEffect();
            GameManager.instance.audioManager.PlaySuccess();
            particleEffect.transform.localPosition = other.transform.localPosition;
            particleEffect.SetActive(true);
            particleEffect.GetComponent<ParticleSystem>().Play();
            StartCoroutine(IDisableparticleEffect());
            //particle.SetActive(true);

        }
    }

    IEnumerator IDisableparticleEffect()
    {

        yield return new WaitForSeconds(2f);
        particleEffect.SetActive(false);
    }

}
