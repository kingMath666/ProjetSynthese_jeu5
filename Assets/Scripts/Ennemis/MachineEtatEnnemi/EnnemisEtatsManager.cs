using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using Unity.VisualScripting;
public class EnnemisEtatsManager : MonoBehaviour
{
    public EnnemisEtatsBase etatActuel;
    public EnnemisEtatRepos repos = new EnnemisEtatRepos(); //attente (écoute si perso entre dans champ vision)
    public SOPerso _donneesPerso;
    public SONiveau _donneesNiveau;
    public int _dommages = 1;
    public int _vies = 10;
    public NavMeshAgent agent { get; set; }
    public Animator animator { get; set; }
    public Dictionary<string, dynamic> infos { get; set; } = new Dictionary<string, dynamic>(); //dictionnaire
    public AudioClip _sonAttaque;

    void Start()
    {
        infos["vitesse"] = 4f;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        transform.Rotate(new Vector3(0, Random.Range(0, 360), 0)); //random de la direction de départ des ennemis
        infos["vision"] = 30f; //zone de repérage du perso/joueur - distance en unité Unity
        etatActuel = repos;
        etatActuel.InitEtat(this);
    }
    public void ChangerEtat(EnnemisEtatsBase etat)
    {
        etatActuel.ExitEtat(this);
        etatActuel = etat;
        etatActuel.InitEtat(this);
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "balleDeNeige")
        {
            _vies = _vies - _dommages;
        }
        // etatActuel.TriggerEnterEtat(this, other); //Appelle la fonction TriggerEnterEtat
    }

    void Update()
    {
        if (_vies == 0)
        {
            _donneesNiveau.nbTuer++;
            GameObject.Destroy(gameObject);
        }
    }
}