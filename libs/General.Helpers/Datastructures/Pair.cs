using System;

namespace General.Helpers {
  public class Pair<S,T> {
    private S _first;
    private T _second;

    public Pair(S first, T second) {
      _first = first;
      _second = second;
    }

    public S First {
      get { return _first; }
    }

    public T Second {
      get { return _second; }
    }

    public override int GetHashCode() {
      return _first.GetHashCode() ^ _second.GetHashCode();
    }

    public override bool Equals(object obj) {
      Pair<S, T> other = obj as Pair<S, T>;
      if( other == null )
        return false;
      
      return _first.Equals(other._first) && _second.Equals(other._second);
    }
  }

  public class Pair<S>: Pair<S,S> {
    public Pair(S first, S second) : base(first, second) { }
  }
}
