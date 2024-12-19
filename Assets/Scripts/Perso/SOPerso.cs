using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(fileName = "Perso", menuName = "Perso")] //Permet de créer un ScriptableObject pour le perso

/// </summary>
/// Contient les données du personnage
/// #Élyzabelle Rollin
/// <summary>
public class SOPerso : ScriptableObject
{
    [Header("Navigation")]
    [SerializeField] SONavigation _navigation; //Informations de navigation du jeu 

    [Header("Voice Over")]
    [SerializeField] AudioClip[] _sonRemisePingouin; //Voice Over de capture de pingouin
    [SerializeField] AudioClip[] _sonPingouinVientIci; //Voice Over de quand le pingouin suit le joueur
    [SerializeField] AudioClip[] _sonPingouinAttend; //Voice Over d'attente de pingouin
    [SerializeField] AudioClip[] _sonManqueFlocons; //Voice Over de manque de flocons
    [SerializeField] AudioClip[] _sonManqueBalles; //Voice Over de manque de balles de neige

    [Header("Valeurs initiales")]
    [SerializeField, Range(0, 500)] int _nbFloconsIni = 0; //Nombre de flocons initial
    [SerializeField, Range(0, 999)] const int NB_MAX_FLOCONS = 999; //Nombre maximum de flocons que le joueur peut avoir
    [SerializeField, Range(0, 500)] int _nbBoulesDeNeigeIni = 0; //Nombre de boules de neige initial
    [SerializeField, Range(0, 10000)] int _nbPingouinsIni = 0; //Nombre de pingouins initial 
    [SerializeField, Range(0, 10000)] int _nbPingouinsCapturesIni = 0; //Nombre de pingouins initial 
    [SerializeField, Range(0, 10000)] int _prixBonhommeDeNeige = 50; //Prix du bonhomme de neige

    [Header("Valeurs actuelles")]
    [SerializeField, Range(0, 10000)] int _nbBoulesDeNeige = 0; //Nombre de boules de neige que le joueur possède
    [SerializeField, Range(0, 10000)] int _nbFlocons = 0; //Nombre de flocons que le joueur possède
    [SerializeField, Range(0, 10000)] int _nbPingouinsCaptures = 0; //Nombre de pingouins que le joueur a capturé
    [SerializeField] bool _bougerPerso = true; //Indique si le joueur peut bouger
    [SerializeField] bool _tutorielActif1 = false; //Indique si le tutoriel est actif
    [SerializeField] bool _tutorielActif2 = false; //Indique si le tutoriel est actif

    [Header("UnityEvents")]
    UnityEvent _modifierInfosDuJeu = new UnityEvent(); //Evenement qui modifie toutes les informations du jeu
    public UnityEvent modifierInfosDuJeu => _modifierInfosDuJeu; //Accesseur

    UnityEvent _modifierInfoFlocons = new UnityEvent(); //Evenement qui modifie le nombre de flocons
    public UnityEvent modifierInfoFlocons => _modifierInfoFlocons; //Accesseur

    UnityEvent _modifierInfoPingouins = new UnityEvent(); //Evenement qui modifie le nombre de pingouins
    public UnityEvent modifierInfoPingouins => _modifierInfoPingouins; //Accesseur

    UnityEvent _modifierInfoBoulesDeNeige = new UnityEvent(); //Evenement qui modifie le nombre de boules de neige
    public UnityEvent modifierInfoBoulesDeNeige => _modifierInfoBoulesDeNeige; //Accesseur


    //--------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------
    // Réinitialisation
    //--------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Fonction qui réinitialise 
    /// les valeurs du personnage 
    /// apres une partie
    /// </summary>
    public void Initialiser()
    {
        RestaurerPossession();
        modifierInfosDuJeu.Invoke(); // Mise à jour des infos après initialisation
    }


    /// <summary>
    /// Restore toutes les possessions
    /// du joueur à leurs valeurs initiales
    /// </summary>
    public void RestaurerPossession()
    {
        nbFlocons = _nbFloconsIni;
        nbBalleDeNeige = _nbBoulesDeNeigeIni;
        nbPingouinsCaptures = _nbPingouinsCapturesIni;
    }

    //--------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------
    // Possession
    //--------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Ajoute un flocon au joueur
    /// quand le joueur ramasse un flocon
    /// </summary>
    public void AjouterFlocon()
    {
        nbFlocons++;
    }

    /// <summary>
    /// Ajoute un pingouin au joueur
    /// quand le joueur capture un pingouin
    /// </summary>
    public void AjouterPingouin()
    {
        nbPingouinsCaptures++;
        if (nbPingouinsCaptures == _nbPingouinsIni && !_tutorielActif1 && !_tutorielActif2) _navigation.AllerSceneFin();
        else if (nbPingouinsCaptures == _nbPingouinsIni && _tutorielActif1)
        {
            _navigation.AllerSceneTutoriel2();
        }
        else if (nbPingouinsCaptures == _nbPingouinsIni && _tutorielActif2)
        {
            _navigation.Jouer();
        }
    }
    public void RetirerPingouin()
    {
        nbPingouinsCaptures--;
    }

