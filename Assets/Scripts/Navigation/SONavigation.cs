using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Le ScriptableObject est utilisé pour stocker des données de navigation
[CreateAssetMenu(fileName = "Ma navigation", menuName = "Navigation")]

/// <summary>
/// Classe qui permet d'effectuer 
/// la transition entre les scènes
/// #Élyzabelle Rollin
/// </summary>
public class SONavigation : ScriptableObject
{
    [Header("Références")]
    [SerializeField] SOPerso _donneesPerso; // Référence vers les données du personnage
    [SerializeField] SONiveau _donneesNiveau; // Référence vers les données du niveau
    public Camera mainCamera;
    private GameObject canvas;

    [Header("Sons")]
    [SerializeField] AudioClip _clipBouton; // Son des boutons


    //--------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------
    // Changement de scène
    //--------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Fonction qui permet de changer
    /// de scène pour aller au menu
    /// </summary>
    public void AllerSceneAccueil()
    {
        JouerSonBouton();
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Fonction qui permet de changer
    /// de scène pour aller au tutoriel
    /// </summary>
    public void AllerSceneTutoriel()
    {
        _donneesPerso.tutorielActif1 = true; // Activer le tutoriel
        _donneesPerso.tutorielActif2 = false;
        JouerSonBouton();
        _donneesPerso.Initialiser();
        _donneesNiveau.Initialiser();
        SceneManager.LoadScene(1);
    }


    public void AllerSceneTutoriel2()
    {
        _donneesPerso.tutorielActif1 = false;
        _donneesPerso.tutorielActif2 = true; // Activer le tutoriel
        JouerSonBouton();
        _donneesPerso.Initialiser();
        _donneesNiveau.Initialiser();
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Fonction qui permet de changer 
    /// de scène pour jouer au jeu
    /// </summary>
    public void Jouer()
    {
        _donneesPerso.tutorielActif1 = false; // Désactiver le tutoriel
        _donneesPerso.tutorielActif2 = false; // Désactiver le tutoriel
        JouerSonBouton();
        _donneesPerso.Initialiser();
        _donneesNiveau.Initialiser();
        SceneManager.LoadScene(4);
    }

    /// <summary>
    /// Fonction qui permet de changer
    /// de scène pour aller aux crédits 
    /// </summary>
    public void AllerSceneFin()
    {
        JouerSonBouton();
        SceneManager.LoadScene(3);
    }
    /// <summary>
    /// Fonction qui permet de changer
    /// de scène pour aller aux crédits 
    /// </summary>
    public void AllerSceneCredit()
    {
        JouerSonBouton();
        SceneManager.LoadScene(2);
    }

    /// <summary>
    /// Fonction qui permet de quitter l'application 
    /// </summary>
    public void Quitter()
    {
        JouerSonBouton();
        Application.Quit();
    }

    /// <summary>
    /// Fonction qui permet de retourner au jeu
    /// </summary>
    public void RetourJeu()
    {
        SceneManager.LoadScene(4);
    }

    /// <summary>
    /// Fonction qui permet d'allez dans le mini jeu
    /// </summary>
    public void MiniJeu()
    {
        SceneManager.LoadScene(5);
    }

    //--------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------
    // Jouer le son des boutons
    //--------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------

    void JouerSonBouton()
    {
        if (_clipBouton && GestAudio.instance) GestAudio.instance.JouerEffetSonoreAvecVariation(_clipBouton); // Jouer le son
        else Debug.LogWarning("[SONavigation]: Le clip audio est manquant"); //Message d'erreur
    }
}