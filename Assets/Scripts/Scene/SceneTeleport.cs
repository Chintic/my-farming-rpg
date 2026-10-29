using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class SceneTeleport : MonoBehaviour
{
    [SerializeField] private SceneName sceneNameGoto = SceneName.Scene2_Field;
    [SerializeField] private Vector3 scenePositionGoto = new Vector3();

    // We are using "Stay" so our player can just step back to return to the previous scene. If we use OnTriggerEnter, we had to step out and step in to make it trigger
    private void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            float xPosition = Mathf.Approximately(scenePositionGoto.x, 0f)
                ? player.transform.position.x
                : scenePositionGoto.x;
            float yPosition = Mathf.Approximately(scenePositionGoto.y, 0f)
                ? player.transform.position.y
                : scenePositionGoto.y;
            float zPosition = 0f;

            //Teleports to new Scene
            SceneControllerManager.Instance.FadeAndLoadScene(sceneNameGoto.ToString(), new Vector3(xPosition, yPosition, zPosition));

        }
    }
}
