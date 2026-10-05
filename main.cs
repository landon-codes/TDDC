namespace TDDC;

static class Program
{
	public static void Main(string[] args)
	{
		var arguments = ArgumnentParser.ParseArguments(args);

		Console.WriteLine(arguments.StartingDate);
		Console.WriteLine(arguments.DueDate);
		Console.WriteLine(arguments.Pages);
	}
}
