using UnityEngine;

[ExecuteAlways]
public class GenerateGUID : MonoBehaviour
{
    [SerializeField] private string _gUID = "";

    public string GUID
    {
        get { return _gUID; }
        set { _gUID = value; }
    }

    public void Awake()
    {
        // Only populate in the editor
        if (!Application.isPlaying)
        {
            // Ensure the object has a guaranteed unique identifier
            if (_gUID == "")
            {
                // Assign GUID
                _gUID = System.Guid.NewGuid().ToString();
            }
        }
    }
}
