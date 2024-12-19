using System.Collections;
using UnityEngine;

/// <summary>
/// Cette classe définit le comportement d'une piste musicale dans le jeu.
/// #Élyzabelle Rollin
/// </summary>
public class PisteMusicale : MonoBehaviour
{
    [Header("Instance")]
    static PisteMusicale _instance; // Instance unique de la piste musicale
    public static PisteMusicale instance => _instance; // Getter pour l'instance de la piste musicale

    [Header("Type de piste musicale")]
    [SerializeField] TypePiste _type; // Type de la piste musicale
    public TypePiste type => _type; // Getter pour le type de piste musicale

    [Header("Composants")]
    AudioSource _source; // Référence à la source audio de la piste
    public AudioSource source => _source; // Getter pour la source audio de la piste


    [Header("Paramètres")]
    [SerializeField] bool _estActifParDefaut; // Détermine si la piste est activée par défaut
    [SerializeField] bool _estActif; // Indique si la piste est actuellement active
    public bool estActif
    {
        get => _estActif; // Getter pour l'état actif
        set
        {
            _estActif = value; // Modifie l'état actif
            AjusterVolume(); // Appele la méthode pour ajuster le volume en fonction de l'état actif
        }
    }

    // Initialisation de la piste musicale
    void Awake()
    {
        _source = GetComponent<AudioSource>(); // Récupère la source audio attachée à cet objet
        _estActif = _estActifParDefaut; // Initialise l'état actif en fonction de la valeur par défaut
        _source.loop = true; // Active la lecture en boucle de la source audio
        _source.playOnAwake = true; // Active la lecture automatique de la source audio lorsqu'elle est créée
    }

    // Méthode appelée après l'initialisation des autres objets
    void Start()
    {
        AjusterVolume(); // Appelle la méthode pour ajuster le volume en fonction de l'état actif
    }

    // Méthode pour ajuster le volume de la piste en fonction de son état actif
    public void AjusterVolume()
    {
        if (_estActif)
            _source.volume = GestAudio.instance.volumeMusicalRef; // Ajuste le volume au volume musical de référence si la piste est active
        else
            _source.volume = 0f; // Met le volume à zéro si la piste n'est pas active
    }
}