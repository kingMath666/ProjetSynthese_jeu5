using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Cette classe gère la gestion audio du jeu, y compris la lecture de musique et d'effets sonores.
/// #Élyzabelle Rollin
/// </summary>
public class GestAudio : MonoBehaviour
{
    [Header("Paramètres")]
    [SerializeField, Range(0f, 10f)] float fadeDuration = 2f; // Durée du fondu en secondes.
    [SerializeField] float _volumeMusicalRef = 1f; // Volume musical par défaut.
    [SerializeField] Vector2 _pitchMinMax = new(0.8f, 1.2f); // Interval de pitch pour les effets sonores.
    public float volumeMusicalRef => _volumeMusicalRef; // Getter pour le volume musical.
    PisteMusicale[] _tPistes;  // Tableau des pistes musicales.
    public PisteMusicale[] tPistes => _tPistes; // Getter pour le tableau des pistes musicales.
    int _volumeMin = 0; // Volume minimal.
    private IEnumerator fadeInCoroutine; // Coroutine pour le fade in d'un AudioSource.
    private IEnumerator fadeOutCoroutine; // Coroutine pour le fade out d'un AudioSource.

    [Header("Composants")]
    AudioSource _sourceEffetsSonores; // Source audio pour les effets sonores.
    AudioSource _sourceVoiceOver; // Source audio pour les effets sonores.

    [Header("Instances")]
    static GestAudio _instance; // Instance unique de GestAudio.
    public static GestAudio instance => _instance; // Getter pour l'instance de GestAudio.

    void Awake()
    {
        // Singleton : S'assure qu'il n'y a qu'une seule instance de GestAudio dans la scène:
        if (!DevenirInstanceSingleton()) return;
        DontDestroyOnLoad(gameObject); // Empêche la destruction de l'objet lors du changement de scène.
        _tPistes = GetComponentsInChildren<PisteMusicale>(); // Récupère les pistes musicales de tous les enfants de cet objet.
        _sourceEffetsSonores = gameObject.AddComponent<AudioSource>(); // Ajoute un AudioSource à cet objet pour les effets sonores.
        _sourceVoiceOver = gameObject.AddComponent<AudioSource>(); // Ajoute un AudioSource à cet objet pour le voice over.
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Changement du volume et du pitch
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Change le volume général de toutes les pistes musicales.
    /// </summary>
    /// <param name="volume">Le nouveau volume à appliquer.</param>
    public void ChangerVolumeGeneral(float volume)
    {
        _volumeMusicalRef = volume; // Change le volume par défaut.
        foreach (PisteMusicale piste in _tPistes) piste.AjusterVolume(); // Ajuste le volume de chaque piste.
    }

    /// <summary>
    /// Change le pitch de toutes les pistes musicales.
    /// </summary>
    /// <param name="pitch">Le nouveau pitch à appliquer.</param>
    public void ChangerPitchMusique(float pitch)
    {
        foreach (PisteMusicale piste in _tPistes) piste.source.pitch = pitch; // Change le pitch de chaque piste.
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Jouer un effet sonore
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Joue un effet sonore aléatoire dans la plage de pitch spécifiée.
    /// </summary>
    /// <param name="clip">Le clip audio à jouer.</param>
    public void JouerEffetSonoreAvecVariation(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("Le clip audio est manquant.");
            return;
        }
        else
        {
            float pitchAlea = Random.Range(_pitchMinMax.x, _pitchMinMax.y); // Génère un pitch aléatoire.
            _sourceEffetsSonores.pitch = pitchAlea; // Change le pitch de l'effet sonore.
            _sourceEffetsSonores.PlayOneShot(clip); // Joue l'effet sonore.
        }
    }

    /// <summary>
    /// Joue un effet sonore aléatoire dans la plage de pitch spécifiée et avec un volume spécifié.
    /// </summary>
    /// <param name="clip">Le clip audio à jouer.</param>
    /// <param name="volume">Le volume de l'effet sonore.</param>
    public void JouerEffetSonoreAvecVariation(AudioClip clip, float volume)
    {
        if (clip == null)
        {
            Debug.LogWarning("Le clip audio est manquant.");
            return;
        }
        else
        {
            float pitchAlea = Random.Range(_pitchMinMax.x, _pitchMinMax.y); // Génère un pitch aléatoire.
            _sourceEffetsSonores.pitch = pitchAlea; // Change le pitch de l'effet sonore.
            _sourceEffetsSonores.volume = volume; // Change le volume de l'effet sonore.
            _sourceEffetsSonores.PlayOneShot(clip); // Joue l'effet sonore.
        }
    }

    /// Joue un effet sonore aléatoire dans la plage de pitch spécifiée.
    /// </summary>
    /// <param name="clip">Le clip audio à jouer.</param>
    public void JouerEffetSonore(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("Le clip audio est manquant.");
            return;
        }
        else _sourceEffetsSonores.PlayOneShot(clip); // Joue l'effet sonore. 
    }

