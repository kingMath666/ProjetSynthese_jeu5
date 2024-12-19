using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// S'occupe de la logique de l'état de peur.
/// Fuit l'ennemi si il est trop proche.
/// Suit le joueur si il est proche.
/// Sinon, reste immobile.
/// #Élyzabelle Rollin
/// </summary>
public class NPCEtatPeur : NPCEtatsBase
{
    /// <summary>
    /// Start de l'état
    /// </summary>
    /// <param name="NPC"> Réference au NPC </param>
    public override void InitEtat(NPCEtatsManager NPC)
    {
        // Vérifie si le NPC peut se déplacer avant de démarrer l'état:
        if (NPC._peutBouger) NPC.StartCoroutine(GererEtatPeur(NPC));
    }

    /// <summary>
    /// Coroutine pour gérer l'état de peur.
    /// </summary>
    private IEnumerator GererEtatPeur(NPCEtatsManager NPC)
    {
        const float PAUSE = 2f; // Temps d'attente avant la prochaine itération
        // Tant que l'ennemi est dans l'etat de peur et peut bouger:
        while (NPC._etatActuel == NPC._peur && NPC._peutBouger)
        {
            NPC.agent.speed *= 1.5f; // Augmenter la vitesse
            // Calcul des distances entre le NPC et l'ennemi:
            float distanceToEnnemi = Vector3.Distance(NPC.transform.position, NPC.infos["ennemi"].transform.position);

            // Si l'ennemi est trop loin, retourner au repos:
            if (distanceToEnnemi > NPC.infos["vision"] * 2f)
            {
                NPC.ChangerEtat(NPC._repos); // Changer à l'état de repos
                yield break; // Terminer la coroutine après le changement d'état
            }
            FuirEnnemi(NPC); // Fuir l'ennemi

            // Attendre avant la prochaine itération:
            yield return new WaitForSeconds(PAUSE);
        }
        NPC._animator.SetBool("course", false); // Désactiver l'animation de course
        NPC.ChangerEtat(NPC._repos); //Si l'ennemi ne peut pas fuir, retourner au repos
    }

    /// <summary>
    /// Fait fuir le NPC dans la direction opposée à l'ennemi.
    /// </summary>
    private void FuirEnnemi(NPCEtatsManager NPC)
    {
        NPC._animator.SetBool("course", true); // Activer l'animation de course
        Vector3 directionFuite = NPC.transform.position - NPC.infos["ennemi"].transform.position; // Calculer la direction de fuite
        NPC.agent.destination = NPC.transform.position + directionFuite.normalized * 5f; // Deplacer le NPC vers la direction de fuite
    }

    public override void ExitEtat(NPCEtatsManager NPC)
    {
        NPC.StopAllCoroutines(); // Arrête toutes les coroutines
    }

    /// <summary>
    /// OnTriggerEnter
    /// </summary>
    /// <param name="NPC"> Réference au NPC </param>
    /// <param name="other"> Collider de l'objet entrant en collision </param>
    public override void TriggerEnterEtat(NPCEtatsManager NPC, Collider other)
    {
    }

    /// <summary>
    /// OnTriggerExit
    /// </summary>
    /// <param name="NPC"></param>
    /// <param name="other"></param>
    public override void TriggerExitEtat(NPCEtatsManager NPC, Collider other)
    {
        // Si l'ennemi est sorti de la zone de vision:
        if (other.tag == "ennemi" && NPC._ennemiActuel == other.gameObject)
        {
            NPC._animator.SetBool("course", false); // Désactiver l'animation de course
            NPC._ennemiActuel = null; // Réinitialiser l'ennemi
            NPC.infos.Remove("ennemi"); // Supprimer l'ennemi
            NPC.agent.speed = NPC.infos["vitesseIni"]; // Restaurer la vitesse 
            NPC.ChangerEtat(NPC._repos); // Changer à l'état de repos
        }
    }
}
