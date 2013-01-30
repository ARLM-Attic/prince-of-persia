using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrinceOfPersia
{
    public class SyncList 
    { 
        object syncRoot = new object(); 
        List<Tile> list = new List<Tile>();
    
        public void Add(Tile item) 
        { 
            lock (syncRoot) list.Add(item);
        }

        public int Count
        {
            get { lock (syncRoot) return list.Count; }
        }

        public IteratorWrapper GetIteratorWrapper()
        {
            return new IteratorWrapper(this);
        }

    }

    public class IteratorWrapper : IDisposable, IEnumerable<T>
    {
        bool disposed;
        List<Tile> c;
        
        public IteratorWrapper(List<Tile> c) 
        { 
            this.c = c; 
            Monitor.Enter(c.syncRoot); 
        }
        
        public void Dispose() 
        { if (!disposed) Monitor.Exit(c.syncRoot); disposed = true; }

    public IEnumerator<T> GetEnumerator()
    {
        return c.list.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}


    }
}
