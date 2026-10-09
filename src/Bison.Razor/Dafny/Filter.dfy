module {:extern "Bison.Razor.Models"} Filter {
    class {:extern} Taxon {
        function {:extern} isSubTaxon(ancestor: Taxon): bool
    }

    class {:extern} Observation {
        function {:extern} getTaxon(): Taxon
    }

    function FilterBy(root: Taxon, obs: seq<Observation>): (result: seq<Observation>)
        ensures |result| <= |obs|
        ensures forall o :: o in result ==> o in obs && o.getTaxon().isSubTaxon(root)
        ensures forall o :: o in obs && o.getTaxon().isSubTaxon(root) ==> o in result
    {
        if |obs| == 0 then []
        else if obs[0].getTaxon().isSubTaxon(root) then [obs[0]] + FilterBy(root, obs[1..])
        else FilterBy(root, obs[1..])
    }
}

module {:extern "Bison.Razor.Filtering"} FilterWrapper {
  import opened Filter
  method FilterObservations(root: Taxon, obs: array<Observation>) returns (result: array<Observation>)
    ensures result[..] == FilterBy(root, obs[..])
  {
    var filtered := FilterBy(root, obs[..]);
    result := new Observation[|filtered|](i requires 0 <= i < |filtered| => filtered[i]);
  }
}