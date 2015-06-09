using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;
using BehaviorConverter.BehaviorEngine;
using BehaviorConverter.BehaviorEngine.fabiano;
using Microsoft.CSharp;
using wServer.logic;

namespace BehaviorConverter
{
    class Program
    {
        private const string FILENAME = "BehaviorDb.Mountain.cs";

        static void Main(string[] args)
        {
            try
            {
                const string source = "new Shoot(10, projectileIndex: 0, count: 12, shootAngle: 30, fixedAngle: 25, coolDown: 100000, coolDownOffset: 1000),";

                var argsDic = new ShootBehavior.ShootBehaviorAgrumentList();

                Console.WriteLine("Radius: {0}", BehaviorParserUtils.GetParameter<double>(source, "radius", argsDic));
                Console.WriteLine("Count: {0}", BehaviorParserUtils.GetParameter<int>(source, "count", argsDic));
                Console.WriteLine("Shoot Angle: {0}", BehaviorParserUtils.GetParameter<double?>(source, "shootAngle", argsDic));
                Console.WriteLine("Projectile Index: {0}", BehaviorParserUtils.GetParameter<int>(source, "projectileIndex", argsDic));
                Console.WriteLine("Fixed Angle: {0}", BehaviorParserUtils.GetParameter<double?>(source, "fixedAngle", argsDic));
                Console.WriteLine("Angle Offset: {0}", BehaviorParserUtils.GetParameter<double>(source, "angleOffset", argsDic));
                Console.WriteLine("Default Angle: {0}", BehaviorParserUtils.GetParameter<double?>(source, "defaultAngle", argsDic));
                Console.WriteLine("Predictive: {0}", BehaviorParserUtils.GetParameter<double>(source, "predictive", argsDic));
                Console.WriteLine("Cooldown Offset: {0}", BehaviorParserUtils.GetParameter<int>(source, "coolDownOffset", argsDic));
                Console.WriteLine("Cooldown: {0}", BehaviorParserUtils.GetParameter<int>(source, "coolDown", argsDic));

                //Console.WriteLine($"Checking for compile errors in file \"{FILENAME}\"");
                //Console.WriteLine(CheckForCompileErrors(FILENAME));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
            Console.ReadLine();
        }

        private static string CheckForCompileErrors(string fileName)
        {
            var f = new FileInfo(fileName);
            if (!f.Exists) return "Could not find the file.";

            var codeFixer = new CodeFixer(new StreamReader(f.OpenRead()).ReadToEnd());

            var provider = new CSharpCodeProvider();
            var compilerparams = new CompilerParameters
            {
                ReferencedAssemblies = { "wServer.exe", "db.dll" },
                GenerateExecutable = false,
                GenerateInMemory = true
            };
            var cleanSource = codeFixer.GetFixed();
            var results = provider.CompileAssemblyFromSource(compilerparams, cleanSource);
            if (!results.Errors.HasErrors) return "No Compile errors found.\nSuccess.";
            var errors = new StringBuilder("Compiler Errors :\r\n");
            foreach (CompilerError error in results.Errors)
                errors.AppendFormat("Line {0},{1}\t: {2}\n", error.Line, error.Column, error.ErrorText);

            throw new Exception(errors.ToString());
        }
    }
}
