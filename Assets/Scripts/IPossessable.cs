using UnityEngine;

public interface IPossessable
{
    void Possess(possessionEventArgs gameobject);
    void ExitPossess();
}
