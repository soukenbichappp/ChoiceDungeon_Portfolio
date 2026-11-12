using UnityEngine;

public class FloorModel
{
    private int _maxFloor = 5;
    private int _currentFloor = 0;

    public int MaxFloor => _maxFloor;
    public int CurrentFloor => _currentFloor;

    public void AddCurrentFloor()
    {
        _currentFloor++;
    }
}
