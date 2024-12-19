using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.PlayerLoop;

/// <summary>
/// Script qui modifie le texte du jeu 
/// en fonction des infos du perso
/// et ajoute des animations de rétroaction
/// #Élyzabelle Rollin 
/// </summary>
public class RetroactionTexteJeu : MonoBehaviour
{
    [Header("Champs textes")]
    // Texte qui affiche le nombre de flocons que le joueur possède:
    [SerializeField] TextMeshProUGUI _txtFlocons;
    // Texte qui affiche le nombre de boules de neige que le joueur possède:
    [SerializeField] TextMeshProUGUI _txtBoulesDeNeige;
    // Texte qui affiche le nombre de pingouins que le joueur a rapatrié:
    [SerializeField] TextMeshProUGUI _txtPingouins;

    [Header("Animators")]
    [SerializeField] Animator _animPingouins; //Animator du champ texte des pingouins
    [SerializeField] Animator _animBDN; //Animator du champ texte des boules de neige
    [SerializeField] Animator _animFlocons; //Animator du champ texte des flocons
    [SerializeField] SOPerso _donneesPerso; //Données du perso
    [SerializeField] SONiveau _donneesNiveau;

    [Header("Instances")]
    static RetroactionTexteJeu _instance; // Instance de la classe
    public static RetroactionTexteJeu instance => _instance; // Accesseur

    [Header("Champs textes Minijeu")]
    [SerializeField] TextMeshProUGUI _nbEnnemi;
    [SerializeField] TextMeshProUGUI _nbBoulesDeNeige;

    public void Awake()
    {
        //Évite qu'il y ait plusieurs instances de la classe:
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        if (_nbEnnemi && _nbBoulesDeNeige)
        {
            ModifierInfosDuMiniJeu();
            return;
        }
        //Évite que le script soit activé si certaines références ne sont pas assignées:
        if (_donneesPerso == null || _txtFlocons == null || _txtBoulesDeNeige == null)
        {
            Debug.LogError("Certaines références ne sont pas assignées dans l'inspecteur !");
            enabled = false;
            return;
        }
        _donneesPerso.modifierInfosDuJeu.AddListener(ModifierInfosDuJeu); // Ajout du listener
        _donneesPerso.modifierInfoFlocons.AddListener(ModifierInfoFlocons); // Ajout du listener
        _donneesPerso.modifierInfoPingouins.AddListener(ModifierInfoPingouins); // Ajout du listener
        _donneesPerso.modifierInfoBoulesDeNeige.AddListener(ModifierInfoBoulesDeNeige); // Ajout du listener
        ModifierInfosDuJeu(); // Initialisation des infos au début du jeu
    }

    /// <summary>
    /// Fonction qui modifie le texte du jeu  
    /// en fonction des infos du perso lors 
    /// de l'initialisation du jeu
    /// </summary>
    void ModifierInfosDuJeu()
    {
        ModifierInfoFlocons();
        ModifierInfoPingouins();
        ModifierInfoBoulesDeNeige();
    }

    /// <summary>
    /// Fonction qui modifie le texte des flocons
    /// en fonction des infos du perso et
    /// ajoute une animation 
    /// </summary>
    void ModifierInfoFlocons()
    {
        _animFlocons.SetTrigger("ajout"); //Animation du champ texte

        // Modification du texte:
        // Doit toujours avoir 3 chiffres:
        if (_donneesPerso.nbFlocons <= 9) _txtFlocons.text = "00" + _donneesPerso.nbFlocons.ToString();
        else if (_donneesPerso.nbFlocons <= 99) _txtFlocons.text = "0" + _donneesPerso.nbFlocons.ToString();
        else _txtFlocons.text = _donneesPerso.nbFlocons.ToString();
    }

    /// <summary>
    /// Fonction qui modifie le texte des pingouins
    /// en fonction des infos du perso et
    /// ajoute une animation
    /// /// </summary>
    void ModifierInfoPingouins()
    {
        _animPingouins.SetTrigger("ajout"); //Animation du champ texte
        _txtPingouins.text = _donneesPerso.nbPingouinsCaptures.ToString() + "/" + _donneesPerso.nbPingouinsIni.ToString(); //Modification du texte
    }

    /// <summary>
    /// Fonction qui modifie le texte des boules de neige
    /// en fonction des infos du perso et
    /// ajoute une animation
    /// </summary>
    void ModifierInfoBoulesDeNeige()
    {
        _animBDN.SetTrigger("ajout"); //Animation du champ texte
        _txtBoulesDeNeige.text = _donneesPerso.nbBalleDeNeige.ToString(); //Modification du texte
    }

    void ModifierInfosDuMiniJeu()
    {
        _nbEnnemi.text = _donneesNiveau.nbTuer + "/" + _donneesNiveau.nbEnnemis;
        _nbBoulesDeNeige.text = _donneesPerso.nbBalleDeNeige.ToString();
    }

    void Update()
    {
        if (_nbEnnemi && _nbBoulesDeNeige)
        {
            ModifierInfosDuMiniJeu();
            return;
        }
    }
}
