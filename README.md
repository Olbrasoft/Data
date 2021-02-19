# Data [![AppVeyor build status](https://img.shields.io/appveyor/build/Olbrasoft/data/master.svg)](https://ci.appveyor.com/project/Olbrasoft/data)
 
## <img align="left" alt="Olbrasoft.Data.Paging" style="float: left;  display: block; margin: 0px 0px 20px;" src="https://raw.githubusercontent.com/Olbrasoft/Data/master/olbrasoft-data-paging.png" width="45" height="45"/>   Olbrasoft.Data.Paging  [![NuGet](https://img.shields.io/nuget/vpre/Olbrasoft.Data.Paging.svg)](https://www.nuget.org/packages/Olbrasoft.Data.Paging/)

  [![NuGet](https://img.shields.io/nuget/vpre/Olbrasoft.Data.Sorting.svg)](https://www.nuget.org/packages/Olbrasoft.Data.Sorting/) Olbrasoft.Data.Sorting <img alt="Olbrasoft.Data.Paging" style="float: left;  display: block; margin: 0px 0px 20px;" src="https://raw.githubusercontent.com/Olbrasoft/Data/master/olbrasoft-data-sorting.png" width="45" height="45"/> 

  [![NuGet](https://img.shields.io/nuget/vpre/Olbrasoft.Data.Sorting.Extensions.svg)](https://www.nuget.org/packages/Olbrasoft.Data.Sorting.Extensions/) Olbrasoft.Data.Sorting.Extensions

  [![NuGet](https://img.shields.io/nuget/vpre/Olbrasoft.Data.Cqrs.svg)](https://www.nuget.org/packages/Olbrasoft.Data.Cqrs/) Olbrasoft.Data.Cqrs <img alt="Olbrasoft.Data.Paging" style="float: left;  display: block; margin: 0px 0px 20px;" src="https://raw.githubusercontent.com/Olbrasoft/Data/master/olbrasoft-data-cqrs.png" width="45" height="45"/> 
    
  [![NuGet](https://img.shields.io/nuget/vpre/Olbrasoft.Data.Cqrs.EntityFrameworkCore.svg)](https://www.nuget.org/packages/Olbrasoft.Data.Cqrs.EntityFrameworkCore/) Olbrasoft.Data.Cqrs.EntityFrameworkCore
  
  ## <img align="left" alt="Olbrasoft.Data.Paging" style="float: left;  display: block; margin: 0px 0px 20px;" src="https://raw.githubusercontent.com/Olbrasoft/Data/master/olbrasoft-data-x-pagedList.png" width="45" height="45"/> Olbrasoft.Data.Paging.X.PagedList   [![NuGet](https://img.shields.io/nuget/vpre/Olbrasoft.Data.Paging.X.PagedList.svg)](https://www.nuget.org/packages/Olbrasoft.Data.Paging.X.PagedList/)
 ### [![NuGet](https://img.shields.io/nuget/vpre/Olbrasoft.Data.Paging.X.PagedList.svg)](https://www.nuget.org/packages/Olbrasoft.Data.Paging.X.PagedList/)  Olbrasoft.Data.Paging.X.PagedList


  ## Olbrasoft.Data.Paging.X.PagedList.AspNetCore.Mvc

Olbrasoft.Data.Paging.X.PagedList.AspNetCore.Mvc

Inspiration repository https://github.com/hieudole/PagedList.Core.Mvc

Reason asimilation repository are dependent on https://github.com/troygoode/PagedList is obsolete .

## Installation

1. Install-Package Olbrasoft.Data.Paging.X.PagedList.AspNetCore.Mvc

2. Edit `_ViewImports.cshtml`

```diff
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
+ @addTagHelper *, Olbrasoft.Data.Paging.X.PagedList.AspNetCore.Mvc
```

## Usage
```html
<pager class="pager-container" list="@Model.SearchResult.SearchHits" options="@PagedListRenderOptions.Bootstrap4" asp-action="Index" asp-controller="Search" asp-route-query="@Model.SearchResult.SearchQuery" />
```
## Sample

![Sample](./assets/SearchResult.jpg?raw=true)

![Olbrasoft Paging Icon](./olbrasoft-x-paged-list.png)