<br/>
<p align="center">

  <h3 align="center">HBH.FiltraCore</h3>

  <p align="center">
    HBH.FiltraCore is a .NET Core library for filtering collections of data. It provides a flexible and easy-to-use API for filtering data in a variety of ways.
    <br/>
    <br/>
    <a href="https://github.com/haithambasim/HBH.FiltraCore"><strong>Explore the docs »</strong></a>
    <br/>
    <br/>
    <a href="https://github.com/haithambasim/HBH.FiltraCore">View Demo</a>
    .
    <a href="https://github.com/haithambasim/HBH.FiltraCore/issues">Report Bug</a>
    .
    <a href="https://github.com/haithambasim/HBH.FiltraCore/issues">Request Feature</a>
  </p>
</p>

![Downloads](https://img.shields.io/github/downloads/haithambasim/HBH.FiltraCore/total) ![Contributors](https://img.shields.io/github/contributors/haithambasim/HBH.FiltraCore?color=dark-green) ![Issues](https://img.shields.io/github/issues/haithambasim/HBH.FiltraCore) ![License](https://img.shields.io/github/license/haithambasim/HBH.FiltraCore) 

## Table Of Contents

* [About the Project](#about-the-project)
* [Built With](#built-with)
* [Getting Started](#getting-started)
* [Installation](#installation)
* [Usage](#usage)
* [Roadmap](#roadmap)
* [Contributing](#contributing)
* [License](#license)
* [Authors](#authors)

## About The Project

HBH.FiltraCore is a .NET Core library for filtering collections of data. It provides a flexible and easy-to-use API for filtering data in a variety of ways.

## Built With



* [Newtonsoft.Json]()
* [System.Linq]()
* [System.Linq.Dynamic.Core]()

## Getting Started


### Installation

To install HBH.FiltraCore, you can use the NuGet Package Manager in Visual Studio or the dotnet CLI.

NuGet Package Manager

```c#
PM> Install-Package HBH.FiltraCore
```

.NET CLI

```c#
dotnet add package HBH.FiltraCore
```

## Usage

## For example you have Employee entity and you want to create an endpoint with dynamic filters


```c#
namespace Test
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Jobtitle { get; set; }
        public decimal Salary { get; set; }
    }
}
```

## first : You have to create a dto for the filter

```c#
using HBH.FiltraCore;

namespace Test
{
    public class AdvanceFilterRequestDto : IFiltraCoreRequestInput
    {
        public string Filters { get; set; }
        public string Term { get; set; }
        public string TermBy { get; set; }
    }
}
```

## second : Create a dto for the input
```c#
namespace Test
{
    public class EmployeeFilterRequestDto : AdvanceFilterRequestDto
    {
      // add your custom filters here 
    }
}
```

## Third : Create Search endpoint

```c#
using HBH.FiltraCore;

namespace Test
{
    [Authorize]
    [ApiController]
    [Route("app/service/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly TestDbContext _testDbContext;
        
        public EmployeeController(TestDbContext testDbContext)
        {
            _testDbContext = testDbContext;
        }
    }

    public async Task<List<Employee>> SearchAsync(EmployeeFilterRequestDto input)
    {
        return await _testDbContext.Employees
                                   // using ApplyAdvanceFilters extension
                                   .ApplyAdvanceFilters(input)
                                   .ToListAsync();
    }
}
```

# Request examples
# using `Term`,`TermBy` filters
* `Term` is the value that you want to search for
* `TermBy` is a comma separated string contains the fields that you want the term filter to by applied on

for example if you want to search using `Term` on two fields `Name`, `Jobtitle` the request must be as bellow:
```Js
await fetch(`https://localhost:5000/app/service/employee?term=haitham&termBy=name,jobtitle`)
```

# using `Filters`
`Filters` is a string contains a 2d array contains all your filters

[
  ['ColumnName', 'FilterType', 'Value']
]

### Available filter types
* Contains: `"$contains"`
* Not Contains: `"$notContains"`
* Equal: `"$eq"`
* Not Equal: `"$ne"`
* Start With: `"$startsWith"`
* EndsWith: `"$endsWith"`
* Less Than: `"$lt"`
* Less Than Or Equal: `"$lte"`
* Greater Than: `"$gt"`
* Greater Than Or Equal: `"$gte"`
* Is Null: `"$null"`
* Is Not Null: `"$notNull"`

### Note: `$null` and `$notNull` doesn't require value field

## Example using `filters`
```Js
await fetch(`https://localhost:5000/app/service/employee?filters=[["salary","$eq","1000"],["jobtitle","$contains","developer"]]`)
```
## Roadmap

See the [open issues](https://github.com/haithambasim/HBH.FiltraCore/issues) for a list of proposed features (and known issues).

## Contributing

Contributions to HBH.FiltraCore are always welcome!

### Creating A Pull Request

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

HBH.FiltraCore is licensed under the MIT License. This means that you are free to use the library for any purpose, including commercial use, and you can modify the source code as needed. However, please be sure to read the terms of the license carefully before using the library.

## Authors

* **Haitham Basim** - *Software developer* - [Haitham Basim](https://github.com/haithambasim) - *built the library*