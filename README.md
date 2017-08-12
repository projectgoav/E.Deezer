# E.Deezer
Unoffical asynchronous Deezer .NET API.

## Usage

Latest Nuget: [v2.3.1](http://nuget.org/packages/e.deezer)

Via Nuget:
```
Package Manager> Install-Package E.Deezer
```

Via Source: 
```
git clone https://github.com/projectgoav/e.deezer
```

Open solution in VS and build it.


Once E.Deezer has been referenced in your new project:
 ```
 //Create a new DeezerSession for your application
 //You'll retrieve a 'Deezer' object which you can browse the API from.
 var Deezer = DeezerSession.CreateNew();
 
 //This performs an async search on Deezer for albums matching "Abba"
 //Mapping to API: search/album?q=Abba&index=0&limit=25
 var search = await Deezer.Search.Albums("Abba");
 
 //You can vary the size and starting position of querys...

 //Will only return UP-TO a maximum of 10 artists matching "Queen"
 //Mapping to API: search/artist?q=Queen&index=0&limit=10
 var small_search = await Deezer.Search.Artists("Queen", 10);

 //This will return for UP-TO a maximum of 15 tracks by Elvis. 
 //These will be offset by 20 places in the results. This is useful for pagination.
 //Mapping to API: search/track/?q=Elivs&index=20&limt=15
 var offset_search = await Deezer.Search.Tracks("Elivs", 20, 15);
 ```

See more in the [Wiki](http://github.com/projectgoav/E.Deezer/wiki)

An example UI demo can be found [here](http://github.com/projectgoav/E.ExploreDeezer)


## Requirements
- Visual Studio 2013 (or later)
- .NET 4.0 (or later)
- [RestSharp](http://restsharp.org/)


##License
(MIT)
Copyright (c) 2017

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
