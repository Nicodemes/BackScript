using System;
using System.Collections.Generic;
namespace Interpreter {
    class OrderedPair{
        public object First { get; set; }
        public object Last { get; set; }
        public OrderedPair(object first, object last) {
            if (first != null)
                lastCount++;
            if (last != null)
                lastCount++;
            this.First = first;
            this.Last = last;

        }
        public OrderedPair(object first):this(first,null){ }
        public OrderedPair() : this(null, null) { }
        
        public void Add(object item) {
            if (item == null)
                return;
            
            noUpdate = false;
            
            if (this.First == null) {
                this.First = item;
                return;
            }
            if (this.Last == null) {
                this.Last = item;
                return;
            }
            this.First = new OrderedPair(this.First, this.Last);
            this.Last = item;
        }
        
        bool noUpdate = true;
        int lastCount = 0;
        public int Count {
            get {
                if (noUpdate)
                    return lastCount;
                int a;
                int b;
                if (this.First is OrderedPair)
                    a = (this.First as OrderedPair).Count;
                else
                    a = 1;
                if (this.Last is OrderedPair)
                    b = (this.First as OrderedPair).Count;
                else
                    return b = 1;
                return b + 1;
            }
        }
        
        public override string ToString() {
            return "{" + this.First + ", " + this.Last + "}";
        }
    }
}
