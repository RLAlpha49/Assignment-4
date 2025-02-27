using NLog;
string path = Directory.GetCurrentDirectory() + "//nlog.config";

var logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();

logger.Info("Program started");

string file = "mario.csv";

if (!File.Exists(file))
{
    logger.Error("File does not exist: {File}", file);
}
else
{
    List<MarioCharacter> characters = new List<MarioCharacter>();

    try
    {
        StreamReader sr = new(file);
        // first line contains column headers
        sr.ReadLine();
        while (!sr.EndOfStream)
        {
            string? line = sr.ReadLine();
            if (line is not null)
            {
                string[] characterDetails = line.Split(',');

                ulong id = UInt64.Parse(characterDetails[0]);
                string name = characterDetails[1];
                string? description = characterDetails[2];
                string? species = characterDetails[3];
                string? firstAppearance = characterDetails[4];
                int? yearCreated = int.Parse(characterDetails[5]);

                MarioCharacter character = new MarioCharacter(id, name, description, species, firstAppearance, yearCreated);
                characters.Add(character);
            }
        }
        sr.Close();
    }
    catch (Exception ex)
    {
        logger.Error(ex.Message);
    }


    try
    {
        string? choice;
        do
        {
            // display choices to user
            Console.WriteLine("1) Add Character");
            Console.WriteLine("2) Display All Characters");
            Console.WriteLine("Enter to quit");
            // input selection
            choice = Console.ReadLine();
            logger.Info("User choice: {Choice}", choice);
            if (choice == "1")
            {
                Console.WriteLine("Enter new character name: ");
                string? Name = Console.ReadLine();
                if (!string.IsNullOrEmpty(Name))
                {
                    // check for duplicate name
                    List<string> LowerCaseNames = Names.ConvertAll(n => n.ToLower());
                    if (LowerCaseNames.Contains(Name.ToLower()))
                    {
                        logger.Info($"Duplicate name {Name}");
                    }
                    else
                    {
                        // generate id - use max value in Ids + 1
                        UInt64 Id = Ids.Max() + 1;
                        // input character description
                        Console.WriteLine("Enter description:");
                        string? Description = Console.ReadLine();
                        // input character species
                        Console.WriteLine("Enter species:");
                        string? speciesInput = Console.ReadLine();
                        // input character first appearance
                        Console.WriteLine("Enter first appearance:");
                        string? firstAppearance = Console.ReadLine();
                        // input character year created
                        Console.WriteLine("Enter year created:");
                        string? yearInput = Console.ReadLine();
                        int? yearCreated = null;
                        if (int.TryParse(yearInput, out int tempYear))
                        {
                            yearCreated = tempYear;
                        }
                        // create file from data
                        StreamWriter sw = new(file, true);
                        sw.WriteLine($"{Id},{Name},{Description},{speciesInput},{firstAppearance},{yearCreated}");
                        sw.Close();
                        // add new character details to Lists
                        Ids.Add(Id);
                        Names.Add(Name);
                        Descriptions.Add(Description);
                        Species.Add(speciesInput);
                        FirstAppearances.Add(firstAppearance);
                        YearCreated.Add(yearCreated);
                        // log transaction
                        logger.Info($"Character id {Id} added");
                    }
                }
                else
                {
                    logger.Error("You must enter a name");
                }
            }
            else if (choice == "2")
            {
                for (int i = 0; i < Ids.Count; i++)
                {
                    // display character details
                    Console.WriteLine($"Id: {Ids[i]}");
                    Console.WriteLine($"Name: {Names[i]}");
                    Console.WriteLine($"Description: {Descriptions[i]}");
                    Console.WriteLine($"Species: {Species[i]}");
                    Console.WriteLine($"First Appearance: {FirstAppearances[i]}");
                    Console.WriteLine($"Year Created: {YearCreated[i]}");
                    Console.WriteLine();
                }
            }
            else if (!string.IsNullOrEmpty(choice))
            {
                logger.Warn("Unexpected choice: {Choice}", choice);
                Console.WriteLine("Invalid option. Please choose 1 or 2, or press Enter to quit.");
            }
        } while (choice == "1" || choice == "2");
    }
    catch (Exception ex)
    {
        logger.Error(ex.Message);
    }
}

logger.Info("Program ended");