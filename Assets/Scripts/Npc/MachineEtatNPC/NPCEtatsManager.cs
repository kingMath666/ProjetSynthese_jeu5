using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// Gestionnaire de l'etat du NPC
/// S'occupe des comportements du NPC
// #Élyzabelle Rollin
/// </summary>
public class NPCEtatsManager : MonoBehaviour
{
    [Header("Données du personnage")]
    public SOPerso _donneesPerso; // Données du joueur

    [Header("Sons")]
    public AudioClip _sonHeureux; // Son heureux

    [Header("États du NPC")]
    public NPCEtatsBase _etatActuel; // État actuel
    public NPCEtatPeur _peur = new NPCEtatPeur(); // État de peur
    public NPCEtatRepos _repos = new NPCEtatRepos(); // État de repos
    public NPCEtatHeureux _heureux = new NPCEtatHeureux(); // État heureux
    public NPCEtatSuivre _suivrePerso = new NPCEtatSuivre(); // État de suivre le joueur

    [Header("Informations du NPC")]
    public GameObject _ennemiActuel; // Ennemi actuel
    public bool _peutBouger { get; set; } // Permet de savoir si le NPC peut bouger
    public float _distanceEntrePerso = 3f; // Distance entre le joueur et le NPC
    public NavMeshAgent agent { get; set; } // Component NavMeshAgent
    public Animator _animator { get; set; } // Component Animator
    public Dictionary<string, dynamic> infos { get; set; } = new Dictionary<string, dynamic>(); // Informations du NPC

    void Start()
    {
        _peutBouger = false; // Bloquer le mouvement
        agent = GetComponent<NavMeshAgent>(); // Rechercher l'agent
        _animator = GetComponent<Animator>(); // Rechercher l'animator
        transform.Rotate(new Vector3(0, Random.Range(0, 360), 0)); // Rotation aléatoire
        infos["vision"] = 30f; // Zone de vision en unité Unity
        _etatActuel = _repos; // Etat initial
        _etatActuel.InitEtat(this); // Initialisation de l'etat
    }

    /// <summary>
    /// Sert à changer l'état du NPC
    /// </summary>
    /// <param name="etat"> Nouvel état du NPC </param>
    public void ChangerEtat(NPCEtatsBase etat)
    {
        _etatActuel.ExitEtat(this); //Appelle la fonction ExitEtat
        _etatActuel = etat; //Modifie l'etat actuel
        _etatActuel.InitEtat(this); //Appelle la fonction IniEtat
    }

    /// <summary>
    /// OnTriggerEnter
    /// </summary>
    /// <param name="other"> Collider de l'objet entrant en collision </param>
    void OnTriggerEnter(Collider other)
    {
        // Si l'ennemi entre en collision, que le NPC n'est pas dans l'etat de peur et peut bouger:
        if (other.CompareTag("ennemi") && _etatActuel != _peur && _peutBouger)
        {
            infos["ennemi"] = other.gameObject; // Enregistrer l'ennemi
            _ennemiActuel = other.gameObject; // Enregistrer l'ennemi
            ChangerEtat(_peur); // Changer à l'état de peur
        }
        if (other.CompareTag("lac")) ChangerEtat(_heureux);
    }

    /// <summary>
    /// OnTriggerExit
    /// </summary>
    /// <param name="other"> Collider de l'objet sortant de la collision </param>
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("lac")) _donneesPerso.RetirerPingouin();
    }
}
