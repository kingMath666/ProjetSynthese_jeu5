using UnityEngine;
using System.Collections;

public class EnnemisEtatRepos : EnnemisEtatsBase
{
    public override void InitEtat(EnnemisEtatsManager ennemi)
    {
        ennemi.animator.SetBool("enCourse", true); // Enable walking animation if applicable
        ennemi.animator.SetBool("enAttaque", false);

        ennemi.agent.speed = 4f; // Slower speed for wandering
        ennemi.StartCoroutine(Repos(ennemi));
    }

    /// <summary>
    /// Couroutine pour l'etat de repos de l'ennemi.
    /// </summary>
    /// <param name="ennemi"> Manager d'état de l'ennemi </param>
    /// <returns></returns>
    private IEnumerator Repos(EnnemisEtatsManager ennemi)
    {
        //Tant que l'ennemi est dans l'etat de repos:
        while (ennemi.etatActuel == ennemi.repos)
        {
            if (ennemi.infos["cible"] != null)
            {
                Destination(ennemi);
            }
            yield return new WaitForSeconds(Random.value * 10f);
        }
    }

    private void Destination(EnnemisEtatsManager ennemi)
    {
        Vector3 positionActuelle = ennemi.transform.position;
        Vector3 cible = ennemi.infos["cible"];
        Vector3 maison = ennemi.infos["maison"];

        // Calculer la distance uniquement sur les axes X et Z
        float distanceCible = Vector2.Distance(new Vector2(positionActuelle.x, positionActuelle.z), new Vector2(cible.x, cible.z));
        float distanceMaison = Vector2.Distance(new Vector2(positionActuelle.x, positionActuelle.z), new Vector2(maison.x, maison.z));

        if (distanceCible < 2f)
        {
            ennemi.agent.destination = maison;
        }
        else if (distanceMaison < 2f)
        {
            ennemi.agent.destination = cible;
        }
    }


    public override void TriggerEnterEtat(EnnemisEtatsManager ennemi, Collider other)
    {
        // No action needed for now in repose state
    }
    public override void ExitEtat(EnnemisEtatsManager biome)
    {
        biome.StopAllCoroutines();
    }
}
