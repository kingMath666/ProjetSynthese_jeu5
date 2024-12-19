using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Reflection.Emit;

/// <summary>
/// État final et vide du biome
/// Le biome à son état final vide est enneigé.
/// N'instancie aucune ressource.
/// #Élyzabelle Rollin
/// </summary>
public class BiomesEtatFinalVide : BiomesEtatsBase
{
    /// <summary>
    /// Start de l'état
    /// </summary>
    /// <param name="biome"> Réference au biome </param>
    public override void IniEtat(BiomesEtatsManager biome)
    {
        biome._init = false; //Désactiver l'initialisation
        biome._estEnneige = true; //Biome enneigé
        //Si le biome a une plante, on l'enneige:
        if (biome._maPlante) Object.Destroy(biome._maPlante); //Destruction de la plante
        ChangerTexture(biome); //Modifier la texture
    }

    /// <summary>
    /// Fonction permettant de changer 
    /// la texture du biome
    /// </summary>
    /// <param name="biome"> Réference au biome </param>
    void ChangerTexture(BiomesEtatsManager biome)
    {
        int rand = Random.Range(1, 3); //Choix d'un variant
        biome.GetComponent<Renderer>().material = (Material)Resources.Load("Biomes/Biome" + biome._infos["biome"] + "/b3_" + rand); //Modifier la texture du biome
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="biome"> Réference au biôme</param>
    public override void UpdateEtat(BiomesEtatsManager biome)
    {

    }

    /// <summary>
    /// TriggerEnter
    /// </summary>
    /// <param name="biome"> Réference au biome </param>
    /// <param name="other"> Collider de l'objet entrant en collision </param>
    public override void TriggerEnterEtat(BiomesEtatsManager biome, Collider other)
    {
        //Si on entre en collision avec un bonhomme de neige:
        if (other.CompareTag("bonhomme"))
        {
            float rand = Random.value; //Valeur aleatoire
            //Instancier boule de neige avec 50% de chance:
            if (rand > 0.5)
            {
                GameObject balleDeNeigePrefab = (GameObject)Resources.Load("Items/General/i1"); //Prefab de la boule de neige
                //Instancier la balle de neige:
                GameObject balleDeNeige = Object.Instantiate(balleDeNeigePrefab, biome.transform.position + Vector3.up, Quaternion.identity);
            }
            biome.ChangerEtat(biome._cultivable); //Retour à l'état "cultivable"
        }
    }

    /// <summary>
    /// TriggerEnterStay
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
        biome.StopAllCoroutines(); //Stopper toutes les coroutines
    }
}
