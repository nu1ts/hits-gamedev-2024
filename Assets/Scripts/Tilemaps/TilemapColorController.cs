using UnityEngine;

public class TilemapColorController : MonoBehaviour
{
    public TilemapColorChanger tilemapColorChanger;

    private void Start()
    {
        tilemapColorChanger.ChangeColors(
            "#577277",
            "#a8b5b2",
            "#202e37",
            "#090a14",
            "#394a50"
        );
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            tilemapColorChanger.ChangeColors(
                "#a8ca58",
                "#d0da91",
                "#75a743",
                "#19332d",
                "#d7b594"
            );
        }
    }
}
