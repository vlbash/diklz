using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Core.Business.Services;
using App.Core.Data.Entities.ATU;
using App.Data.DTO.ATU;

namespace App.Business.Services.AtuService
{
    public class AtuAddressService : IAtuAddressService
    {
        private ICommonDataService _dataService { get; }

        public AtuAddressService(ICommonDataService dataService)
        {
            _dataService = dataService;
        }


        public bool SaveAddress(SubjectAddress newSubjectAddress)
        {
            var street = _dataService.GetDto<AtuStreetDTO>(p => p.Id == newSubjectAddress.StreetId).SingleOrDefault();
            if (street == null)
            {
                return false;
            }

            var subjAddress = _dataService.GetEntity<SubjectAddress>(p =>
                p.StreetId == newSubjectAddress.StreetId && p.Building == newSubjectAddress.Building &&
                p.PostIndex == newSubjectAddress.PostIndex).SingleOrDefault();
            if (subjAddress != null)
            {
                newSubjectAddress.Id = subjAddress.Id;
                return true;
            }

            newSubjectAddress.AddressType = street.Caption;
            _dataService.Add(newSubjectAddress);
            _dataService.SaveChanges();
            return true;
        }

        public Guid InsertStreet(Street street)
        {
            var guid = _dataService.Add(street);
            _dataService.SaveChanges();
            return guid;
        }

        public Guid InsertRegion(Region region)
        {
            var guid = _dataService.Add(region);
            _dataService.SaveChanges();
            return guid;
        }

        public Guid InsertCity(City city)
        {
            var guid = _dataService.Add(city);
            _dataService.SaveChanges();
            return guid;
        }

        public Guid InsertCounty(Country country)
        {
            var guid = _dataService.Add(country);
            _dataService.SaveChanges();
            return guid;
        }
    }
}
