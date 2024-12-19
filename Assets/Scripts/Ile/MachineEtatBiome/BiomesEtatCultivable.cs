using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

/// <summary>
/// État cultivable du biome
/// Le biome à son état cultivable est enneigé.
/// Il déclenche le changement d'etat du biome en "final".
/// #Élyzabelle Rollin
/// </summary>
public class BiomesEtatCultivable : BiomesEtatsBase
{
    /// <summary>
    /// Start de l'état
    /// </summary>
    /// <param name="biome"> Réference au biome</param>
    public override void IniEtat(BiomesEtatsManager biome) //this = biome
    {
        biome._estEnneige = true; //Biome enneigé
        //Si le biome a une plante, on l'enneige:
        if (biome._maPlante)
        {
            Transform etatBase = biome._maPlante.transform.Find("base"); //Récupérer l'objet sans neige
            etatBase.gameObject.SetActive(false); //Désactiver l'objet sans neige
            Transform neigeChild = biome._maPlante.transform.Find("neige"); //Récupérer l'objet avec neige
            neigeChild.gameObject.SetActive(true); //Activer l'objet avec neige
        }
        int rand = Random.Range(1, 3); //Choix d'un variant 
        biome.GetComponent<Renderer>().material = (Material)Resources.Load("Biomes/Biome" + biome._infos["biome"] + "/b2_" + rand); //Modifier la texture du biome
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="biome"> Réference au biome</param>
    public override void UpdateEtat(BiomesEtatsManager biome)
    {

    }

    /// <summary>
    /// OnTriggerEnter
    /// </summary>
    /// <param name="biome"> Réference au biome </param>
    /// <param name="other"> Collider de l'objet entrant en collision </param>
    public override void TriggerEnterEtat(BiomesEtatsManager biome, Collider other)
    {
        if (other.CompareTag("perso")) biome.ChangerEtat(biome._final); //Changer l'état du biome à "final"
        if (other.CompareTag("bonhomme")) biome.ChangerEtat(biome._activable); //Changer l état du biome à "activable"
    }

    /// <summary>
    ///  OnTriggerStay
    /// </summary>
    /// <param name="biome"> Réference au biome </param>
    /// <param name="other"> Collider de l'objet entrant en collision </param>
    public override void TriggerEnterStay(BiomesEtatsManager biome, Collider other)
    {
        // Filtrer rapidement les objets avec le tag "npc":
        if (!other.CompareTag("npc")) return;

        // Récupérer une fois le composant:
        NPCEtatsManager npcEtatManager = other.GetComponent<NPCEtatsManager>();

        // Valider l'existence du composant et appliquer la logique:
        if (npcEtatManager != null && !npcEtatManager._peutBouger)
        {
            npcEtatManager._peutBouger = true; // Autoriser le mouvement du NPC
        }
    }


    public override void ExitEtat(BiomesEtatsManager biome)
    {
        biome.StopAllCoroutines(); // Stopper toutes les coroutines
    }
}
