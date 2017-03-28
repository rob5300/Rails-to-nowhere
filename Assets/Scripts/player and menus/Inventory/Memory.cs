using UnityEngine;

public class Memory : Item {

    public Sprite MemorySprite;
    public int CodeDigit;

    public Memory(string name, string id, string description, Sprite memorySprite) : base(name, id, description) {
        MemorySprite = memorySprite;
        Dropable = false;
    }

    public override void OnAddToInventory() {
        CodeDigit = Progression.GetNextDigit();
    }

}
