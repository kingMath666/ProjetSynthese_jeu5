using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// État de fin du NPC
/// Le NPC se met en mouvement vers le lac,
/// puis déclenche une animation.
/// #Élyzabelle Rollin
/// </summary>
public class NPCEtatHeureux : NPCEtatsBase
{
    /// <summary>
    /// Start de l'état
    /// </summary>
    /// <param name="NPC"> Réference au NPC </param>
    public override void InitEtat(NPCEtatsManager NPC)
    {
        //Joue le message qui dit que le pingouin est à sa place:
        if (GestAudio.instance != null) GestAudio.instance.JouerVoiceOver(NPC._donneesPerso.sonRemisePingouin[Random.Range(0, NPC._donneesPerso.sonRemisePingouin.Length)]);
        if (GestAudio.instance != null) GestAudio.instance.JouerEffetSonore(NPC._sonHeureux, 2f); // Jouer le son
        NPC._donneesPerso.AjouterPingouin(); // Ajoute un pingouin au joueur 
        NPC.StartCoroutine(Heureux(NPC)); // Démarrer la coroutine
    }

    public override void ExitEtat(NPCEtatsManager NPC)
    {
        NPC.StopAllCoroutines(); // Arrête toutes les coroutines
    }

    /// <summary>
    /// TriggerEnter
    /// </summary>
    /// <param name="NPC"> Réference au NPC </param>
    /// <param name="other"> Collider de l'objet entrant en collision </param>
    public override void TriggerEnterEtat(NPCEtatsManager NPC, Collider other)
    {
    }

    /// <summary>
    /// Gère le comportement lorsque
    /// le pingouin est heureux
    /// </summary>
    /// <param name="NPC"> Réference au NPC </param>
    IEnumerator Heureux(NPCEtatsManager NPC)
    {
        Vector3 objectif = NPC.infos["lac"].transform.position; // Position du lac

        float duration = Random.Range(2f, 5f); // Durée du déplacement unique pour chaque NPC
        float tempsPasse = 0f; // Durée passée

        // Déplacement du NPC vers le lac sur la durée définie:
        while (tempsPasse < duration)
        {
            tempsPasse += Time.deltaTime; // Augmente la durée passée
            NPC._animator.SetBool("marche", true); // Active l'animation de marche
            // Change la destination du NPC pour le lac:
            NPC.agent.destination = objectif;
            yield return null; // Attendre la prochaine frame
        }

        // Change la destination du NPC pour sa position:
        NPC.agent.destination = NPC.transform.position;
        NPC._animator.SetBool("marche", false); // Désactive l'animation de marche

        // Rotation du NPC vers l'opposé du lac
        Vector3 direction = NPC.transform.position - objectif;
        Quaternion rotation = Quaternion.LookRotation(direction);
        NPC.transform.rotation = rotation;

        // Une fois arrivé, jouer une animation de salutation ou de remerciement:
        float random = Random.Range(0f, 1f); // Tirage aléatoire
        //%50 de chance de salutation:
        if (random < 0.5f) NPC._animator.SetBool("salut", true);
        //%50 de chance de remerciement:
        else NPC._animator.SetBool("remerciment", true);
        yield break;
    }

    /// <summary>
    /// OnTriggerExit
    /// </summary>
    /// <param name="NPC"> Réference au NPC </param>
    /// <param name="other"> Collider de l'objet sortant de la collision </param>
    public override void TriggerExitEtat(NPCEtatsManager NPC, Collider other)
    {
    }
}
