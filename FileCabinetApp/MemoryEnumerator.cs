using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class MemoryEnumerator<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> list;

        public MemoryEnumerator(IEnumerable<T> list)
        {
            this.list = list;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var record in this.list)
            {
                yield return record;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
