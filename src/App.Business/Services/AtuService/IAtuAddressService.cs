using System;
using App.Core.Data.Entities.ATU;

namespace App.Business.Services.AtuService
{
    public interface IAtuAddressService
    {
        #region Save & Insert

        bool SaveAddress(SubjectAddress newSubjectAddress);
        Guid InsertStreet(Street street);
        Guid InsertRegion(Region region);
        Guid InsertCity(City city);
        Guid InsertCounty(Country country);

        #endregion
    }
}
