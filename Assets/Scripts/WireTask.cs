using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireTask : MonoBehaviour
{
    public List<Color> _wireColors = new List<Color>();
    public List<Wire> _leftWires = new List<Wire>();
    public List<Wire> _rightWires = new List<Wire>();
    private List<Color> _availableColors;
    private List<int> _availableLeftWireIndex;
    private List<int> _availableRightWireIndex;

    public Wire CurrentDraggedWire;
    public Wire CurrentHoverWire;

    public bool IsTaskCompleted = false;
    // Start is called before the first frame update
    private void OnEnable()
    {
        _availableColors = new List<Color>(_wireColors);
        _availableLeftWireIndex = new List<int>();
        _availableRightWireIndex = new List<int>();

        for (int i = 0; i < _leftWires.Count; i++)
        {
            _availableLeftWireIndex.Add(i);
            _leftWires[i].Initialize();
        }
        for (int i = 0; i < _rightWires.Count; i++)
        {
            _availableRightWireIndex.Add(i);
            _rightWires[i].Initialize();
        }
        // Generate  New wire each time this object become active
        while (_availableColors.Count > 0 && _availableLeftWireIndex.Count > 0 && _availableRightWireIndex.Count > 0)
        {
            Color pickedColors = _availableColors[Random.Range(0, _availableColors.Count)];
            int pickedLeftWireIndex = Random.Range(0, _availableLeftWireIndex.Count);
            int pickedRightWireIndex = Random.Range(0, _availableRightWireIndex.Count);

            _leftWires[_availableLeftWireIndex[pickedLeftWireIndex]].SetColor(pickedColors);
            _leftWires[_availableLeftWireIndex[pickedLeftWireIndex]].IsLeftWire = true;
            _rightWires[_availableRightWireIndex[pickedRightWireIndex]].SetColor(pickedColors);

            _availableColors.Remove(pickedColors);
            _availableLeftWireIndex.RemoveAt(pickedLeftWireIndex);
            _availableRightWireIndex.RemoveAt(pickedRightWireIndex);


        }
    }
    // void Start()
    // {
    //     Debug.Log("Start");
    //     _availableColors = new List<Color>(_wireColors);
    //     _availableLeftWireIndex = new List<int>();
    //     _availableRightWireIndex = new List<int>();

    //     for (int i = 0; i < _leftWires.Count; i++) { _availableLeftWireIndex.Add(i); }
    //     for (int i = 0; i < _rightWires.Count; i++) { _availableRightWireIndex.Add(i); }

    //     while (_availableColors.Count > 0 && _availableLeftWireIndex.Count > 0 && _availableRightWireIndex.Count > 0)
    //     {
    //         Color pickedColors = _availableColors[Random.Range(0, _availableColors.Count)];
    //         int pickedLeftWireIndex = Random.Range(0, _availableLeftWireIndex.Count);
    //         int pickedRightWireIndex = Random.Range(0, _availableRightWireIndex.Count);

    //         _leftWires[_availableLeftWireIndex[pickedLeftWireIndex]].SetColor(pickedColors);
    //         _leftWires[_availableLeftWireIndex[pickedLeftWireIndex]].IsLeftWire = true;
    //         _rightWires[_availableRightWireIndex[pickedRightWireIndex]].SetColor(pickedColors);

    //         _availableColors.Remove(pickedColors);
    //         _availableLeftWireIndex.RemoveAt(pickedLeftWireIndex);
    //         _availableRightWireIndex.RemoveAt(pickedRightWireIndex);


    //     }

    // }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(CheckTaskCompletion());
    }
    private IEnumerator CheckTaskCompletion()
    {
        // check if all wire is success
        int successfulWires = 0;
        for (int i = 0; i < _rightWires.Count; i++)
        {
            if (_rightWires[i].IsSuccess) { successfulWires++; }
        }
        if (successfulWires >= _rightWires.Count)
        {
            // Task Completed
            gameObject.SetActive(false);
        }
        else
        {
            // Task Incomplete

        }
        yield return new WaitForSeconds(0.5f);

    }
}
