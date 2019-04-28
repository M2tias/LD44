using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockingManager : MonoBehaviour
{
    private List<Enemy> dockableEnemies;

    // Start is called before the first frame update
    void Start()
    {
        dockableEnemies = new List<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDockable(Enemy e)
    {
        if(!dockableEnemies.Contains(e))
        {
            dockableEnemies.Add(e);
        }
    }

    public List<Enemy> GetDockableEnemies()
    {
        for(int i = dockableEnemies.Count - 1; i >= 0; i--)
        {
            if (dockableEnemies[i] == null)
            {
                dockableEnemies.RemoveAt(i);
            }
        }

        return dockableEnemies;
    }
}
