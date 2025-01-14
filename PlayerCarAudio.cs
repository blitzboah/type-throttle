using UnityEngine;

//its just a copy of bot audio lmao
public class PlayerCarAudio : MonoBehaviour
{
    private AudioSource audioSourceReady;
    private AudioSource audioSourceMoving;

    [SerializeField] private AudioClip readySound;
    [SerializeField] private AudioClip movingSound;

    private void Awake()
    {
        audioSourceReady = gameObject.AddComponent<AudioSource>();
        audioSourceMoving = gameObject.AddComponent<AudioSource>();
    }
    private void Start()
    {
        audioSourceReady.clip = readySound;
        audioSourceReady.volume = 0.15f;
        audioSourceReady.pitch = 1f;
        audioSourceReady.loop = false;
        audioSourceReady.spatialBlend = 0f;
        audioSourceReady.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSourceReady.minDistance = 1f;
        audioSourceReady.maxDistance = 50f;


        audioSourceMoving.clip = movingSound;
        audioSourceMoving.volume = 0.08f;
        audioSourceMoving.pitch = 1f;
        audioSourceMoving.loop = true;
        audioSourceMoving.spatialBlend = 0f;
        audioSourceMoving.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSourceMoving.minDistance = 1f;
        audioSourceMoving.maxDistance = 50f;
    }

    public void PlayReadySound()
    {
        audioSourceReady.Play();
    }

    public void PlayEngineSound()
    {
        audioSourceMoving.Play();
    }

    public void StopEngineSound()
    {
        audioSourceMoving.Stop();
    }
}
