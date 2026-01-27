using System;
using UnityEngine;

public class PlayerDeMentira : MonoBehaviour
{
    public static PlayerDeMentira Instance;
    
    /*
     * la parte logica del contador, al player no le importa la UI y no sabe que existe, asi que no depende de nada
     * la UI es la que se encargara de reaccionar a estos cambios
     */
    
    private int pressCounter;

    /*
     * Esto es un evento de C#, permite invocarlo para que los scripts que esten interesados, reaccionen
     */
    public event EventHandler PressCounterChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pressCounter++;
            /*
             * para quien este interesado: avisamos que press counter cambió
             * lo de "this" y "event args empty" son parametros que un evento puede recibir
             *
             * se ven estos parametros usados en CounterUI
             *
             * asi, el player no necesita saber de quien necesita nada, simplemente emite este evento, y
             * lo que necesite reaccionar lo hace, en este caso, el UI
             */
            
            PressCounterChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public int GetCounter()
    {
        return pressCounter;
    }
}