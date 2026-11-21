using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Loader;
using HarmonyLib;

class CompileHooks
{
    static string root=Environment.GetEnvironmentVariable("HV_BEPINEX") ?? throw new Exception("Set HV_BEPINEX to the game BepInEx directory");
    static int Main(string[] args)
    {
        AssemblyLoadContext.Default.Resolving+=(context,name)=>{
            foreach(var sub in new[]{"core","interop"}){
                var path=Path.Combine(root,sub,name.Name+".dll");
                if(File.Exists(path))return context.LoadFromAssemblyPath(path);
            }return null;
        };
        try { Run(args); return 0; } catch(Exception e) { Console.WriteLine(e); return 1; }
    }
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    static void Run(string[] args)
    {
        var game=Assembly.LoadFrom(Path.Combine(root,"interop","Assembly-CSharp.dll"));
        var plugin=Assembly.LoadFrom(Path.GetFullPath(args.Length>0?args[0]:"StrategicSupport.dll"));
        var patches=plugin.GetType("StrategicSupport",true);
        var module=AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("ManagedHookTargets"),AssemblyBuilderAccess.Run).DefineDynamicModule("Targets");
        var harmony=new Harmony("strategic.offline.hook.compile");
        int methods=0,callbacks=0;
        foreach(var group in File.ReadAllLines("hook-bindings.txt").Select(l=>l.Split(' ')).GroupBy(p=>p[0]+" "+p[1])){
            var first=group.First();
            var original=AccessTools.Method((game.GetType(first[0],false)??Assembly.LoadFrom(Path.Combine(root,"interop","UnityEngine.UI.dll")).GetType(first[0],true)),first[1]);
            if(original==null)throw new Exception("Missing original "+group.Key);
            var parameters=original.GetParameters();
            // Managed stand-in uses the installed game's exact argument/return types.
            // __instance binding is checked below against the real declaring type;
            // compilation uses a static stand-in (null instance), never native calls.
            var tb=module.DefineType("Target"+methods,TypeAttributes.Public|TypeAttributes.Abstract|TypeAttributes.Sealed);
            var mb=tb.DefineMethod("Invoke",MethodAttributes.Public|MethodAttributes.Static,original.ReturnType,parameters.Select(p=>p.ParameterType).ToArray());
            mb.SetImplementationFlags(MethodImplAttributes.NoInlining);
            for(int i=0;i<parameters.Length;i++)mb.DefineParameter(i+1,parameters[i].Attributes,parameters[i].Name);
            var il=mb.GetILGenerator();
            if(original.ReturnType!=typeof(void)){
                var temp=il.DeclareLocal(original.ReturnType);il.Emit(OpCodes.Ldloca,temp);il.Emit(OpCodes.Initobj,original.ReturnType);il.Emit(OpCodes.Ldloc,temp);
            }
            il.Emit(OpCodes.Ret);
            var target=tb.CreateType().GetMethod("Invoke");
            foreach(var binding in group){
                var patch=patches.GetMethod(binding[3],BindingFlags.Static|BindingFlags.NonPublic)??plugin.GetType("StrategicSupport+ArrivalReportScope")?.GetMethod(binding[3],BindingFlags.Static|BindingFlags.NonPublic);
                if(patch==null)throw new Exception("Missing patch "+binding[3]);
                Console.WriteLine("BIND "+patch.Name+" "+string.Join(",",patch.GetParameters().Select(p=>p.Name)));
                foreach(var p in args.Contains("--skip-contract")?Array.Empty<ParameterInfo>():patch.GetParameters()){
                    var pt=p.ParameterType.IsByRef?p.ParameterType.GetElementType():p.ParameterType;
                    if(p.Name=="__instance"){
                        if(original.IsStatic||!pt.IsAssignableFrom(original.DeclaringType))throw new Exception("Bad instance "+patch.Name);
                    }else if(p.Name=="__result"){
                        if(pt!=original.ReturnType)throw new Exception("Bad result "+patch.Name);
                    }else if(p.Name=="__state"||p.Name=="__exception"){} 
                    else{
                        int index;
                        var source=p.Name.StartsWith("__")&&int.TryParse(p.Name.Substring(2),out index)?parameters.ElementAtOrDefault(index):parameters.FirstOrDefault(a=>a.Name==p.Name);
                        if(source==null)throw new Exception("Unbound parameter "+patch.Name+"::"+p.Name+" on "+group.Key);
                        var st=source.ParameterType.IsByRef?source.ParameterType.GetElementType():source.ParameterType;
                        if(st!=pt)throw new Exception("Argument type mismatch "+patch.Name+"::"+p.Name);
                    }
                }
                var hm=new HarmonyMethod(patch);
                harmony.Patch(target,prefix:binding[2]=="prefix"?hm:null,postfix:binding[2]=="postfix"?hm:null,finalizer:binding[2]=="finalizer"?hm:null);
                callbacks++;
                Console.WriteLine("COMPILED "+group.Key+" "+binding[2]+" "+patch.Name);
            }
            methods++;
        }
        harmony.UnpatchSelf();
        Console.WriteLine($"PASS {callbacks} production callbacks compiled by installed Harmony over {methods} managed stand-ins; real native signatures checked, no game execution.");
    }
}



