using System;
using System.Collections.Generic;
using System.Text;

namespace General.Helpers {
  public class Triple<S,T,U> {
    private S _first;
    private T _second;
    private U _third;


    public Triple(S first, T second, U third) {
      _first = first;
      _second = second;
      _third = third;
    }

    public S First {
      get { return _first; }
    }

    public T Second {
      get { return _second; }
    }
    public U Third {
      get { return _third; }
    }

    public override int GetHashCode() {
      return _first.GetHashCode() ^ _second.GetHashCode();
    }

    public override bool Equals(object obj) {
      Triple<S, T, U> other = obj as Triple<S, T, U>;
      if ( other == null )
        return false;

      return _first.Equals(other._first) && _second.Equals(other._second) && _third.Equals(other._third);
    }
  }

  public class Triple<S>: Triple<S, S, S> {
    public Triple(S first, S second, S third): base(first,second,third) { }
  }
}
