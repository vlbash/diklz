using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using App.Core.Base;
using App.Core.Business.Services;
using App.Core.Data.Entities.ATU;
using App.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

/*
https://data.gov.ua/dataset/dc081fb0-f504-4696-916c-a5b24312ab6e/resource/296adb7a-476a-40c8-9de6-211327cb3aa1
Code structure (KOATUU):

Code consists from 10 chars:

Char number     Description
1,2             Level 1 code
3               Level 2 type
4,5             Level 2 code
6               Level 3 type
7,8             Level 3 code
9,10            Level 4 code
 
Level 2 type (char 3)
1       міста обласного значення
2       райони Автономної Республіки Крим, області
3       райони міст, що мають спеціальний статус

Level 3 type (char 6)
1	міста районного значення
2	is unused
3	райони в містах обласного значення
4	селища міського типу, що входять до складу міськради
5	селища міського типу, що входять до складу райради
6	селища міського типу, що входять до складу райради в місті
7	міста, що входять до складу міськради
8	сільради, що входять до складу райради
9	сільради, села, що входять до складу райради міста, міськради 

*/

namespace App.Business.Services.AtuService
{
    public class AtuImportService: IAtuImportService
    {
        private ICommonDataService _dataService { get; }
        private IHostingEnvironment _hostingEnvironment { get; }

        public AtuImportService(ICommonDataService dataSevice, IHostingEnvironment hostingEnvironment)
        {
            _dataService = dataSevice;
            _hostingEnvironment = hostingEnvironment;
        }

        // Running time about 15 minutes.
        public void ImportData()
        {
            if (_dataService.GetEntity<Region>(take: 1).Count() > 0)
            {
                return;
            }

            var filePath = Path.GetFullPath(_hostingEnvironment.WebRootPath + "\\custom_js\\koatuu.json");
            using (var reader = new StreamReader(filePath))
            {
                var json = reader.ReadToEnd();
                var model = JsonConvert.DeserializeObject<AtuImport.ATU>(json);

                var country = new Country
                {
                    Name = "Україна",
                    Code = "UKR",
                };
                _addAndSave(country);

                foreach (var level1 in model.level1)
                {
                    level1.name = level1.name.Split("/")[0].ToLower();
                    level1.name = level1.code.StartsWith("01") // 01 КРИМ
                                    ? Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(level1.name)
                                    : char.ToUpper(level1.name[0]) + level1.name.Substring(1);
                    var region = new Region
                    {
                        Id = Guid.Empty,
                        Name = level1.name,
                        Code = level1.code,
                        CountryId = country.Id
                    };
                    _addAndSave(region);

                    // міста обласного значення
                    foreach (var level2 in level1.level2.Where(p => new Regex(@"\d{2}1\d[^00]{1}\d{5}").IsMatch(p.code)))
                    {
                        level2.name = level2.name.ToLower();
                        level2.name = char.ToUpper(level2.name[0]) + level2.name.Substring(1);
                        var city = new City
                        {
                            Id = Guid.Empty,
                            Name = level2.name,
                            Code = level2.code,
                            RegionId = region.Id,
                            TypeEnum = "CitiesOfRegionalSubordination"
                        };
                        _addAndSave(city);

                        foreach (var level3 in level2.level3)
                        {
                            if (!_addCity(level3.name, level3.code, level3.type, region.Id))
                            {
                                foreach (var level4 in level3.level4)
                                {
                                    if (!_addCity(level4.name, level4.code, level4.type, region.Id))
                                    {
                                        continue;
                                    }
                                }
                                continue;
                            }

                            foreach (var level4 in level3.level4)
                            {
                                if (!_addCity(level4.name, level4.code, level4.type, region.Id))
                                {
                                    continue;
                                }
                            }
                        }
                        _dataService.SaveChanges();
                    }

                    // райони Автономної Республіки Крим, області
                    foreach (var level2 in level1.level2.Where(p => new Regex(@"\d{2}2\d[^00]{1}\d{5}").IsMatch(p.code)))
                    {
                        level2.name = level2.name.Split("/")[0].ToLower();
                        level2.name = char.ToUpper(level2.name[0]) + level2.name.Substring(1);
                        var district = new Region
                        {
                            Id = Guid.Empty,
                            Name = level2.name,
                            Code = level2.code,
                            ParentId = region.Id,
                            CountryId = country.Id
                        };
                        _addAndSave(district);

                        foreach (var level3 in level2.level3)
                        {
                            if (!_addCity(level3.name, level3.code, level3.type, region.Id))
                            {
                                foreach (var level4 in level3.level4)
                                {
                                    if (!_addCity(level4.name, level4.code, level4.type, region.Id))
                                    {
                                        continue;
                                    }
                                }
                                continue;
                            }

                            foreach (var level4 in level3.level4)
                            {
                                if (!_addCity(level4.name, level4.code, level4.type, region.Id))
                                {
                                    continue;
                                }
                            }
                        }
                        _dataService.SaveChanges();
                    }


                    // Районні центри
                    if (!level1.code.StartsWith("01"))
                    {
                        foreach (var level2 in level1.level2.Where(p => new Regex(@"\d{2}2\d[00]{1}\d{5}").IsMatch(p.code))
                            .Where(p => p.name.Contains("/")))
                        {
                            level2.name = level2.name.Split("/")[0].ToLower();
                            level2.name = char.ToUpper(level2.name[0]) + level2.name.Substring(1);
                            var district = new Region
                            {
                                Id = Guid.Empty,
                                Name = level2.name,
                                Code = level2.code,
                                ParentId = region.Id,
                                CountryId = Guid.Parse("4c9c84e8-c363-4ef5-9bda-a7b04ec65fb6")
                            };
                            _addAndSave(district);

                            foreach (var level3 in level2.level3)
                            {
                                if (!_addCity(level3.name, level3.code, level3.type, region.Id))
                                {
                                    foreach (var level4 in level3.level4)
                                    {
                                        if (!_addCity(level4.name, level4.code, level4.type, region.Id))
                                        {
                                            continue;
                                        }
                                    }
                                    continue;
                                }

                                foreach (var level4 in level3.level4)
                                {
                                    if (!_addCity(level4.name, level4.code, level4.type, region.Id))
                                    {
                                        continue;
                                    }
                                }
                            }
                            _dataService.SaveChanges();
                        }
                    }

                }
            }
        }

        // Level 3 and level 4
        private bool _addCity(string name, string code, string type, Guid regionId)
        {
            name = name.ToLower();
            City city;
            switch (type)
            {
                case "Т":
                case "М":
                case "С":
                case "Щ":
                    {
                        city = new City
                        {
                            Id = Guid.Empty,
                            Name = char.ToUpper(name[0]) + name.Substring(1),
                            Code = code,
                            RegionId = regionId,
                        };
                        switch (type)
                        {
                            case "Т": city.TypeEnum = "UrbanTypeVillages"; break;
                            case "М": city.TypeEnum = "CitiesOfRegionalSubordination"; break;
                            case "С": city.TypeEnum = "Village"; break;
                            case "Щ": city.TypeEnum = "Hamlet"; break;
                        }
                        break;
                    }
                default: return false;
            }
            _dataService.Add(city);
            return true;
        }

        private void _addAndSave(IEntity model)
        {
            _dataService.Add(model);
            _dataService.SaveChanges();
        }
    }
}
