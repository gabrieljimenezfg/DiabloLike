using System;
using TMPro;
using UnityEngine;

public class CounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pressCounterText;

    private void Start()
    {
        /*
         * Nos "suscribimos" al evento del player, "PressCounterChanged"
         * Cuando el Player llame al evento, PressCounterChanged.Invoke, la funcion "PlayerOnPressCounterChanged" se ejecuta
         *
         * Nuestro CounterUI solo se interesa por lo visual, la logica se la deja al player
         */
        PlayerDeMentira.Instance.PressCounterChanged += PlayerOnPressCounterChanged;
    }

    private void PlayerOnPressCounterChanged(object sender, EventArgs e)
    {
        /*
         * Recibimos el evento, lo de "object sender" y "EventArgs e" son parametros default de los eventos de C#
         * Sender, en este caso es el player, y "e" tendria argumentos extra si los quisieramos,
         * por ejemplo, un daño que hicimos, OnPlayerHit
         *
         * En este caso no necesitamos mas info, simplemente sabemos que hay que actualizar el UI
         */
        UpdateCounterVisual();
    }

    private void UpdateCounterVisual()
    {
        int currentCounterValue = PlayerDeMentira.Instance.GetCounter();
        pressCounterText.text = "Presses: " + currentCounterValue;
    }
}