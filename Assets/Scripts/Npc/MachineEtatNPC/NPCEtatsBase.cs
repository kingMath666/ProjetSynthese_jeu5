using UnityEngine;

/// <summary>
/// Classe abstraite de base des états du NPC
/// #Patrick Watt-Charron
/// </summary>
public abstract class NPCEtatsBase
{
    public abstract void InitEtat(NPCEtatsManager NPC); // Initialisation de l'état
    public abstract void ExitEtat(NPCEtatsManager NPC); // Sortie de l'état
    public abstract void TriggerEnterEtat(NPCEtatsManager NPC, Collider other); // OnTriggerEnter
    public abstract void TriggerExitEtat(NPCEtatsManager NPC, Collider other); // OnTriggerExit
}
