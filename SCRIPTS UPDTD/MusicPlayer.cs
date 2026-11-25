using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    [FormerlySerializedAs("song")]
    [SerializeField] private AudioClip normalSong;
    [SerializeField] private AudioClip proSong;
    [SerializeField] private AudioClip finaleSong;
    [SerializeField, Range(0f, 1f)] private float volume = 0.5f;

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        source.volume = volume;
        source.loop = true;

        GameManager.Instance.OnGameStarted += Play;
        GameManager.Instance.OnFinale += PlayFinale;
        GameManager.Instance.OnGameEnded += source.Stop;
    }

    private void Play()
    {
        source.clip = GameManager.Instance.ProMode ? proSong : normalSong;
        source.Play();
    }

    private void PlayFinale()
    {
        source.clip = finaleSong;
        source.Play();
    }
}
