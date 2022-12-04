using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
    bool isValidGridPos()
    {
        foreach (Transform child in transform)
        {
            Vector2 v = Playfield.roundVec2(child.position);

            // Check if inside border
            if (!Playfield.insideBorder(v))
                return false;
            if (Playfield.grid[(int)v.x, (int)v.y] != null && Playfield.grid[(int)v.x, (int)v.y].parent != transform)
                return false;
        }
        return true;
    }

    void updateGrid()
    {
        // Older children can be killed
        for (int y = 0; y < Playfield.h; ++y)
            for (int x = 0; x < Playfield.w; ++x)
                if (Playfield.grid[x, y] != null)
                    if (Playfield.grid[x, y].parent == transform)
                        Playfield.grid[x, y] = null;

        // New children needed
        foreach (Transform child in transform)
        {
            Vector2 v = Playfield.roundVec2(child.position);
            Playfield.grid[(int)v.x, (int)v.y] = child;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Is the board too full?
        if (!isValidGridPos())
        {
            // Creating an object of my game over script so that I can end my bloody game with some text
            GameOverScript gameOverScript;
            gameOverScript = GameObject.Find("Canvas").GetComponent<GameOverScript>();
            if (gameOverScript != null)
            {
                gameOverScript.GameOver();
            }
            Debug.Log("GAME OVER!");
            Destroy(gameObject);
        }
    }

    // Time since last gravity tick
    float lastFall = 0;

    // Update is called once per frame
    void Update()
    {
        // Move left
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Z))
        {
            //press ui
            //GameObject.Find("transleft").material.color(0, 0, 0, 100);

            //Change pos
            transform.position += new Vector3(-1, 0, 0);

            // is it valid?
            if (isValidGridPos())
                updateGrid();
            else
                //Revertion
                transform.position += new Vector3(1, 0, 0);
        }

        //Slide to the right
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.C))
        {
            //Change pos
            transform.position += new Vector3(1, 0, 0);

            // is it valid?
            if (isValidGridPos())
                updateGrid();
            else
                //Revertion
                transform.position += new Vector3(-1, 0, 0);
        }

        //Rotation Left
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.D))
        {
            //Change pos
            transform.Rotate(0, 0, -90);

            // is it valid?
            if (isValidGridPos())
                updateGrid();
            else
                //Revertion
                transform.Rotate(0, 0, 90);
        }

        //Rotation Right
        if (Input.GetKeyDown(KeyCode.A))
        {
            //Change pos
            transform.Rotate(0, 0, 90);

            // is it valid?
            if (isValidGridPos())
                updateGrid();
            else
                //Revertion
                transform.Rotate(0, 0, -90);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - lastFall >= 1)
        {
            //Change pos
            transform.position += new Vector3(0, -1, 0);

            // is it valid?
            if (isValidGridPos())
            {
                updateGrid();
            }
            else
            {
                //Revertion
                transform.position += new Vector3(0, -1, 0);

                //Delete the filled row
                Playfield.deleteFullRows();

                // New block needed
                FindObjectOfType<Spawner>().spawnNext();

                // disable script
                enabled = false;
            }
            lastFall = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.X))
        {
            //Do it in a while true loop, break out once it hits the bottom:
            while (true)
            {
                //Change pos
                transform.position += new Vector3(0, -1, 0);

                // is it valid?
                if (isValidGridPos())
                {
                    updateGrid();
                }
                else
                {
                    //Revertion
                    transform.position += new Vector3(0, -1, 0);

                    //Delete the filled row
                    Playfield.deleteFullRows();

                    // New block needed
                    FindObjectOfType<Spawner>().spawnNext();

                    // disable script
                    enabled = false;

                    // break
                    break;
                }
            }

        }
    }
}
