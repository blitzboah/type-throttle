using UnityEngine;

public class BotCarAudio : MonoBehaviour
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
        audioSourceReady.volume = 0.1f;
        audioSourceReady.pitch = 1f;
        audioSourceReady.loop = false;
        audioSourceReady.spatialBlend = 1f;
        audioSourceReady.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSourceReady.minDistance = 1f;
        audioSourceReady.maxDistance = 50f;


        audioSourceMoving.clip = movingSound;
        audioSourceMoving.volume = 0.05f;
        audioSourceMoving.pitch = 1f;
        audioSourceMoving.loop = true;
        audioSourceMoving.spatialBlend = 1f; // 3d sound
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
