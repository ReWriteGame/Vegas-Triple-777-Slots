using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameRuler : MonoBehaviour
{
    [SerializeField] private Roulette roulette1;
    [SerializeField] private Arrow arrow1;
    [SerializeField] private Roulette roulette2;
    [SerializeField] private Arrow arrow2;
    [SerializeField] private Roulette roulette3;
    [SerializeField] private Arrow arrow3;
    
    [SerializeField] private ScoreCounter scoreCounter;
    [SerializeField] private Text text;
    
    

    public UnityEvent arrowCoincidedEvent;
    public UnityEvent arrowNotCoincidedEvent;

    private int endWorkSlots = 0;

    public void SetEndSlot()
    {
        endWorkSlots++;
        GetResults();
    }

    private void OnEnable()
    {
        roulette1.endRotateEvent?.AddListener(SetEndSlot);
        roulette2.endRotateEvent?.AddListener(SetEndSlot);
        roulette3.endRotateEvent?.AddListener(SetEndSlot);
    }

    private void OnDisable()
    {
        roulette1.endRotateEvent.RemoveListener(SetEndSlot);
        roulette2.endRotateEvent.RemoveListener(SetEndSlot);
        roulette3.endRotateEvent.RemoveListener(SetEndSlot);
    }

    private float GetUserValue()
    {
        float value = 0;
        if (!string.IsNullOrEmpty(text.text))
        {
            value = float.Parse(text.text);
            if (value > scoreCounter.Score)
                value = scoreCounter.Score;
            if (value < 0) value = 0;
        }

        return value;
    }

    public void GetResults()
    {
        if (endWorkSlots == 3)
        {
            if (arrow1.collidedObject.ID == arrow2.collidedObject.ID || arrow2.collidedObject.ID == arrow3.collidedObject.ID || arrow1.collidedObject.ID == arrow3.collidedObject.ID)
            {
                if (arrow1.collidedObject.ID == arrow2.collidedObject.ID)
                    scoreCounter.Add(GetUserValue());
                if (arrow2.collidedObject.ID == arrow3.collidedObject.ID)
                    scoreCounter.Add(GetUserValue());
                if (arrow1.collidedObject.ID == arrow3.collidedObject.ID)
                    scoreCounter.Add(GetUserValue());
                
                arrowCoincidedEvent?.Invoke();
            }
            else
            {
                scoreCounter.TakeAway(GetUserValue());
                arrowNotCoincidedEvent?.Invoke();
            }
            
            endWorkSlots = 0;
        }
    }
}
