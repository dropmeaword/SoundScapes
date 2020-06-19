using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Main : MonoBehaviour {

    public GameObject tile_set;

    public WangTileSet nope;

    
    void Start() {
        

        /*
                
        1 - select any tile for the NW corner
        2 - tile the top row left to right where the W edge of the tile matches the E edge of the previous tile
        3 - select a first tile for the next row such that the N edge matches the S edge from the tile above
        4 - continue this row, where N and W edges match the S and E edges of the tile above and the left
        5 - go to step 3 if there are more rows

        */

        WangTile[] tiles = Get_Tile_Array();


        int x_steps = 30;
        int y_steps = 30;

        WangTile[][] cells = new WangTile[y_steps][];

        // c# thing
        for (int i = 0; i < cells.Length; i++) {
            cells[i] = new WangTile[x_steps];
        }

        // --------------------------------------------------

        // step 1
        cells[0][0] = Spawn_Tile(tiles[0], new Vector3(0,0,0));
        
        // step 2, fill first row
        float TILE_WIDTH = 30f;
        float TILE_HEIGHT = 30f;

        for (int x = 1; x < x_steps; x++) {

            WangTile tile_left  = cells[0][x-1];

            // get a tile where W matches E
            // SLOPPY!
            List<WangTile> options = new List<WangTile>();
            for (int i = 0; i < tiles.Length; i++) {
                WangTile t = tiles[i];
                if (t.W == tile_left.E) {
                    options.Add(t);
                }
            }
            WangTile selected = options[Random.Range(0, options.Count)];
            cells[0][x] = Spawn_Tile(selected, new Vector3(x*TILE_WIDTH,0,0));

        }

        
        // step 3
        for (int y = 1; y < y_steps; y++) {
                        
            WangTile tile_above = cells[y-1][0];
            List<WangTile> options = new List<WangTile>();

            for (int i = 0; i < tiles.Length; i++) {

                if (i == 0) continue;

                WangTile t = tiles[i];
                if (t.N == tile_above.S) {
                    options.Add(t);
                }
            }
            WangTile selected = options[Random.Range(0, options.Count)];
            cells[y][0] = Spawn_Tile(selected, new Vector3(0*TILE_WIDTH, 0, -y*TILE_HEIGHT));


         
        }

        // step 4
        for (int y = 1; y < y_steps; y++) {
            for (int x = 1; x < x_steps; x++) {
                
                WangTile tile_above = cells[y-1][x];
                WangTile tile_left  = cells[y][x-1];
                
                List<WangTile> options = new List<WangTile>();
                for (int i = 0; i < tiles.Length; i++) {

                    if (i == 0) continue;

                    WangTile t = tiles[i];
                    if (t.N == tile_above.S && t.W == tile_left.E) {
                        options.Add(t);
                    }
                }
                WangTile selected = options[Random.Range(0, options.Count)];
                cells[y][x] = Spawn_Tile(selected, new Vector3(x*TILE_WIDTH, 0, -y*TILE_HEIGHT));
            }
        }



    }

    
    // TODO, remove prefered
    WangTile[] Get_Tile_Array() {
        WangTileSet set = tile_set.GetComponent<WangTileSet>();
        GameObject[] tiles = set.tiles;
        WangTile[] result = new WangTile[tiles.Length];
        for (int i = 0; i < tiles.Length; i++) {
            result[i] = tiles[i].GetComponent<WangTile>();
        }
        return result;
    }


    WangTile Spawn_Tile(WangTile tile, Vector3 pos) {
        return GameObject.Instantiate(tile, pos, Quaternion.Euler(0, 0, 0));
    }




    void Update() {
        
    }




}
