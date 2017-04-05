# OfferingSolutions Datashaper

[![NuGet Downloads](https://img.shields.io/nuget/dt/OfferingSolutions.DataShaper.svg)](https://www.nuget.org/packages/OfferingSolutions.DataShaper/) [![NuGet Version](https://img.shields.io/nuget/v/OfferingSolutions.DataShaper.svg)](https://www.nuget.org/packages/OfferingSolutions.DataShaper/)

Gives you the possibility to strip the data you want to send based on the query of the client using DTOs.

## Nuget
The OfferingSolutions.Datashaper is available on Nuget:

http://www.nuget.org/packages/OfferingSolutions.DataShaper/

## Demos
http://www.fabian-gosebrink.de/Projects/Datashaper
https://github.com/FabianGosebrink/OfferingSolutions-DatashaperDemo

<pre>
public IHttpActionResult Get(string fields = null)
{
    try
    {
        //...
        
        List<string> listOfFields = new List<string>();
        if (fields != null)
        {
            listOfFields = fields.Split(',').ToList();
        }

        IQueryable<MyItems> myItems = _repository.GetMyItems();

        //...
        
        var result = myItems
            .ToList()
            .Select(x => Datashaper.CreateDataShapedObject(x, listOfFields));

        return Ok(result);
    }
    catch (Exception)
    {
         return InternalServerError();
    }
}</pre>


![datashaping-picture1](http://fabian-gosebrink.de/img/projects/datashaper_2.PNG)
![datashaping-picture2](http://fabian-gosebrink.de/img/projects/datashaper_3.PNG)
![datashaping-picture3](http://fabian-gosebrink.de/img/projects/datashaper_4.PNG)

Now you can shape your data based on the fields you send with your request.

<pre>
GET /api/test?fields=Id,Title,Date<br/>
GET /api/test?fields=Id,Title,Date,ChildClasses.Description,ChildClasses.Id
</pre>

Have fun!
