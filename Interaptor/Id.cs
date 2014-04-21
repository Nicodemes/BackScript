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

    }
}
