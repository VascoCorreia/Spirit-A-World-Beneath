using UnityEngine;

public interface IPossessable
{
    string TypeInPossession { get; set; }
    void Possess(possessionEventArgs gameobject);
    void ExitPossess();
}
