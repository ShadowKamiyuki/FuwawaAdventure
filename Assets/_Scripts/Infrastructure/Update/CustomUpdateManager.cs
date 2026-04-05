using System.Collections.Generic;
using UnityEngine;

public class CustomUpdateManager : MonoBehaviour, IUpdateService
{
    // Lista de todas las clases que deben recibir el Tick()
    private List<IUpdatable> updatables = new List<IUpdatable>();

    private void OnDestroy()
    {
        if (ServiceLocator.Exists<IUpdateService>())
            ServiceLocator.Unregister<IUpdateService>();
    }

    void Update()
    {
        foreach (var u in updatables)
        {
            u.Tick(Time.deltaTime); // Llamamos al método Tick de cada clase registrada
        }
    }

    // Método para registrar una clase al sistema de actualización
    public void Register(IUpdatable updatable)
    {
        if (!updatables.Contains(updatable))
            updatables.Add(updatable);
    }

    // Método para quitar una clase del sistema si ya no necesita actualizarse
    public void Unregister(IUpdatable updatable)
    {
        updatables.Remove(updatable);
    }
}