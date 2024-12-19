using UnityEngine;

/// <summary>
///  Classe abstraite permettant de 
/// définir les comportement d'un biome
/// </summary>
public abstract class BiomesEtatsBase
{
    public abstract void IniEtat(BiomesEtatsManager biome); // Initialisation de l'état
    public abstract void ExitEtat(BiomesEtatsManager biome); // Sortie de l'état
    public abstract void UpdateEtat(BiomesEtatsManager biome); //Fonction Update
    public abstract void TriggerEnterEtat(BiomesEtatsManager biome, Collider other); //Fonction de OnTriggerEnter
    public abstract void TriggerEnterStay(BiomesEtatsManager biome, Collider other); //Fonction de OnTriggerStay
}
