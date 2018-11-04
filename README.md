# E.Deezer
Unoffical asynchronous Deezer .NET API.

## Usage
Latest Stable Nuget: [v3.2.0](http://nuget.org/packages/e.deezer)
 - .NET 4.5+
 - PCL Profile 111 (.NET Standard 1.1)

Via Nuget:
```Shell
Package Manager> Install-Package E.Deezer
```

Via Source: 
```Shell
git clone https://github.com/projectgoav/e.deezer
```

Open solution in VS and build it.



## Getting Started

```C#
//Quickstart
using E.Deezer;
using E.Deezer.Api;

var deezer = DeezerSession.CreateNew();

//List genre
var genreList = await deezer.Browse.Genre.GetCommonGenre();

//Get latest chart
var chart = await deezer.Browse.Chart.GetChart();

//Pagination support
var top10 = await deezer.Browse.Chart.GetTrackChart(aCount: 10);
var next10 = await deezer.Browse.Chart.GetTrackChart(aStart: 10, aCount: 10);
var another10 = await deezer.Browse.Chart.GetTrackChart(aStart: 20, aCount: 10);

//Search
var loveAlbums = await deezer.Search.Albums("love");

```

See more in the [Wiki](http://github.com/projectgoav/E.Deezer/wiki)

An example UI demo can be found [here](http://github.com/projectgoav/E.ExploreDeezer)


## Requirements
#### V3+
- Visual Studio 2015+
- .NET 4.5+
- [System.Net.Http](http://nuget.org/packages/system.net.http)
- [Json.NET](http://nuget.org/packages/newtonsoft.json)

#### (V2 or earlier):
- Visual Studio 2013+
- .NET 4 +
- [RestSharp](http://nuget.org/packages/restsharp)



##License
(MIT)
Copyright (c) 2018

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
