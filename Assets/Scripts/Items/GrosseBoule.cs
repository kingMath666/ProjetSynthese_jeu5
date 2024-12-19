using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Gère le comportement de la grosse boule
/// lorsque le joueur est proche et lorsqu'il
/// achète le bonhomme de neige
/// #Élyzabelle Rollin
/// </summary>
public class GrosseBoule : MonoBehaviour
{
    [Header("Données du joueur")]
    [SerializeField] SOPerso _donneesPerso; // Données du joueur
    [SerializeField] Perso _perso; // Reference du personnage

    [Header("FX")]
    [SerializeField] ParticleSystem _fxGrosseBoule; // Effet d'explosion de neige

    [Header("Panneau d'instruction")]
    [SerializeField] GameObject _panneauAInstancier; //Prefab du panneau d'instruction
    GameObject _panneau; // Reference du panneau d'instruction
    bool _isInTriggerZone = false; // Indique si le joueur est dans la zone de trigger

    void OnTriggerEnter(Collider other)
    {
        // Vérifier si le joueur entre en collision:
        if (other.CompareTag("perso"))
        {
            _isInTriggerZone = true; // Indiquer que le joueur est dans la zone de trigger
            _panneau = Instantiate(_panneauAInstancier); // Instancier le panneau d'instruction
            if (GestAudio.instance) GestAudio.instance.ChangerEtatLecturePiste(TypePiste.MusiqueEvenA, true); // Jouer la musique
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("perso"))
        {
            _isInTriggerZone = false; // Indiquer que le joueur est hors de la zone de trigger
            Destroy(_panneau); // Supprimer le panneau d'instruction
            if (GestAudio.instance) GestAudio.instance.ChangerEtatLecturePiste(TypePiste.MusiqueEvenA, false); // Retirer la musique
        }
    }

    void Update()
    {
        // Vérifier si le joueur appuie sur E et qu'il est dans la zone de trigger:
        if (_isInTriggerZone && Input.GetKeyDown(KeyCode.E))
        {
            // Vérifier si le joueur peut acheter le bonhomme de neige: 
            if (_donneesPerso.nbFlocons >= _donneesPerso.prixBonhommeDeNeige)
            {
                Instantiate(_fxGrosseBoule, transform.position, Quaternion.identity); //Instancier l'effet d'explosion
                _perso.InvokeBonhomeDeNeige(gameObject.transform); //Instancier le bonhomme de neige #Patrick Watt-Charron
                Destroy(_panneau); // Supprimer le panneau d'instruction
                Destroy(gameObject); // Supprimer la grosse boule
            }
        }
    }
}
