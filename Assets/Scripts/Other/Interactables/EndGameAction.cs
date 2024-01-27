using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameAction : MonoBehaviour, IButtonAction
{
    public void ExecuteAction()
    {
        Debug.Log("Game Ended!");
    }
}

