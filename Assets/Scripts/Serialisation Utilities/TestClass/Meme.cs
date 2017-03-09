
public class Meme{

    public string bigassstring = "string boi";

    private DialogueNode node = new DialogueNode("meme node", "meme node text");

    public DialogueNode Node {
        get {
            return node;
        }

        set {
            node = value;
        }
    }

    public int simples() {
        return 1 + 1;
    }

}
