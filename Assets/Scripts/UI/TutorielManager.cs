using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe qui gère la première partie 
/// du tutoriel du jeu
/// #Élyzabelle Rollin
/// </summary>
public class TutorielManager : MonoBehaviour
{
    // Le panneau qui contient les instructions du tutoriel:
    [SerializeField] private GameObject _panneau;
    [SerializeField] SOPerso _donneesPerso; // Données du joueur
    bool _panneauActive = false; // Indique si le panneau est actif
    [SerializeField] TextMeshProUGUI _titre; // Titre
    [SerializeField] TextMeshProUGUI _texte; // Texte
    int _etape = 1; // Étape du tutoriel

    void Start()
    {
        _panneau.SetActive(_panneauActive); // Désactive le panneau
        // Vérifier si le joueur est dans un tutoriel2:
        // #Patrick Watt-Charron
        if (_donneesPerso.tutorielActif2)
        {
            _etape = 4;
            Invoke("Etape4", 2f);// Affiche la quatrième étape du tutoriel
        }
        if (_etape == 1) Invoke("Etape1", 2f); //Affiche la première étape du tutoriel
    }

    void Update()
    {
        //Vérifier si le joueur appuie sur A,W,S,D et qu'il est à la deuxième étape du tutoriel:
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) && _etape == 2)
        {
            _etape = 3; // Passer à la troisième étape
            Invoke("Etape2", 10f); // Afficher la deuxieme étape du tutoriel
        }
    }

    /// <summary>
    /// Affiche la première étape du tutoriel
    /// Explique comment se déplacer
    /// </summary>
    void Etape1()
    {
        AfficherPanneau(); // Afficher le panneau
        // Mettre à jour les textes:
        _titre.text = "Welcome to the tutorial !";
        _texte.text = "To move, use the WASD keys !";
    }

    /// <summary>
    /// Affiche la deuxieme étape du tutoriel
    /// Explique comment interagir avec les objets
    /// </summary>
    void Etape2()
    {
        AfficherPanneau(); // Afficher le panneau
        // Mettre à jour les textes:
        _titre.text = "You'll see some objects in the game !";
        _texte.text = "To interact with objects, press the E key !";
        _etape = 3; // Passer à la troisième étape
    }

    /// <summary>
    /// Affiche la troisième étape du tutoriel
    /// Explique que le joueur doit ramener les pingouins
    /// </summary>
    void Etape3()
    {
        AfficherPanneau(); // Afficher le panneau
        // Mettre à jour les textes:
        _titre.text = "Pingouins are your friends !";
        _texte.text = "You must bring them back to the pond ! They only walk on snow !";
    }


    /// <summary>
    /// Affiche la quatrième étape du tutoriel
    /// #Patrick Watt-Charron 
    /// </summary>
    void Etape4()
    {
        AfficherPanneau(); // Afficher le panneau
        // Mettre à jour les textes:
        _titre.text = "Snowball to attack!";
        _texte.text = "Now that you know how to interact with the big balls on the ground.";
        _etape = 5;
    }

    /// <summary>
    /// Affiche la cinquième étape du tutoriel
    /// Explique que le joueur doit ramasser des boule de neige
    /// #Patrick Watt-Charron
    /// </summary>
    void Etape5()
    {
        AfficherPanneau(); // Afficher le panneau
        // Mettre à jour les textes:
        _titre.text = "Snowball to attack!";
        _texte.text = "You can collect snowballs made by the snowman. !";
    }
    /// <summary>
    /// Cacher le panneau d'instruction
    /// </summary>
    public void CacherPanneau()
    {
        _panneauActive = false; // Désactiver le panneau
        _panneau.SetActive(_panneauActive); // Désactiver le panneau
        _donneesPerso.bougerPerso = true; // Activer le mouvement du joueur
        if (_etape == 3 && !_panneauActive) Invoke("Etape3", 10f); // Afficher la troisième étape
        if (_etape == 5 && !_panneauActive) Etape5();// Afficher la cinquième étape #Patrick Watt-Charron
        _etape++; // Passer à l'étape suivante
    }

    /// <summary>
    /// Afficher le panneau d'instruction
    /// </summary>
    void AfficherPanneau()
    {
        _panneauActive = true; // Activer le panneau
        _panneau.SetActive(_panneauActive); // Activer le panneau
        _donneesPerso.bougerPerso = false; // Désactiver le mouvement du joueur
    }
}
