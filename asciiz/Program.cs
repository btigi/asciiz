using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.CommandLine;
using System.CommandLine.Parsing;

var rootCommand = new RootCommand("Image to ascii converter");

var inputFileOption = new Option<string?>(name: "--inputfile", description: "The image file convert.", parseArgument: result =>
{
    var file = result.Tokens.Single().Value;
    if (!File.Exists(file))
    {
        result.ErrorMessage = "File does not exist";
        return null;
    }
    return file;
});
inputFileOption.IsRequired = true;
inputFileOption.AddAlias("-i");
rootCommand.AddOption(inputFileOption);

var outputFileOption = new Option<string?>(name: "--outputfile", description: "The text file to create. The file is overwritten if it exists.");
outputFileOption.IsRequired = false;
outputFileOption.AddAlias("-o");
rootCommand.AddOption(outputFileOption);

var invertOption = new Option<bool>(name: "--invert", getDefaultValue: () => false, description: "Should the greyscale be inverted.");
invertOption.AddAlias("-v");
rootCommand.AddOption(invertOption);

var maxWidthOption = new Option<int>(name: "--maxwidth", getDefaultValue: () => 128, description: "The maximum width of the output text.");
maxWidthOption.AddAlias("-w");
rootCommand.AddOption(maxWidthOption);

var maxHeightOption = new Option<int>(name: "--maxheight", getDefaultValue: () => 80, description: "The maximum height of the output text.");
maxHeightOption.AddAlias("-h");
rootCommand.AddOption(maxHeightOption);

rootCommand.SetHandler(
    (inputFile, outputFile, invert, maxWidth, maxHeight) => { Process(inputFile!, outputFile, invert!, maxWidth!, maxHeight!); },
    inputFileOption, outputFileOption, invertOption, maxWidthOption, maxHeightOption);

return await rootCommand.InvokeAsync(args);

static void Process(string inputFile, string? outputFile, bool invert, int maxWidth, int maxHeight)
{
    using var image = Image.Load<Rgba32>(inputFile);
    var scaledDimensions = ScaleDimensions(image.Width, image.Height, maxWidth, maxHeight);
    image.Mutate(x => x.Resize(scaledDimensions.width, scaledDimensions.height));

    char[] characters = ['@', '#', '8', '&', 'o', ':', '+', '.', ' '];
    if (invert)
    {
        characters = characters.Reverse().ToArray();
    }

    var sb = new StringBuilder();
    for (int y = 0; y < image.Height; y++)
    {
        for (int x = 0; x < image.Width; x++)
        {
            var pixel = image[x, y];
            var averageColour = (pixel.R + pixel.G + pixel.B) / 3.0f;
            var index = (int)(averageColour * (characters.Length - 1) / 255);
            sb.Append(characters[characters.Length - index - 1]);
            sb.Append(characters[characters.Length - index - 1]);
        }
        sb.Append(Environment.NewLine);
    }

    if (!string.IsNullOrEmpty(outputFile))
    {
        File.WriteAllText(outputFile, sb.ToString());
    }
    else
    {
        Console.Write(sb.ToString());
    }
}

static (int width, int height) ScaleDimensions(int width, int height, int maxWidth, int maxHeight)
{
    if (height > maxHeight)
    {
        var scaledWidth = Math.Floor(width * maxHeight / (double)height);
        return ((int)scaledWidth, maxHeight);
    }

    if (width > maxWidth)
    {
        var scaledHeight = Math.Floor(height * maxWidth / (double)width);
        return (maxWidth, (int)scaledHeight);
    }

    return (width, height);
}