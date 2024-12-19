using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Script qui modifie le texte du panneau d'instruction
/// de la création du bonhomme de neige en fonction du 
/// nombre de boules de flocons que le joueur doit avoir
/// pour faire apparaitre le bonhomme de neige
/// S'occupe aussi de jouer le son du voice over qui dit 
/// que le joueur n'a pas assez de boules de neige
/// #Élyzabelle Rollin
/// </summary>
public class InstructionBDN : MonoBehaviour
{
    [Header("Données du perso")]
    [SerializeField] SOPerso _donneesPerso; // Données du perso

    [Header("Paramètres du panneau")]
    // Champ de texte qui affiche combien le joueur doit avoir 
    //de boules de neige pour faire apparaitre le bonhomme de neige:
    [SerializeField] TextMeshProUGUI _textInstructions;

    void Start()
    {
        int floconsManquants = _donneesPerso.prixBonhommeDeNeige - _donneesPerso.nbFlocons; //Calculer le nombre de boules de neige manquantes
        //Si le joueur n'a pas assez de boules de neige:
        if (floconsManquants > 0)
        {
            if (_donneesPerso.sonManqueFlocons != null && GestAudio.instance != null) GestAudio.instance.JouerVoiceOver(_donneesPerso.sonManqueFlocons[Random.Range(0, _donneesPerso.sonManqueFlocons.Length)]); // Jouer le son

            _textInstructions.text = "You need " + _donneesPerso.prixBonhommeDeNeige + " snowflakes to bring this snowman to life."; //Afficher le texte
        }
        //Si le joueur a assez de boules de neige:
        else _textInstructions.text = "You have enough snowflakes to bring this snowman to life."; //Afficher le texte
    }
}
