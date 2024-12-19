
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Gestionnaire de l'etat de l'ami
/// S'occupe des comportements de l'ami
/// #Patrick Watt-Charron 
/// </summary>
public class AmisEtatsManager : MonoBehaviour
{
    [Header("Informations du bonhomme de neige")]
    public int _vitesse = 4; //Vitesse de l'ami
    public AmiEtatsBase etatActuel; //État actuel
    public AmiEtatRepos repos = new AmiEtatRepos(); //État repos
    public Dictionary<string, dynamic> infos { get; set; } = new Dictionary<string, dynamic>(); //Contient toutes les infos de l'ami
    public NavMeshAgent agent { get; set; } //Agent de l'ami

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); //Rechercher l'agent de l'ami
        agent.speed = _vitesse; //Ajuster la vitesse
        transform.Rotate(new Vector3(0, Random.Range(0, 360), 0)); //Rotation aléatoire de l'ami
        etatActuel = repos; //État initial
        etatActuel.InitEtat(this); //Initialisation de l'etat actuel
    }

    public void ChangerEtat(AmiEtatsBase etat)
    {
        etatActuel.ExitEtat(this); //Appelle la fonction ExitEtat
        etatActuel = etat; //Modifie l'etat actuel
        etatActuel.InitEtat(this); //Appelle la fonction IniEtat
    }
}
