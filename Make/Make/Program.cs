using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Make
{
    class Program
    {
        static void Main(string[] args)
        {
            var commandLine = new CommandLine();
            try
            {
                commandLine.Parse(args);
                var tanalog = new Model.TanaLog()
                {
                    StoreCode = commandLine.StoreCode,
                    ActivityYear = commandLine.Year,
                    ActivityMonth = commandLine.Month
                };
                tanalog.LoadFrom(commandLine.TanalogDir);
                tanalog.LoadOutsourcing(commandLine.OutsourcingDir);
                tanalog.Make();
                tanalog.SaveAsExcel(commandLine.OutputFile);
            }
            catch (Mono.Options.OptionException e)
            {
                Console.Error.WriteLine(e.Message);
                Console.WriteLine("Try `CommandLineOption --help' for more information.");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }
    }
}
