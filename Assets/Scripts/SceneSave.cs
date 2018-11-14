using System.Collections.Generic;

[System.Serializable]
public class SceneSave
{
    public List<int> modelIds = new List<int>();

    public List<float> modelXPositions = new List<float>();
    public List<float> modelYPositions = new List<float>();
    public List<float> modelZPositions = new List<float>();

    public List<float> modelXRotations = new List<float>();
    public List<float> modelYRotations = new List<float>();
    public List<float> modelZRotations = new List<float>();

    public List<float> modelXScales = new List<float>();
    public List<float> modelYScales = new List<float>();
    public List<float> modelZScales = new List<float>();
}
