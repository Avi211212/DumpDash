using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This allows us to store a database of all Accessories currently in the bundles, indexed by name.
/// </summary>
public class AccessoriesDatabase
{
    static protected Dictionary<string, CharacterAccessories> m_CharAccessoriesDict;

    static public Dictionary<string, CharacterAccessories> dictionary { get { return m_CharAccessoriesDict; } }

    static protected bool m_Loaded = false;
    static public bool loaded { get { return m_Loaded; } }

    static public CharacterAccessories GetCharAccessory(string type)
    {
        CharacterAccessories charAccessory;
        if (m_CharAccessoriesDict == null || !m_CharAccessoriesDict.TryGetValue(type, out charAccessory))
            return null;

        return charAccessory;
    }

    static public IEnumerator LoadDatabase()
    {
        if (m_CharAccessoriesDict == null)
        {
            m_CharAccessoriesDict = new Dictionary<string, CharacterAccessories>();

            yield return Addressables.LoadAssetsAsync<GameObject>("accessories", op =>
            {
                CharacterAccessories charAccessory = op.GetComponent<CharacterAccessories>();
                if (charAccessory != null)
                {
                    m_CharAccessoriesDict.Add(charAccessory.accessoryName, charAccessory);
                }
            });

            m_Loaded = true;
        }
    }
}