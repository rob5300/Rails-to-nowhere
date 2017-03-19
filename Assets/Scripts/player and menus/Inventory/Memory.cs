using UnityEngine;

public class Memory : Item {

    public Sprite MemorySprite;

	public Memory(string name, string id, string description, Sprite memorySprite) : base(name, id, description) {
        MemorySprite = memorySprite;
    }

}
