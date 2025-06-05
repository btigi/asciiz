## Introduction

asciiz is console application to generate ascii art versions of images..

## Download

Downloads are available as [Github Releases](https://github.com/btigi/asciiz/releases/latest)

## Compiling

To clone and run this application, you'll need [Git](https://git-scm.com) and [.NET](https://dotnet.microsoft.com/) installed on your computer. From your command line:

```
# Clone this repository
$ git clone https://github.com/btigi/asciiz

# Go into the repository
$ cd src

# Build  the app
$ dotnet build
```

## Usage

asciiz is a command line application and should be run from a terminal session. The application accepts several command line parameters:

--inputfile - The image file convert.
--outputfile - The text file to create. The file is overwritten if it exists.
--invert - Should the greyscale be inverted.
--maxwidth - The maximum width of the output text.
--maxheight - The maximum height of the output text.

Example usage
 ```asciiz --inputfile C:\in.jpg --outputfile C:\out.txt```

## Licencing

asciiz is licenced under CC BY-NC-ND 4.0 https://creativecommons.org/licenses/by-nc-nd/4.0/ Full licence details are available in licence.md