    /// <summary>
    /// Ajoute une balle de neige au joueur
    /// quand le joueur ramasse une balle de neige
    /// #Patrick Watt-Charron
    /// </summary>
    public void AjouterBalleDeNeige()
    {
        nbBalleDeNeige++;
    }

    /// <summary>
    /// Retire une balle de neige au joueur
    /// quand le joueur utilise une balle de neige
    /// #Patrick Watt-Charron
    /// </summary>
    public void RetirerBalleDeNeige()
    {
        nbBalleDeNeige--;
    }

    /// <summary>
    /// Ajoute un bonhomme de neige au joueur
    /// quand le joueur achète un bonhomme de neige
    /// si le joueur a assez de flocons
    /// #Patrick Watt-Charron
    /// </summary>
    public void AcheterBonhomeDeNeige()
    {
        if (nbFlocons >= _prixBonhommeDeNeige) nbFlocons -= _prixBonhommeDeNeige;
    }

    //--------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------
    // Setters et getters
    //--------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------

    public int prixBonhommeDeNeige => _prixBonhommeDeNeige; //Accesseur du prix du bonhomme de neige
    public int FLOCONS_MAX => NB_MAX_FLOCONS; //Accesseur du nombre maximum de flocons
    //Accesseur de l'audio clip de quand le joueur capture un pingouin:
    public AudioClip[] sonRemisePingouin => _sonRemisePingouin;
    //Accesseur de l'audio clip de quand le pingouin suit le joueur:
    public AudioClip[] sonPingouinVientIci => _sonPingouinVientIci;
    //Accesseur de l'audio clip de quand le pingouin est en attente du joueur:
    public AudioClip[] sonPingouinAttend => _sonPingouinAttend;
    //Accesseur de l'audio clip de quand le joueur manque des flocons pour faire apparaitre le bonhomme de neige:
    public AudioClip[] sonManqueFlocons => _sonManqueFlocons;
    public AudioClip[] sonManqueBalles => _sonManqueBalles;

    /// <summary>
    /// Accesseur du nombre de flocons
    /// </summary>
    /// <value> Le nombre de flocons actuellement possèdé par le joueur </value>
    public int nbFlocons
    {
        get => _nbFlocons;
        set
        {
            _nbFlocons = Mathf.Clamp(value, 0, NB_MAX_FLOCONS);
            modifierInfoFlocons.Invoke();
        }
    }

    /// <summary>
    /// Accesseur du nombre de boules de neige
    /// </summary>
    /// <value> Le nombre de boules de neige actuellement possèdé par le joueur </value>
    public int nbBalleDeNeige
    {
        get => _nbBoulesDeNeige;
        set
        {
            _nbBoulesDeNeige = Mathf.Clamp(value, 0, int.MaxValue);
            modifierInfoBoulesDeNeige.Invoke();
        }
    }

    /// <summary>
    /// Accesseur du nombre de pingouins
    /// capturés par le joueur
    /// </summary>
    /// <value> Le nombre de pingouins actuellement capturé par le joueur </value>
    public int nbPingouinsCaptures
    {
        get => _nbPingouinsCaptures;
        set
        {
            _nbPingouinsCaptures = Mathf.Clamp(value, 0, nbPingouinsIni);
            modifierInfoPingouins.Invoke();
        }
    }

    /// <summary>
    /// Accesseur du nombre de pingouins
    /// dans le monde
    /// </summary>
    /// <value> Le nombre de pingouins actuellement capturé par le joueur </value>
    public int nbPingouinsIni
    {
        get => _nbPingouinsIni;
        set
        {
            _nbPingouinsIni = Mathf.Clamp(value, 0, int.MaxValue);
            modifierInfoPingouins.Invoke();
        }
    }

    /// <summary>
    /// Accesseur de la variable qui permet de savoir si le joueur peut bouger
    /// </summary>
    /// <value> true si le joueur peut bouger, false sinon </value>
    public bool bougerPerso
    {
        get => _bougerPerso;
        set => _bougerPerso = value;
    }

    /// <summary>
    /// Accesseur de la variable qui permet de savoir si le tutoriel est actif
    /// </summary>
    /// <value> true si le tutoriel est actif, false sinon </value>
    public bool tutorielActif1
    {
        get => _tutorielActif1;
        set => _tutorielActif1 = value;
    }

    /// <summary>
    /// Accesseur de la variable qui permet de savoir si le tutoriel est actif
    /// </summary>
    /// <value> true si le tutoriel est actif, false sinon </value>
    public bool tutorielActif2
    {
        get => _tutorielActif2;
        set => _tutorielActif2 = value;
    }
}