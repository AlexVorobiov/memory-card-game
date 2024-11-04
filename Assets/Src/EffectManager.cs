using CartoonFX;
using UnityEngine;
using Zenject;

public class EffectManager: IInitializable
{
    private ParticleSystem _ps1;
    private ParticleSystem _ps2;
    
    public EffectManager(ParticleSystem ps1, ParticleSystem ps2)
    {
        _ps1 = ps1;
        _ps2 = ps2;
    }
    
    public void Initialize()
    {
        
    }
    
    public void PlayEffect1(Vector3 position)
    {
        _ps1.transform.position = new Vector3(position.x, position.y, 30);
        _ps1.gameObject.SetActive(false);
        _ps1.gameObject.SetActive(true);
        _ps1.Play();
    }
    
    public void PlayEffect2(Vector3 position)
    {
        _ps2.transform.position = new Vector3(position.x, position.y, 30);
        _ps2.gameObject.SetActive(false);
        _ps2.gameObject.SetActive(true);
        _ps2.Play();
    }
}
