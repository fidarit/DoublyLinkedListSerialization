class Program
{
    public static void Main(string[] args)
    {
        string[] data = new[]
        {
            "a",
            "bb",
            "ccc",
            "dddd",
            "eee",
            "ff",
            "g",
        };

        ListRand listRand = StringArrayToListRand(data);

        using (FileStream stream = new FileStream("result.bin", FileMode.OpenOrCreate))
        {
            listRand.Serialize(stream);
            
            stream.Seek(0, SeekOrigin.Begin);
            listRand = new ListRand();
            listRand.Deserialize(stream);
        }

        string[] savedData = ListRandToStringArray(listRand);
        foreach (string s in savedData)
            Console.WriteLine(s);

        Console.ReadLine();
    }

    public static ListRand StringArrayToListRand(string[] data)
    {
        ListRand listRand = new ListRand();
        Random rand = new Random();

        ListNode current = new ListNode();
        listRand.Head = current;
        listRand.Count = data.Length;
        ListNode[] nodes = new ListNode[data.Length];

        for (int i = 0; i < data.Length; i++)
        {
            current.Data = data[i];

            if (i != data.Length - 1)
            {
                current.Next = new ListNode();
                current.Next.Prev = current;
            }
            else
                listRand.Tail = current;

            nodes[i] = current;
            current = current.Next;
        }

        if(nodes.Length > 1)
        {
            int lastIndex = nodes.Length - 1;
            for (int i = 0; i < nodes.Length; i++)
                nodes[i].Rand = nodes[lastIndex - i];

            if(nodes.Length % 2 == 1)
            {
                int midIndex = nodes.Length / 2;
                nodes[midIndex].Rand = nodes[0].Rand;
                nodes[0].Rand = nodes[midIndex];
            }
        }

        return listRand;
    }

    public static string[] ListRandToStringArray(ListRand listRand)
    {
        string[] result = new string[listRand.Count];
        ListNode current = listRand.Head;
        for (int i = 0; i < listRand.Count; i++)
        {
            result[i] = current.Data;
            current = current.Next;
        }

        return result;
    }
}