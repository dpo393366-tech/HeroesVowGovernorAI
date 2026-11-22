using System;
using System.Collections.Generic;
using System.Linq;

public sealed partial class StrategicSupport
{
    sealed class LayoutRevision
    {
        public string Geometry,Catalog,Goals,Buildings,Progress;
        public string Change(LayoutRevision next)=>StructureChange(next)??(Progress!=next.Progress?"construction-or-goal-progress":null);
        public string StructureChange(LayoutRevision next)=>next==null?"missing-state":Geometry!=next.Geometry?"city-expansion-or-plots":
            Catalog!=next.Catalog?"research-or-building-caps":Goals!=next.Goals?"city-goals":Buildings!=next.Buildings?"buildings-changed":null;
    }
    static LayoutRevision ReadLayoutRevision(AFAGFHJONFJ c,MKMOJNAJKJE f)
    {
        // Compare the inputs, not wall time. A regenerated parcel object alone
        // is not a map expansion; use the underlying cells and their geometry.
        var geometry=new List<string>{"scale="+(int)c.MPGFDBPEHBO(true),"relax-adjacency="+(relaxBuildingAdjacency?.Value==true)};
        var buildings=new List<string>();var catalog=new List<string>();var goals=new List<string>();var progress=new List<string>();
        if(c.FGBLEMGGOPO!=null)foreach(var d in c.FGBLEMGGOPO){
            if(d==null||d.KFCOLLDEJNF!=HNDFFIEJNHI.Public)continue;
            geometry.Add($"district={d.OJPECMNINNA}/{d.Pointer}/{d.HFBAJIGHOAF}");
            var cells=new Dictionary<long,FEEDONDCELP>();
            if(d.FAFNFFJAFKH!=null)foreach(var row in d.FAFNFFJAFKH)if(row!=null)foreach(var q in row)if(q!=null)cells[q.Pointer.ToInt64()]=q;
            if(d.HGNFEMJOPPB!=null)foreach(var p in d.HGNFEMJOPPB)foreach(var q in PlotCells(p))if(q!=null)cells[q.Pointer.ToInt64()]=q;
            foreach(var pair in cells){var q=pair.Value;geometry.Add($"cell={d.OJPECMNINNA}/{pair.Key}/{q.BJHLIMBCBNN}/{q.PAHKDHJPFKD}/{q.GHMMBOIKHNC}/{q.HFBAJIGHOAF}/{q.ICGFBPCAJCI}");}
            if(d.AIMLPNBOIIF!=null)foreach(var x in d.AIMLPNBOIIF)if(x?.GEKKLMPLDHJ!=null)
            {
                buildings.Add($"{d.OJPECMNINNA}/{x.OJPECMNINNA}/{x.GEKKLMPLDHJ.OJPECMNINNA}");
                progress.Add($"building={x.OJPECMNINNA}/{Completed(x)}");
            }
        }
        int scale=(int)c.MPGFDBPEHBO(true);
        if(AEHAGJAKNEK.ACIGFKMDOKE!=null)foreach(var kv in AEHAGJAKNEK.ACIGFKMDOKE){var b=kv.Value;
            if(b==null||b.POLCGKHKLHI||b.OCJNCECECMH==FFDDNPOHLHN.Private)continue;
            int limit=b.LJCICFHPMLM!=null&&scale>=0&&scale<b.LJCICFHPMLM.Length?b.LJCICFHPMLM[scale]:-1;
            catalog.Add($"{b.OJPECMNINNA}/{b.PLEHAHBBLGL(f.KCKGONGIMDA)}/{limit}/{b.MDOIJIAONJM}/{EffectSignature(b)}");
        }
        if(c.HFLAGGEFBOO!=null)foreach(var g in c.HFLAGGEFBOO){if(g==null)continue;
            var conditions=new List<string>();if(g.CKIJPJFFFOA!=null)foreach(var q in g.CKIJPJFFFOA)if(q!=null)conditions.Add($"{q.HFBAJIGHOAF}/{q.IJNEIHPHMEE}/{q.KLCDGPDHENB}");
            conditions.Sort(StringComparer.Ordinal);goals.Add($"{g.PIHDGCBCMCP}/{g.DMNMNBGHNKM}/"+string.Join(";",conditions));
            progress.Add($"goal={g.PIHDGCBCMCP}/{g.NHIPAJDALBA(c)}");
        }
        string Join(List<string> x){x.Sort(StringComparer.Ordinal);return string.Join("|",x);}
        return new(){Geometry=Join(geometry),Catalog=Join(catalog),Goals=Join(goals),Buildings=Join(buildings),Progress=Join(progress)};
    }
}
