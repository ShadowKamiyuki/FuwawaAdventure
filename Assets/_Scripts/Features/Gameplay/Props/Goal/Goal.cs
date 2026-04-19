using UnityEngine;

public class Goal : MonoBehaviour
{
    private IAppStateMachine _stateMachine;

    private void Awake()
    {
        _stateMachine = ServiceLocator.Get<IAppStateMachine>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _stateMachine.SetState(AppState.GameOver);
        }
    }
}
