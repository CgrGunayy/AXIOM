using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerPositionerForCinematic : MonoBehaviour, IInteractionResult
{
    [SerializeField] private FirstPersonController player;
    [SerializeField] private Transform point;
    [Space]
    [SerializeField] private PlayableDirector cinematic;

    bool playing;

    public void Run()
    {
        if (!playing)
            StartCoroutine(PositionEnumerator());
    }

    public IEnumerator PositionEnumerator()
    {
        playing = true;
        player.playerCanMove = false;
        player.cameraCanMove = false;
        Vector3 startPos = player.transform.position;
        Quaternion startRot = player.transform.rotation;
        float time = 0.0f;
        WaitForFixedUpdate endFrame = new WaitForFixedUpdate();

        while (true)
        {
            yield return endFrame;
            time += Time.fixedDeltaTime;
            if (time > 1.0f)
                break;

            player.transform.position = Vector3.Lerp(startPos, point.position, time);
            player.transform.rotation = Quaternion.Lerp(startRot, point.rotation, time);
        }

        if (cinematic != null)
        {
            cinematic.Play();

            while (cinematic.state == PlayState.Playing)
                yield return endFrame;

            playing = false;
            player.cameraCanMove = true;
            player.playerCanMove = true;
        }
        else
        {
            playing = false;
        }
    }
}
