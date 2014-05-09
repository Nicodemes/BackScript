using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter.Reserved.Objects {
    class ObjectArray {
        public object[] arr;
        public ObjectArray(OrderedPair pair) {
            this.arr = GetArray(pair, new object[pair.Count],0);
        }
        public ObjectArray(OrderedPair pair, int size) {
            arr = GetArray(pair, new object[size], 0);
        }
        
        public static object[] GetArray(OrderedPair pair, object[] arr , int from) {
            if (from >= arr.Length)
                return arr;
            arr[from] = pair.First;
            from++;
            if (from >= arr.Length)
                return arr;
            if (pair.Last is OrderedPair)
                return GetArray(pair.Last as OrderedPair, arr, from);
            else {
                arr[from] = pair.Last;
                return arr;
            }
        }
        public object this[int index] {
            get { return this.arr[index]; }
            set { this.arr[index] = value; }
        }
    }
}
