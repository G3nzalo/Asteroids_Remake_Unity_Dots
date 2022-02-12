using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    #region Unity ref
    [Header("Sounds Tracks")]
    [Space(3.0f)]

    [Header("Sounds")]
    [SerializeField] private AudioSource track_source = null;
    [SerializeField] private AudioClip[] tracks = null;
    [Space(3.0f)]

    [Header("Source sfx")]
    [SerializeField] private AudioSource[] sfx_source = null;

    [Header("Sfx Shoot")]
    [SerializeField] private AudioClip[] shoot = null;
    [Space(3.0f)]
    [Header("Sfx Destroy")]
    [SerializeField] private AudioClip destroy_sfx = null;

    #endregion

    #region Unity calls
    private void Awake()
    {
        Initialize();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetTrack(0, 0.5f);
    }
    #endregion

    #region Singleton
    public void PlaySfxShoot(float _pan)
    {

        sfx_source[0].volume = Random.Range(0.2f, 0.3f);
        sfx_source[0].pitch = Random.Range(0.9f, 1.1f);
        sfx_source[0].panStereo = _pan;

        sfx_source[0].PlayOneShot(shoot[Random.Range(0, shoot.Length)]);

    }

    public void PlaySfxDestroy(float _pan)
    {
        if (!sfx_source[1].isPlaying)
        {
            sfx_source[1].volume = Random.Range(0.4f, 0.9f);
            sfx_source[1].pitch = Random.Range(0.9f, 1.1f);
            sfx_source[1].panStereo = _pan;
            sfx_source[1].PlayOneShot(destroy_sfx);
        }
    }

    public void SetTrack(int _index, float _volume)
    {
        track_source.clip = tracks[_index];
        track_source.volume = _volume;
        track_source.Play();
        track_source.loop = true;

    }

    private void Initialize()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion


}