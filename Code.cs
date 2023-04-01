using System.Text;

class ListNode
{
    public ListNode Prev;
    public ListNode Next;
    public ListNode Rand; // произвольный элемент внутри списка
    public string Data;
}


class ListRand
{
    public ListNode Head;
    public ListNode Tail;
    public int Count;

    public void Serialize(FileStream s)
    {
        int[] randIndexes = new int[Count];
        ListNode current = Head;
        ListNode current2 = current.Next;  
        for (int i = 0, j = 0; i < Count && j < Count; i++)
        {
            if (current != current2 && current.Rand == current2)
            {
                randIndexes[j] = i;
                current = current.Next;
                current2 = Head;
                j++;
                i = 0;
            }
            current2 = current2.Next;
        }

        //запись в файл
        s.Write(BitConverter.GetBytes(Count));
        current = Head;
        for (int i = 0; i < Count; i++)
        {
            s.Write(BitConverter.GetBytes(randIndexes[i]));
            s.Write(BitConverter.GetBytes(current.Data.Length));
            s.Write(Encoding.UTF8.GetBytes(current.Data));
            current = current.Next;
        }
    }

    public void Deserialize(FileStream s)
    {
        byte[] bytes = new byte[4];
        s.Read(bytes);
        Count = BitConverter.ToInt32(bytes);

        Head = new ListNode();
        ListNode current = Head;
        int[] randIndexes = new int[Count];

        for (int i = 0; i < Count; i++)
        {
            if (i == 0)
            {
                bytes = new byte[4];
                s.Read(bytes);
                randIndexes[i] = BitConverter.ToInt32(bytes);

                s.Read(bytes);
                int dataSize = BitConverter.ToInt32(bytes);

                bytes = new byte[dataSize];
                s.Read(bytes);
                current.Data = Encoding.UTF8.GetString(bytes);

                current.Next = new ListNode();
            }
            else
            {
                current.Next.Prev = current;
                current = current.Next;

                bytes = new byte[4];
                s.Read(bytes);
                randIndexes[i] = BitConverter.ToInt32(bytes);

                s.Read(bytes);
                int dataSize = BitConverter.ToInt32(bytes);

                bytes = new byte[dataSize];
                s.Read(bytes);
                current.Data = Encoding.UTF8.GetString(bytes);

                if (i != Count - 1)
                    current.Next = new ListNode();
                else
                    Tail = current;
            }
        }

        current = Head;                 
        ListNode current2 = current;      
        for (int i = 0, j = 0; i < Count; i++)
        {
            if (randIndexes[j] == i)
            {
                current.Rand = current2;
                current = current.Next;
                j++;
                current2 = Head;
                i = 0;
            }
            current2 = current2.Next;
        }
    }
}
