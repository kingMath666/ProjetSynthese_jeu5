using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Reflection.Emit;

/// <summary>
/// État final du biome
/// Le biome à son état final est enneigé.
/// Il instancie les flocons et les grosse boules de neige.
/// #Élyzabelle Rollin
/// </summary>
public class BiomesEtatFinal : BiomesEtatsBase
{
    public List<GameObject> listeDesPrix = new List<GameObject>(); //Liste des prix de flocons instanciés

    /// <summary>
    /// Start de l'état
    /// </summary>
    /// <param name="biome"> Réference au biome </param>
    public override void IniEtat(BiomesEtatsManager biome)
    {
        biome._estEnneige = true; //Biome enneigé
        ChangerTexture(biome); //Changer la texture du biome
        InstancierRessources(biome); //Instancier les flocons
        InstancierGrosseBoule(biome); //Instancier la grosse boule
    }

    /// <summary>
    /// Changer la texture du biome
    /// pour la texture de l'état final
    /// </summary>
    /// <param name="biome"> Réference au biome </param>
    void ChangerTexture(BiomesEtatsManager biome)
    {
        int rand = Random.Range(1, 3); //Choix d'un variant
        biome.GetComponent<Renderer>().material = (Material)Resources.Load("Biomes/Biome" + biome._infos["biome"] + "/b3_" + rand); //Modifier la texture du biome
    }

    /// <summary>
    /// Permet d'instancier les flocons
    /// </summary>
    /// <param name="biome"> Reference au biome </param>
    void InstancierRessources(BiomesEtatsManager biome)
    {
        GameObject prefabPrix = (GameObject)Resources.Load("Items/General/flocon"); //Prefab du flocon
        //80% de chance d'instancier un flocon:
        if (Random.value > 0.8f)
        {
            int qte = Random.Range(0, 4); //Quantité de flocons à instancier 
            for (int i = 0; i < qte; i++)
            {
                //Instancier le flocon:
                GameObject flocon = Object.Instantiate(prefabPrix, biome.transform.position + Vector3.up, Quaternion.identity, biome._infos["conteneurObjets"]);
                listeDesPrix.Add(flocon); // Ajouter l'instance à la liste 'prix'
                flocon.transform.position += new Vector3(i * Random.value, 0, i * Random.value); // Décalage pour éviter la superposition
            }
        }
    }

    /// <summary>
    /// Permet d'instancier la grosse boule
    /// </summary>
    /// <param name="biome"> Reference au biome </param>
    void InstancierGrosseBoule(BiomesEtatsManager biome)
    {
        GameObject grosseBoulePrefab = (GameObject)Resources.Load("Items/General/grosseBoule"); //Prefab de la grosse boule
        float rand = Random.Range(0f, 1f); //Tirage aleatoire
        //0.01% de chance d'instancier la grosse boule:
        if (rand > 0.996f)
        {
            //Instancier la grosse boule:
            GameObject grosseBoule = Object.Instantiate(grosseBoulePrefab, biome.transform.position + Vector3.up, Quaternion.identity);
        }
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
        GameObject balleDeNeigePrefab = (GameObject)Resources.Load("Items/General/i1"); //Prefab de la balle de neige
        //Si l'objet entrant en collision est un bonhomme de neige:  
        if (other.CompareTag("bonhomme"))
        {
            float rand = Random.value; //Valeur aleatoire
            //Instancier balle de neige avec 50% de chance:
            if (rand > 0.5)
            {
                //Instancier la balle de neige:
                GameObject balleDeNeige = Object.Instantiate(balleDeNeigePrefab, biome.transform.position + Vector3.up, Quaternion.identity);
            }
            biome.ChangerEtat(biome._activable); //Retour à l'état "activable"
        }
    }

    /// <summary>
    /// OnTriggerStay
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
