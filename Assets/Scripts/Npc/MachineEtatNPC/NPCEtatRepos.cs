using UnityEngine;
using System.Collections;
using System.Security.Cryptography;

/// <summary>
/// État de repos de l'ennemi
/// Est en attente de l'action du joueur
/// #Élyzabelle Rollin
/// </summary>
public class NPCEtatRepos : NPCEtatsBase
{
    /// <summary>
    ///  Start de l'état
    /// </summary>
    /// <param name="NPC"> Réference au NPC </param>
    public override void InitEtat(NPCEtatsManager NPC)
    {
        NPC.infos["vitesseIni"] = NPC.agent.speed;
        NPC.StartCoroutine(Repos(NPC)); // Lance la coroutine
    }

    public override void ExitEtat(NPCEtatsManager NPC)
    {
        NPC.StopAllCoroutines(); // Arrête toutes les coroutines
    }

    /// <summary>
    /// Coroutine pour gérer l'état de repos
    /// </summary>
    /// <param name="NPC">Réference au NPC</param>
    private IEnumerator Repos(NPCEtatsManager NPC)
    {
        NPC.agent.destination = NPC.transform.position; // Change la destination du NPC
        //Tant que l'ennemi est dans l'etat de repos:
        while (NPC._etatActuel == NPC._repos)
        {
            //Vérifie si le joueur est proche:
            float distance = Vector3.Distance(NPC.transform.position, NPC.infos["perso"].transform.position);

            //Si le joueur est proche, changer à l'état suivant
            if (distance < NPC.infos["vision"] && distance >= NPC._distanceEntrePerso && NPC._peutBouger)
            {
                if (GestAudio.instance != null) GestAudio.instance.JouerVoiceOver(NPC._donneesPerso.sonPingouinVientIci[Random.Range(0, NPC._donneesPerso.sonPingouinVientIci.Length)]);
                NPC.ChangerEtat(NPC._suivrePerso); // Change à l'état suivant
                yield break; // Sortir de la coroutine
            }
            yield return new WaitForSeconds(2f);
        }
    }

    /// <summary>
    ///  OnTriggerEnter
    /// </summary>
    /// <param name="NPC"> Réference au NPC </param>
    /// <param name="other"> Collider de l'objet entrant en collision </param>
    public override void TriggerEnterEtat(NPCEtatsManager NPC, Collider other)
    {
    }

    /// <summary>
    ///  OnTriggerExit
    /// </summary>
    /// <param name="NPC"> Réference au NPC </param>
    /// <param name="other"> Collider de l'objet sortant de la collision </param>
    public override void TriggerExitEtat(NPCEtatsManager NPC, Collider other)
    {
    }
}
