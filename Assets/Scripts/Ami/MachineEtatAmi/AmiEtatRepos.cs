using UnityEngine;
using System.Collections;

/// <summary>
/// Etat de repos de l'ami.
/// Se met en mouvement aléatoirement.
/// #Élyzabelle Rollin
/// </summary>
public class AmiEtatRepos : AmiEtatsBase
{
    public override void InitEtat(AmisEtatsManager ami)
    {
        ami.StartCoroutine(Repos(ami)); // Lancer la coroutine
    }
    public override void ExitEtat(AmisEtatsManager ami)
    {
        ami.StopAllCoroutines(); // Stopper toutes les coroutines
    }

    private IEnumerator Repos(AmisEtatsManager ami)
    {
        //Tant que l'ennemi est dans l'etat de repos:
        while (ami.etatActuel == ami.repos)
        {
            //Assigne une nouvelle destination chaque fois:
            AssignerUneNouvelleDestination(ami);
            yield return new WaitForSeconds(Random.value * 10f);
        }
    }

    /// <summary>
    /// Méthode pour assigner une nouvelle destination 
    /// aléatoire à l'ennemi en utilisant le NavMesh.
    /// </summary>
    /// <param name="ami"> Manager d'état de l'ami </param>
    private void AssignerUneNouvelleDestination(AmisEtatsManager ami)
    {
        float rayonDeMouvement = 20f; // Ajuster le rayon de mouvement

        // Générer une nouvelle destination aléatoire autour de l'ennemi:
        Vector3 rand = Random.insideUnitSphere * rayonDeMouvement;
        rand += ami.transform.position; // Ajouter l'emplacement de l'ennemi
        rand.y = ami.transform.position.y; //Conserver la hauteur de l'ennemi

        UnityEngine.AI.NavMeshHit navHit; // Variable pour stocker le NavMeshHit
        // S'assurer que la nouvelle destination est sur le NavMesh:
        if (UnityEngine.AI.NavMesh.SamplePosition(rand, out navHit, rayonDeMouvement, -1))
        {
            ami.agent.destination = navHit.position; // Assigner la nouvelle destination
        }
    }
}
