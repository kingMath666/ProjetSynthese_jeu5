using UnityEngine;

/// <summary>
///  Classe abstraite permettant de 
/// définir les comportement d'un ami
/// #Patrick Watt-Charron
/// </summary>
public abstract class AmiEtatsBase
{
    public abstract void InitEtat(AmisEtatsManager NPC); //Initialisation de l'état
    public abstract void ExitEtat(AmisEtatsManager NPC); //Sortie de l'état
}
