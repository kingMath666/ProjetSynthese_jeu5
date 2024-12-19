using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Permet de créer un objet de type SONiveau dans l'éditeur Unity, avec le nom de fichier "Niveau"
[CreateAssetMenu(fileName = "Niveau", menuName = "Niveau")]
public class SONiveau : ScriptableObject
{
    [Header("Références")]
    [SerializeField] SOPerso _donneesPerso; // Référence vers les données du personnage (ex : informations sur les balles de neige et le tutoriel)

    [Header("Valeurs initiales")]
    [SerializeField, Range(0, 500)] int _nbEnnemisIni; // Nombre initial d'ennemis dans le niveau (modifiable dans l'inspecteur Unity)
    [SerializeField, Range(0, 500)] int _nbTuerIni; // Nombre initial d'ennemis tués (modifiable dans l'inspecteur Unity)

    [Header("Valeurs actuelles")]
    [SerializeField, Range(0, 500)] int _nbEnnemis; // Nombre actuel d'ennemis (modifiable dans l'inspecteur Unity)
    [SerializeField, Range(0, 500)] int _nbTuer; // Nombre actuel d'ennemis tués (modifiable dans l'inspecteur Unity)

    //--------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------
    // Réinitialisation
    //--------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------

    // Méthode pour initialiser les valeurs du niveau
    public void Initialiser()
    {
        // Initialise le nombre d'ennemis tués à sa valeur de départ
        _nbTuer = _nbTuerIni;

        // Vérifie les conditions du tutoriel pour ajuster le nombre d'ennemis en fonction du tutoriel actif
        if (_donneesPerso.tutorielActif1) // Si le tutoriel 1 est actif, il n'y a pas d'ennemis
        {
            _nbEnnemis = 0;
        }
        else if (_donneesPerso.tutorielActif2) // Si le tutoriel 2 est actif, il y a un ennemi
        {
            _nbEnnemis = 1;
        }
        else // Si aucun tutoriel n'est actif, utilise le nombre initial d'ennemis
        {
            _nbEnnemis = _nbEnnemisIni;
        }
    }

    //--------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------
    // Setters et getters
    //--------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------

    // Getter et setter pour le nombre d'ennemis
    public int nbEnnemis
    {
        get => _nbEnnemis; // Récupère le nombre actuel d'ennemis
        set
        {
            // Permet de définir le nombre d'ennemis tout en le limitant à une valeur comprise entre 0 et int.MaxValue
            _nbEnnemis = Mathf.Clamp(value, 0, int.MaxValue);
        }
    }

    // Getter et setter pour le nombre d'ennemis tués
    public int nbTuer
    {
        get => _nbTuer; // Récupère le nombre actuel d'ennemis tués
        set
        {
            // Permet de définir le nombre d'ennemis tués tout en le limitant à une valeur comprise entre 0 et int.MaxValue
            _nbTuer = Mathf.Clamp(value, 0, int.MaxValue);
        }
    }
}