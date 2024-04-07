using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Globalization;
using System.Runtime.Intrinsics.X86;
using System.Xml;
using WorkspaceForHomework;

namespace HomeworkWorkspaceNamespace
{
    public class HomeworkWorkspaceClass
    {
        
        public static void Main()
        {
            const string SetCSVFileName = "Introduce el nombre de tu fichero CSV (default: ../../../CSV/ConsumAigua.csv): ";
            const string ErrorMSG = "Error! ese fichero no existe o está vacío :(";
            const string IncorrectQueryMSG = "Parece que el código o nombre es incorrecto.";
            const string WelcomeMSG = "Bienvenido a mi programa de gestión de consumo de agua!";
            const string XMLSaved = "Se ha creado un archivo XML con el contenido del CSV: ";
            const string ChoiceMSG = "Que desea hacer?";
            const string ExitMSG = "0. Salir";
            const string IdentComarques = "1. Buscar comarcas con una población superior a 200.000";
            const string CalConsDomestic = "2. Calcular el consum domèstic mitjà per comarca.";
            const string CalcTopConsumers = "3. Mostrar les comarques amb el consum domèstic per càpita més alt. (Top 3)";
            const string CalcLessConsumers = "4. Mostrar les comarques amb el consum domèstic per càpita més baix. (Top 3)";
            const string FilterComarques = "5. Filtrar les comarques per nom o codi.";
            const string AskCodName = "Introduce el codigo o nombre de la comarca.";
            const string PressAnyKey = "Presiona cualquier tecla para continuar.";
            const string csvDefaultFilepath = "../../../CSV/ConsumAigua.csv";
            const int maxPopulation = 200000;

            List<Consum> fetchedCSVResults = new List<Consum>(); 
            string csvpath = "";
            int choice;
            bool EXIT = false;

            do
            {
                try
                {
                    Console.Write(SetCSVFileName);
                    csvpath = Console.ReadLine();
                    if (csvpath != "")
                        fetchedCSVResults = Utils.ParseConsumCSV(csvpath);
                    else
                        fetchedCSVResults = Utils.ParseConsumCSV(csvDefaultFilepath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ErrorMSG);
                }
            } while (fetchedCSVResults.Count == 0);

            Console.Clear();

            do
            {
                Console.WriteLine(WelcomeMSG);
                Console.WriteLine(XMLSaved + Utils.RecordsToXMLFile(fetchedCSVResults));
                Console.WriteLine(ChoiceMSG);
                Console.WriteLine(ExitMSG);
                Console.WriteLine(IdentComarques);
                Console.WriteLine(CalConsDomestic);
                Console.WriteLine(CalcTopConsumers);
                Console.WriteLine(CalcLessConsumers);
                Console.WriteLine(FilterComarques);
                choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 0:
                        EXIT = true;
                        break;
                    case 1:
                        List<Consum> query = fetchedCSVResults.FindAll(x => x.Poblacio > maxPopulation);
                        foreach (Consum consum in query)
                        {
                            Utils.PrettyPrint(consum.ToString());
                        }
                        break;
                    case 2:
                        List<int> codsComarcas = Utils.GetComarcas(fetchedCSVResults);
                        foreach (int currentCod in codsComarcas)
                        {
                            float personas = 0;
                            float totalCapita = 0;
                            List<Consum> foundConsums = fetchedCSVResults.FindAll(x => x.CodComarca == currentCod);
                            foreach (Consum consum in foundConsums)
                            {
                                totalCapita += consum.ConsumPerCapita;
                                personas++;
                            }
                            float avg = totalCapita / personas;
                            Utils.PrettyPrint($"Codi Comarca -> {currentCod} | Mitja: {avg}");
                        }
                        break;
                    case 3:
                        IEnumerable<Consum> topConsumer = fetchedCSVResults.OrderByDescending(c => c.ConsumPerCapita).Take(3);
                        foreach (Consum current in topConsumer)
                        {
                            Utils.PrettyPrint(current.ToString());
                        }
                        break;
                    case 4:
                        IEnumerable<Consum> lessConsumer = fetchedCSVResults.OrderBy(c => c.ConsumPerCapita).Take(3);
                        foreach (Consum current in lessConsumer)
                        {
                            Utils.PrettyPrint(current.ToString());
                        }
                        break;
                    case 5:
                        Console.WriteLine(AskCodName);
                        string input = Console.ReadLine();
                        var query1 = fetchedCSVResults.FindAll(x => x.CodComarca.ToString() == input);
                        var query2 = fetchedCSVResults.FindAll(x => x.Comarca.ToString() == input);
                        if (query1.Count != 0)
                        {
                            foreach (Consum current in query1)
                            {
                                Utils.PrettyPrint(current.ToString());
                            }
                        }
                        else if (query2.Count != 0)
                        {
                            foreach (Consum current in query2)
                            {
                                Utils.PrettyPrint(current.ToString());
                            }
                        }
                        else
                            Utils.PrettyPrint(IncorrectQueryMSG);
                        break;
                }
                Console.WriteLine(PressAnyKey);
                Console.ReadKey();
                Console.Clear();
            }
            while (!EXIT);
        }
    }
}