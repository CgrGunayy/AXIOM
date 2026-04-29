using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class CutsceneTrigger : MonoBehaviour
{
    PlayableDirector director;

    bool played;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        director.RebuildGraph();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (played)
            return;

        played = true;
        director.Play();
    }
}
