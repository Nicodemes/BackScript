using System;
using System.Collections.Generic;

namespace Interaptor {
    class Id {
        public LinkedList<string> path;
        public Id(string head) {
            path = new LinkedList<string>();
            path.AddFirst(head);
        }
        public void AddPath(string p) {
            path.AddLast(p);
        }
        public int Length { get { return path.Count; } }
        public override string ToString() {
            string toReturn = "";
            LinkedListNode<string> next=path.First;
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
