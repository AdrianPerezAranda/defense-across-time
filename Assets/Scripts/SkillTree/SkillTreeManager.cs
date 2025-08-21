using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillTreeManager : MonoBehaviour
{
    // Clase SkillNode
    [System.Serializable]
    public class SkillNode
    {
        public string skillName;       // Nombre de la habilidad
        public int cost;              // Costo de la habilidad
        public Button skillButton;     // Botón de la habilidad
        public string[] prerequisites; // Habilidades necesarias para desbloquear esta habilidad
        public bool isUnlocked = false; // Estado de la habilidad
    }

    public SkillNode[] skills;       // Lista de habilidades
    public int playerPunts = 1000;   // Puntos del jugador
    public TextMeshProUGUI puntText; // Texto de los puntos del jugador

    void Start()
    {
        UpdateUI(); // Actualiza la interfaz de inicio

       // Asigna el evento TryUnlockSkill en cada uno de los botones
        foreach (var skill in skills)
        {
            var currentSkill = skill; 
            skill.skillButton.onClick.AddListener(() => TryUnlockSkill(currentSkill));
        }
    }


    #region Metodos del arbol de habilidades

    // Metodo que comprueba el estado de cada habilidad
    private void TryUnlockSkill(SkillNode skill)
    {
        if (skill.isUnlocked)
        {
            Debug.Log($"{skill.skillName} ya está desbloqueada.");
            return;
        }

        if (!ArePrerequisitesMet(skill))
        {
            Debug.Log($"No cumples los requisitos para desbloquear {skill.skillName}.");
            return;
        }

        if (playerPunts >= skill.cost)
        {
            playerPunts -= skill.cost;
            skill.isUnlocked = true;
            Debug.Log($"{skill.skillName} ha sido desbloqueada.");
        }
        else
        {
            Debug.Log("No tienes suficiente dinero.");
        }

        UpdateUI();
    }


    // Metodo que verifica si cumplen los requisitos para desbloquear las habilidades
    private bool ArePrerequisitesMet(SkillNode skill)
    {
        foreach (var prerequisiteName in skill.prerequisites)
        {
            // Busca la habilidad en la lista
            SkillNode prerequisite = System.Array.Find(skills, s => s.skillName == prerequisiteName);

            if (prerequisite == null || !prerequisite.isUnlocked)
                return false;
        }
        return true;
    }

    // Metodo que actualiza la interfaz 
    private void UpdateUI()
    {
        puntText.text = $"Dinero: {playerPunts}";

        foreach (var skill in skills)
        {
            bool canUnlock = !skill.isUnlocked && ArePrerequisitesMet(skill) && playerPunts >= skill.cost;
            skill.skillButton.interactable = canUnlock;

            var buttonText = skill.skillButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = skill.isUnlocked ? "Desbloqueado" : $"{skill.skillName}\nCosto: {skill.cost}";
            }
        }
    }

#endregion

}
