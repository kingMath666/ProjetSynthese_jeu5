using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Gestionnaire de l'etat du biome
/// S'occupe de changer l'état du biome,
/// gérer son comportement et ses ressources.
/// #Élyzabelle Rollin
/// </summary>
public class BiomesEtatsManager : MonoBehaviour
{
    [Header("Paramètres du biome")]
    public bool _init = true; //Variable qui permet de savoir si le biome a déjà été initialisé
    public bool _estEnneige = false; //Variable qui permet de savoir si le biome est enneigé
    BiomesEtatsBase _etatActuel; //Mémoire de l'etat actuel
    public GameObject _maPlante { get; set; } // Contient la plante du biome
    public List<List<Material>> _biomesMats { get; set; } = new List<List<Material>> { }; // Contient les biomes et leurs variantes
    public Dictionary<string, dynamic> _infos { get; set; } = new Dictionary<string, dynamic>(); // Contient des infos dynamiques du biome

    [Header("Etats du biome")]
    public BiomesEtatActivable _activable = new BiomesEtatActivable(); //instance de la classe
    public BiomesEtatCultivable _cultivable = new BiomesEtatCultivable(); //instance de la classe
    public BiomesEtatFinal _final = new BiomesEtatFinal(); //instance de la classe
    public BiomesEtatFinalVide _finalVide = new BiomesEtatFinalVide(); //instance de la classe

    [Header("Données du perso")]
    public SOPerso _donneesPerso; // Contient les infos du perso

    void Start()
    {
        _init = true;
        _etatActuel = _activable; //État initial
        _etatActuel.IniEtat(this); //Initialisation de l'etat actuel
    }

    /// <summary>
    /// Fonction qui permet de changer l'état du biome
    /// </summary>
    /// <param name="etat"></param>
    public void ChangerEtat(BiomesEtatsBase etat)
    {
        _etatActuel.ExitEtat(this); //Appelle la fonction ExitEtat
        _etatActuel = etat; //Modifie l'etat actuel
        _infos["etat"] = _etatActuel.GetType().Name.Replace("BiomesEtat", ""); //Modifie le nom de l'etat en fonction de l'état actuel
        _etatActuel.IniEtat(this); //Appelle la fonction IniEtat
    }

    void OnTriggerEnter(Collider other)
    {
        _etatActuel.TriggerEnterEtat(this, other); //Appelle la fonction TriggerEnterEtat
    }
    void OnTriggerStay(Collider other)
    {
        _etatActuel.TriggerEnterStay(this, other); //Appelle la fonction TriggerEnterEtat
    }
}
