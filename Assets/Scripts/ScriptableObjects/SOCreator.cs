using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOCreator : MonoBehaviour {

    // Make class singleton and destroy if script already exists
    private static SOCreator _instance; // **<- reference link to the class
    public static SOCreator Instance { get { return _instance; } }

    private void Awake() {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public List<PickableSO> LoadAllWeaponsAndArmor() {
        PickableSO[] weapons = Resources.LoadAll<PickableSO>("ScriptableObjects/PickableItems/Weapons/");
        PickableSO[] armors = Resources.LoadAll<PickableSO>("ScriptableObjects/PickableItems/Armors/");

        List<PickableSO> list = new List<PickableSO>();
        list.AddRange(weapons);
        list.AddRange(armors);

        return list;
    }

    public List<ConsumableSO> LoadAllConsumables() {
        ConsumableSO[] loaded = Resources.LoadAll<ConsumableSO>("ScriptableObjects/PickableItems/Consumables/");

        List<ConsumableSO> list = new List<ConsumableSO>();

        foreach (ConsumableSO item in loaded) {
            // Instantiate so that if code changes it's values, the Asset values won't get changed
            ConsumableSO inst = Instantiate(item);
            // Initialize consumable in case some values need to be generated
            inst.Initialize();
            list.Add(inst);
        }

        return list;
    }

    /// <summary>
    /// Loads a weapon from assets with a given name.
    /// 
    /// Weapon does not need to be instantiated since none of it's values change from code.
    /// </summary>
    /// <param name="name">name of the weapon</param>
    /// <returns>retrieved weapon</returns>
    public WeaponSO CreateWeapon(string name) {
        return Resources.Load<WeaponSO>("ScriptableObjects/PickableItems/Weapons/" + name);
    }

    /// <summary>
    /// Loads a armor from assets with a given name.
    /// 
    /// Armor does not need to be instantiated since none of it's values change from code.
    /// </summary>
    /// <param name="name">name of the armor</param>
    /// <returns> retrieved armor</returns>
    public ArmorSO CreateArmor(string name) {
        return Resources.Load<ArmorSO>("ScriptableObjects/PickableItems/Armors/" + name);
    }

    /// <summary>
    /// Creates an instantiated consumable from assets.
    /// </summary>
    /// <param name="serialized">serialized consumable scriptable object</param>
    /// <returns>created consumable</returns>
    public ConsumableSO CreateConsumable(SerializableConsumableSO serialized) {
        Debug.Log(serialized.name);
        ConsumableSO con = Instantiate(Resources.Load<ConsumableSO>("ScriptableObjects/PickableItems/Consumables/" + serialized.name));
        con.batteryType = (ConsumableSO.BatteryType)System.Enum.Parse(typeof(ConsumableSO.BatteryType), serialized.batteryTypeString);
        con.toyWordsType = (WordsType)System.Enum.Parse(typeof(WordsType), serialized.toyWordsTypeString);
        con.quantity = serialized.quantity;
        return con;
    }
}
