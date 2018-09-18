using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "NewStats", menuName = "Character/Stats", order = 1)]
public class CharacterStats_SO : ScriptableObject
{
    #region Fields
    public bool setManually = false;
    public bool saveDataOnClose = false;

    public int maxHealth = 0;
    public int currentHealth = 0;

    public int baseDamage = 0;
    public int currentDamage = 0;

    public float baseResistance = 0;
    public float currentResistance = 0f;
    #endregion

    #region Stat Increasers
    public void ApplyHealth(int healthAmount)
    {
        if ((currentHealth + healthAmount) > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += healthAmount;
        }
    }
    #endregion

    #region Stat Reducers
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    #endregion

    #region Death
    private void Death()
    {
        Debug.Log("You kicked it! Sorry Moosa-Magoose.");
        //Call to Game Manager for Death State to trigger respawn
        //Dispaly the Death visualization
    }
    #endregion

    #region SaveCharacterData
    public void saveCharacterData()
    {
        saveDataOnClose = true;
        //EditorUtility.SetDirty(this);
    }
    #endregion
}



