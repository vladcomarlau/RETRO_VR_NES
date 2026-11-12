using Aya.UNES;
using UnityEngine;

public class NESLoader : MonoBehaviour
{
    public UNESBehaviour nesBehaviour;

    [Header("ROM Library")]
    [Tooltip("Drag your .bytes ROM files here from the Project/Roms folder")]
    public TextAsset[] availableRoms;

    [SerializeField]
    [Tooltip("0 = First ROM, 1 = Second ROM, 2 = Third ROM, etc.")]
    public int selectedRomIndex = 0;

    void Start()
    {
        // 1. Check if we actually put any ROMs in the list
        if (availableRoms.Length == 0)
        {
            Debug.LogError("No ROMs in the list! Please add them in the Inspector.");
            return;
        }

        // 2. Check if the index number is valid
        if (selectedRomIndex < 0 || selectedRomIndex >= availableRoms.Length)
        {
            Debug.LogError("Index out of bounds! Choose a number between 0 and " + (availableRoms.Length - 1));
            return;
        }

        // 3. Load the selected ROM
        TextAsset selectedRom = availableRoms[selectedRomIndex];

        if (selectedRom != null)
        {
            byte[] romBytes = selectedRom.bytes;
            nesBehaviour.Boot(romBytes);
            Debug.Log("Successfully booted: " + selectedRom.name);
        }
        else
        {
            Debug.LogError("The selected ROM slot is empty.");
        }
    }
}