using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderService : MonoBehaviour, ISceneLoader
{
    public bool IsLoading { get; private set; }
    public float Progress { get; private set; }

    private const string LOADING_SCENE_NAME = "Loading";
    private const float MIN_LOADING_TIME = 0.6f;

    public async Task ProcessRequest(LoadingRequest request)
    {
        if (IsLoading)
            return;

        IsLoading = true;
        Progress = 0f;

        // 1. Asegurar loading cargado
        await EnsureLoadingLoaded();

        var loadingView = ServiceLocator.Get<LoadingView>();

        // 2. Fade IN (pantalla negra)
        await loadingView.FadeInAsync();

        float startTime = Time.unscaledTime;

        // 3. Descargar escenas
        foreach (var scene in request.ScenesToUnload)
        {
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(scene);
            await AwaitOperation(unloadOp, loadingView);
        }

        // 4. Cargar escenas nuevas
        foreach (var scene in request.ScenesToLoad)
        {
            AsyncOperation loadOp = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            await AwaitOperation(loadOp, loadingView);
        }

        Progress = 1f;
        loadingView.SetProgress(1f);

        // 5. Tiempo mínimo
        float elapsed = Time.unscaledTime - startTime;
        if (elapsed < MIN_LOADING_TIME)
        {
            await Task.Delay(Mathf.CeilToInt((MIN_LOADING_TIME - elapsed) * 1000f));
        }

        var scene2 = SceneManager.GetSceneByName(request.ScenesToLoad[0]);

        Debug.Log("Scene loaded: " + scene2.isLoaded);
        Debug.Log("Scene valid: " + scene2.IsValid());
        // 6. Activar escena principal
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(request.ScenesToLoad[0]));

        // 7. Fade OUT (revela escena nueva)
        await loadingView.FadeOutAsync();

        IsLoading = false;
    }

    private async Task EnsureLoadingLoaded()
    {
        if (ServiceLocator.Exists<LoadingView>())
            return;

        AsyncOperation op = SceneManager.LoadSceneAsync(LOADING_SCENE_NAME, LoadSceneMode.Additive);

        while (!op.isDone)
            await Task.Yield();

        // MUY IMPORTANTE: esperar a que Awake registre el servicio
        while (!ServiceLocator.Exists<LoadingView>())
            await Task.Yield();
    }

    private async Task AwaitOperation(AsyncOperation operation, LoadingView loadingView)
    {
        while (!operation.isDone)
        {
            Progress = Mathf.Clamp01(operation.progress / 0.9f);

            loadingView.SetProgress(Progress);

            await Task.Yield();
        }
    }
}