    /// <summary>
    /// Joue un effet sonore 
    /// </summary>
    /// <param name="clip">Le clip audio à jouer.</param>
    public void JouerEffetSonore(AudioClip clip, float volume)
    {
        if (clip == null)
        {
            Debug.LogWarning("Le clip audio est manquant.");
            return;
        }
        else
        {
            _sourceEffetsSonores.volume = volume; // Change le volume de l'effet sonore.
            _sourceEffetsSonores.PlayOneShot(clip); // Joue l'effet sonore.
        }
    }

    /// <summary>
    /// Permet de jouer la voix d'un personnage
    /// sans que les audios se superposent
    /// </summary>
    /// <param name="clip"> Le clip audio à jouer. </param>
    public void JouerVoiceOver(AudioClip clip)
    {
        // Si le clip audio est manquant ou si la piste est en 
        //cours de lecture, on ne joue pas l'effet sonore:
        if (clip == null || _sourceVoiceOver.isPlaying)
        {
            Debug.LogWarning("Le clip audio est manquant ou la piste est en cours de lecture.");
            return;
        }
        else _sourceVoiceOver.PlayOneShot(clip); // Joue l'effet sonore.
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // État de lecture des pistes
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Change l'état de lecture des pistes audio en fonction du type spécifié.
    /// </summary>
    /// <param name="typeRecherche">Le type de piste à jouer.</param>
    public void ChangerEtatLecturePiste(TypePiste typeRecherche, bool doitActiver)
    {
        foreach (PisteMusicale piste in _tPistes) // Boucle sur toutes les pistes.
        {
            // Si la piste voulu doit être jouée, on augmente son volume:
            if (doitActiver && piste.type == typeRecherche)
            {
                fadeInCoroutine = CoroutineFadeIn(piste.source);
                StartCoroutine(fadeInCoroutine);
                // StartCoroutine(CoroutineFadeIn(piste.source));
            }
            // Si la piste voulu ne doit pas être jouée, on diminue son volume:
            else if (!doitActiver && piste.type == typeRecherche)
            {
                fadeOutCoroutine = CoroutineFadeOut(piste.source);
                StartCoroutine(fadeOutCoroutine);
                // StartCoroutine(CoroutineFadeOut(piste.source));
            }
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Transition audio
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Eteint toutes les pistes musicales en les faisant progressivement s'estomper.
    /// </summary>
    public void EteindreLesPistes()
    {
        //Réduit le volume de toutes les pistes:
        foreach (PisteMusicale piste in _tPistes) StartCoroutine(CoroutineFadeOut(piste.source));
    }

    /// <summary>
    /// Augmente le volume graduellement.
    /// </summary>
    /// <param name="source">La source audio à laquelle appliquer le fondu enchaîné.</param>
    private IEnumerator CoroutineFadeIn(AudioSource source)
    {
        //Tant que le volume n'est pas réglé sur la valeur cible, on l'augmente progressivement:
        while (source.volume < volumeMusicalRef)
        {
            source.volume += volumeMusicalRef * Time.deltaTime / fadeDuration; //Augmente le volume progressivement
            yield return null;
        }
        // Assure que le volume est réglé sur la valeur cible à la fin:
        source.volume = volumeMusicalRef;
        StopCoroutine(fadeInCoroutine);
    }

    /// <summary>
    ///Diminue le volume graduellement.
    /// </summary>
    /// <param name="source">La source audio à laquelle appliquer le fondu enchaîné.</param>
    private IEnumerator CoroutineFadeOut(AudioSource source)
    {
        float startVolume = source.volume; //Recoit la valeur initiale du volume

        // Tant que le volume n'est pas réglé sur 0, on le baisse progressivement:
        while (source.volume > _volumeMin)
        {
            source.volume -= startVolume * Time.deltaTime / fadeDuration; //Diminue le volume progressivement
            yield return null;
        }
        // Assure que le volume est réglé sur 0 à la fin:
        source.volume = _volumeMin;
        StopCoroutine(fadeOutCoroutine);
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Singleton
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Rend l'instance actuelle de GestAudio un singleton en vérifiant s'il existe déjà une instance dans la scène.
    /// Si une instance existe, elle affiche un message de journalisation et détruit l'objet de jeu actuel.
    /// Si aucune instance n'existe, elle définit l'instance actuelle comme variable _instance.
    /// </summary>
    /// <returns>Retourne vrai si l'instance actuelle devient le singleton, faux sinon.</returns>
    bool DevenirInstanceSingleton()
    {
        // Singleton : S'assure qu'il n'y a qu'une seule instance de GestAudio dans la scénario.
        if (_instance != null)
        {
            Debug.LogWarning("Il y a déjà une instance de GestAudio dans la scène.");
            Destroy(gameObject); // Détruit l'objet de jeu actuel.
            return false;
        }
        else _instance = this;
        return true;
    }
}