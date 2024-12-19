using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Etat de poursuite du NPC
/// Le NPC se met en mouvement vers le joueur.
/// Arrête si le joueur est hors de son champ 
/// de vision ou est trop près du joueur
/// #Élyzabelle Rollin
/// </summary>
public class NPCEtatSuivre : NPCEtatsBase
{

    /// <summary>
    ///  Start de l'état
    /// </summary>
    /// <param name="NPC"> Référence au NPC </param>
    public override void InitEtat(NPCEtatsManager NPC)
    {
        //Vérifie si le NPC peut se bouger avant de demarrer l'état:
        if (NPC._peutBouger) NPC.StartCoroutine(SuivrePerso(NPC));
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

    public override void ExitEtat(NPCEtatsManager NPC)
    {
        NPC.StopAllCoroutines(); // Arrête toutes les coroutines
    }

    /// <summary>
    /// Couroutine qui permet au NPC de suivre le joueur lorsque
    /// celui-ci entre dans son champ de vision et que le NPC a
    /// un chemin possible enneigé
    /// </summary>
    /// <param name="NPC">NPCEtatsManager</param>
    private IEnumerator SuivrePerso(NPCEtatsManager NPC)
    {
        ///Tant que le NPC est dans l'etat de poursuite et peut bouger:
        while (NPC._etatActuel == NPC._suivrePerso && NPC._peutBouger)
        {
            //Change la destination du NPC pour le perso:
            NPC.agent.destination = NPC.infos["perso"].transform.position;
            NPC._animator.SetBool("marche", true); //Active l'animation de marche

            //Si la distance entre le NPC et le perso 
            //est plus grande que le champ de vision du NPC: 
            float distance = Vector3.Distance(NPC.transform.position, NPC.infos["perso"].transform.position);
            if (distance > NPC.infos["vision"] || distance <= NPC._distanceEntrePerso || !NPC._peutBouger)
            {
                NPC._animator.SetBool("marche", false); //Désactive l'animation de marche
                if (GestAudio.instance != null) GestAudio.instance.JouerVoiceOver(NPC._donneesPerso.sonPingouinAttend[Random.Range(0, NPC._donneesPerso.sonPingouinAttend.Length)]);
                NPC.ChangerEtat(NPC._repos); //Change pour l'état de repos
                yield break;
            }
            yield return new WaitForSeconds(2f); //Attend avant de recommencer la coroutine
        }
        NPC._animator.SetBool("marche", false); //Désactive l'animation de marche
        if (GestAudio.instance != null) GestAudio.instance.JouerVoiceOver(NPC._donneesPerso.sonPingouinAttend[Random.Range(0, NPC._donneesPerso.sonPingouinAttend.Length)]);
        NPC.ChangerEtat(NPC._repos); //Change pour l'état de repos
    }
}
