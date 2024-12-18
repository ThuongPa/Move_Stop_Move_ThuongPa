using UnityEngine;
using System.Collections.Generic;

public class Cache
{
    private static Dictionary<float, WaitForSeconds> m_WFS = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds GetWFS(float key)
    {
        if (!m_WFS.ContainsKey(key))
        {
            m_WFS[key] = new WaitForSeconds(key);
        }

        return m_WFS[key];
    }

    //Cache character
    private static Dictionary<Collider, Character> m_Character = new Dictionary<Collider, Character>();
    public static Character GetCharacter(Collider key)
    {
        if (!m_Character.ContainsKey(key))
        {
            Character character = key.GetComponent<Character>();

            if (character != null)
            {
                m_Character.Add(key, character);
            }
            else
            {
                return null;
            }
        }
        return m_Character[key];
    }

    private static Dictionary<Collider, Player> m_Player = new Dictionary<Collider, Player>();
    public static Player GetPlayer(Collider key)
    {
        if (!m_Player.ContainsKey(key))
        {
            Player player = key.GetComponent<Player>();

            if (player != null)
            {
                m_Player.Add(key, player);
            }
            else
            {
                return null;
            }
        }

        return m_Player[key];
    }
}
