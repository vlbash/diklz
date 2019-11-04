using System;
using System.Collections.Generic;

namespace App.Data.Models
{
    public class EserviceUserInfo
    {
        public Data Data { get; set; }
    }

    public class Data
    {
        public Profile Profile { get; set; }
    }

    public class Profile
    {
        public int Id { get; set; }
        public Name Name { get; set; }
        public Gender Gender { get; set; }
        public PassportInternal PassportInternal { get; set; }
        public Birthday Birthday { get; set; }
        public List<Address> Addresses { get; set; }
        public List<Phone> Phones { get; set; }
        public List<Email> Emails { get; set; }
        public Itin Itin { get; set; }
    }

    public class Name
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string ShortName { get; set; }
        public bool Verified { get; set; }
    }

    public class Gender
    {
        public string gender { get; set; }
        public bool Verified { get; set; }
    }

    public class PassportInternal
    {
        public string Type { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Series { get; set; }
        public string Number { get; set; }
        public string Birthday { get; set; }
        public DateTime? IssueDate { get; set; }
        public string IssuedBy { get; set; }
        public object IssueId { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool Verified { get; set; }
    }

    public class Itin
    {
        public string itin { get; set; }
        public bool Verified { get; set; }
    }

    public class Birthday
    {
        public DateTime? Date { get; set; }
        public bool Verified { get; set; }
    }

    public class Address
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string Area { get; set; }
        public string District { get; set; }
        public string SettlementName { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Frame { get; set; }
        public string Flat { get; set; }
        public bool Verified { get; set; }
    }

    public class Phone
    {
        public string PhoneNumber { get; set; }
        public bool Confirmed { get; set; }
        public string Type { get; set; }
        public bool Verified { get; set; }
    }

    public class Email
    {
        public string email { get; set; }
        public bool Confirmed { get; set; }
        public string Type { get; set; }
        public bool Verified { get; set; }
    }

   


}
