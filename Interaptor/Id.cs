using System;
using System.Collections.Generic;

namespace Interaptor {
    class Id {
        public LinkedList<string> Path { get; set; }
        public string Head { get { return Path.First.Value; } }
        public Id(string head) {
            Path = new LinkedList<string>();
            Path.AddFirst(head);
        }
        public void AddPath(string p) {
            Path.AddLast(p);
        }
        public int Length { get { return Path.Count; } }
        public override string ToString() {
            string toReturn = "";
            LinkedListNode<string> next=Path.First;
            if (next != null) {
                toReturn += next.Value;
                next = next.Next;
            }
            while(next!=null){
                toReturn += '.';
                toReturn += next.Value.ToString();
                next = next.Next;
            }
            return toReturn;
        }
    }
}
