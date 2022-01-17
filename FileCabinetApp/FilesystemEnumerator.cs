using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FilesystemEnumerator<T> : IEnumerable<T>
    {
        private readonly List<long> list;
        private readonly Func<T> getRecord;
        private readonly Action<long> setReaderPosition;

        public FilesystemEnumerator(List<long> list, Func<T> getRecord, Action<long> setReaderPosition)
        {
            this.list = list;
            this.getRecord = getRecord;
            this.setReaderPosition = setReaderPosition;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var position in this.list)
            {
                this.setReaderPosition(position);
                yield return this.getRecord();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
