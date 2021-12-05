using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockManifoldsTask : MonoBehaviour
{
    [SerializeField] private List<UnlockManifoldButton> _ButtonList = new List<UnlockManifoldButton>();
    private int currentValue;
    private void OnEnable()
    {
        List<int> numbers = new List<int>();
        for (int i = 0; i < _ButtonList.Count; i++)
        {
            numbers.Add(i + 1);

        }
        for (int i = 0; i < _ButtonList.Count; i++)
        {
            int pickedNumber = numbers[Random.Range(0, numbers.Count)];
            // initialize button
            _ButtonList[i].Initialize(pickedNumber, this);
            numbers.Remove(pickedNumber);
        }
        currentValue = 1;
    }
    private void ResetButton()
    {
        foreach (UnlockManifoldButton button in _ButtonList)
        {
            button.ToggleButton(true);
        }
    }
    public void OnButtonPressed(int buttonID, UnlockManifoldButton currentButton)
    {
        if (currentValue >= _ButtonList.Count)
        {
            Debug.Log("Unlock Manifold Task Complete");
            gameObject.SetActive(false);
        }
        // check if the correct button
        if (currentValue == buttonID)
        {
            currentValue++;
            currentButton.ToggleButton(false);
        }
        else
        {
            currentValue = 1;
            ResetButton();
        }
        Debug.Log("Button pressed " + buttonID);
    }

}
