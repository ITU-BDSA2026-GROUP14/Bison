// Dafny program Filter.dfy compiled into C#
// To recompile, you will need the libraries
//     System.Runtime.Numerics.dll System.Collections.Immutable.dll
// but the 'dotnet' tool in .NET should pick those up automatically.
// Optionally, you may want to include compiler switches like
//     /debug /nowarn:162,164,168,183,219,436,1717,1718

using System;
using System.Numerics;
using System.Collections;
[assembly: DafnyAssembly.DafnySourceAttribute(@"// dafny 4.11.0.0
// Command-line arguments: translate cs Filter.dfy --allow-external-contracts
// Filter.dfy


module {:extern ""Bison.Razor.Models""} Filter {
  function FilterBy(root: Taxon, obs: seq<Observation>): (result: seq<Observation>)
    ensures |result| <= |obs|
    ensures forall o: Observation {:trigger o.getTaxon()} {:trigger o in obs} {:trigger o in result} :: (o in result ==> o in obs) && (o in result ==> o.getTaxon().isSubTaxon(root))
    ensures forall o: Observation {:trigger o in result} {:trigger o.getTaxon()} {:trigger o in obs} :: o in obs && o.getTaxon().isSubTaxon(root) ==> o in result
    decreases root, obs
  {
    if |obs| == 0 then
      []
    else if obs[0].getTaxon().isSubTaxon(root) then
      [obs[0]] + FilterBy(root, obs[1..])
    else
      FilterBy(root, obs[1..])
  }

  class {:extern} Taxon {
    function {:extern} isSubTaxon(ancestor: Taxon): bool
      decreases ancestor
  }

  class {:extern} Observation {
    function {:extern} getTaxon(): Taxon
  }
}

module {:extern ""Bison.Razor.Filtering""} FilterWrapper {
  method FilterObservations(root: Taxon, obs: array<Observation>) returns (result: array<Observation>)
    ensures result[..] == FilterBy(root, obs[..])
    decreases root, obs
  {
    var filtered := FilterBy(root, obs[..]);
    result := new Observation[|filtered|] ((i: int) requires 0 <= i < |filtered| => filtered[i]);
  }

  import opened Filter
}
")]

namespace Dafny {
  internal class ArrayHelpers {
    public static T[] InitNewArray1<T>(T z, BigInteger size0) {
      int s0 = (int)size0;
      T[] a = new T[s0];
      for (int i0 = 0; i0 < s0; i0++) {
        a[i0] = z;
      }
      return a;
    }
  }
} // end of namespace Dafny
internal static class FuncExtensions {
  public static Func<UResult> DowncastClone<TResult, UResult>(this Func<TResult> F, Func<TResult, UResult> ResConv) {
    return () => ResConv(F());
  }
  public static Func<U, UResult> DowncastClone<T, TResult, U, UResult>(this Func<T, TResult> F, Func<U, T> ArgConv, Func<TResult, UResult> ResConv) {
    return arg => ResConv(F(ArgConv(arg)));
  }
  public static Func<U1, U2, UResult> DowncastClone<T1, T2, TResult, U1, U2, UResult>(this Func<T1, T2, TResult> F, Func<U1, T1> ArgConv1, Func<U2, T2> ArgConv2, Func<TResult, UResult> ResConv) {
    return (arg1, arg2) => ResConv(F(ArgConv1(arg1), ArgConv2(arg2)));
  }
}
// end of class FuncExtensions
namespace Bison.Razor.Models {

  public partial class __default {
    public static Dafny.ISequence<Bison.Razor.Models.Observation> FilterBy(Bison.Razor.Models.Taxon root, Dafny.ISequence<Bison.Razor.Models.Observation> obs)
    {
      Dafny.ISequence<Bison.Razor.Models.Observation> _0___accumulator = Dafny.Sequence<Bison.Razor.Models.Observation>.FromElements();
    TAIL_CALL_START: ;
      if ((new BigInteger((obs).Count)).Sign == 0) {
        return Dafny.Sequence<Bison.Razor.Models.Observation>.Concat(_0___accumulator, Dafny.Sequence<Bison.Razor.Models.Observation>.FromElements());
      } else if ((((obs).Select(BigInteger.Zero)).getTaxon()).isSubTaxon(root)) {
        _0___accumulator = Dafny.Sequence<Bison.Razor.Models.Observation>.Concat(_0___accumulator, Dafny.Sequence<Bison.Razor.Models.Observation>.FromElements((obs).Select(BigInteger.Zero)));
        Bison.Razor.Models.Taxon _in0 = root;
        Dafny.ISequence<Bison.Razor.Models.Observation> _in1 = (obs).Drop(BigInteger.One);
        root = _in0;
        obs = _in1;
        goto TAIL_CALL_START;
      } else {
        Bison.Razor.Models.Taxon _in2 = root;
        Dafny.ISequence<Bison.Razor.Models.Observation> _in3 = (obs).Drop(BigInteger.One);
        root = _in2;
        obs = _in3;
        goto TAIL_CALL_START;
      }
    }
  }


} // end of namespace Bison.Razor.Models
namespace Bison.Razor.Filtering {

  public partial class __default {
    public static Bison.Razor.Models.Observation[] FilterObservations(Bison.Razor.Models.Taxon root, Bison.Razor.Models.Observation[] obs)
    {
      Bison.Razor.Models.Observation[] result = new Bison.Razor.Models.Observation[0];
      Dafny.ISequence<Bison.Razor.Models.Observation> _0_filtered;
      _0_filtered = Bison.Razor.Models.__default.FilterBy(root, Dafny.Helpers.SeqFromArray(obs));
      Func<BigInteger, Bison.Razor.Models.Observation> _init0 = Dafny.Helpers.Id<Func<Dafny.ISequence<Bison.Razor.Models.Observation>, Func<BigInteger, Bison.Razor.Models.Observation>>>((_1_filtered) => ((System.Func<BigInteger, Bison.Razor.Models.Observation>)((_2_i) => {
        return (_1_filtered).Select(_2_i);
      })))(_0_filtered);
      Bison.Razor.Models.Observation[] _nw0 = new Bison.Razor.Models.Observation[Dafny.Helpers.ToIntChecked(new BigInteger((_0_filtered).Count), "array size exceeds memory limit")];
      for (var _i0_0 = 0; _i0_0 < new BigInteger(_nw0.Length); _i0_0++) {
        _nw0[(int)(_i0_0)] = _init0(_i0_0);
      }
      result = _nw0;
      return result;
    }
  }
} // end of namespace Bison.Razor.Filtering
namespace _module {

} // end of namespace _module
