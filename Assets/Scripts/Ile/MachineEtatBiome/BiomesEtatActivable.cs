using System.Collections;
using UnityEngine;

/// <summary>
/// État d'activation du biome
/// Le biome à son état activable n'est pas enneigé.
/// Il permet l'initialisation des plantes.
/// Il déclenche le changement d'etat du biome en "cultivable".
/// #Élyzabelle Rollin
/// </summary>
public class BiomesEtatActivable : BiomesEtatsBase
{
    /// <summary>
    /// Start de l'état
    /// </summary>
    /// <param name="biome"> Réference au biôme</param>
    public override void IniEtat(BiomesEtatsManager biome)
    {
        biome._estEnneige = false; // Biome non enneigé
                                   // Choix d'un variant
        int rand = Random.Range(1, 3);
        biome.GetComponent<Renderer>().material = (Material)Resources.Load("Biomes/Biome" + biome._infos["biome"] + "/b1_" + rand); //Nouvelle texture

        // Si l'initialisation est déjà terminée, ne pas recréer de plante
        if (biome._init)
        {
            biome._infos["randItem"] = Random.Range(1, 400); // Choix d'une plante
            GameObject laPlante = (GameObject)Resources.Load("Items/Biome" + biome._infos["biome"] + "/i" + biome._infos["randItem"]); // Rechercher la plante

            // Si la plante existe:
            if (laPlante != null)
            {
                biome._maPlante = Object.Instantiate(
                    laPlante,
                    biome.transform.position + Vector3.up / 2,
                    Quaternion.identity,
                    biome._infos["conteneurObjets"]
                ); // Instancier la plante

                float randomizeVal = Random.Range(0.5f, 2f); // Taille de la plante
                biome._maPlante.transform.localScale = Vector3.one * randomizeVal; // Modifier la taille de la plante
            }

            // Marquer l'initialisation comme terminée
            biome._init = false;
        }

        // Gestion de la neige
        if (biome._maPlante != null)
        {
            Transform neige = biome._maPlante.transform.Find("neige"); // Récupérer l'objet avec neige
            // Si l'objet avec neige existe:
            if (neige != null)
            {
                neige.gameObject.SetActive(false); // Toujours désactiver la neige dans cet état
            }
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
    /// OnTriggerEnter
    /// </summary>
    /// <param name="biome"> Réference au biôme</param>
    public override void TriggerEnterEtat(BiomesEtatsManager biome, Collider other)
    {
        //Si le joueur entre en collision, déclenche le changement d'etat:
        if (other.CompareTag("perso")) biome.StartCoroutine(Anime(biome));
        //Si le lac entre en collision, change l'etat du biome à "finalVide":
        if (other.CompareTag("lac")) biome.ChangerEtat(biome._finalVide);
    }

    /// <summary>
    /// Coroutine qui instancie le prefab de particules
    /// et change l'état du biome à "cultivable"
    /// </summary>
    /// <param name="biome"> Réference au biôme </param>
    private IEnumerator Anime(BiomesEtatsManager biome)
    {
        GameObject particlePrefab = Resources.Load<GameObject>("FX/snow"); // Rechercher le prefab de particules 
        if (particlePrefab != null) // Si le prefab existe
        {
            // Instancier le prefab dans la scène:
            GameObject snowParticleObject = GameObject.Instantiate(particlePrefab, biome.transform.position, Quaternion.identity, biome.transform);
            // Récupérer le ParticleSystem du prefab instancié:
            ParticleSystem snowParticle = snowParticleObject.GetComponent<ParticleSystem>();
            if (snowParticle != null) snowParticle.Play(); // Lancer le système de particules s'il existe
            biome.ChangerEtat(biome._cultivable); // Change l'état du biome à "cultivable"
            while (snowParticle.IsAlive(true)) yield return null; // Attendre que le système de particules se termine
            GameObject.Destroy(snowParticleObject); // Supprimer l'objet contenant le système de particules une fois qu'il a fini
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
        if (npcEtatManager != null && npcEtatManager._peutBouger)
        {
            npcEtatManager._peutBouger = false; // Bloquer le mouvement du NPC
        }
    }


    public override void ExitEtat(BiomesEtatsManager biome)
    {
        biome.StopAllCoroutines(); // Stopper toutes les coroutines
    }
}
