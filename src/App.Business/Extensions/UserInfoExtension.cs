using System.Linq;
using App.Core.Business.Services;

namespace App.Business.Extensions
{
    public static class UserInfoExtension
    {
        public static string FullName(this UserInfo userInfo) =>
            userInfo.LoginData.FirstOrDefault(x => x.Key == "FullName").Value;

        public static string OrganizationName(this UserInfo userInfo) =>
            userInfo.LoginData.FirstOrDefault(x => x.Key == "OrganizationName").Value;

        public static string Position(this UserInfo userInfo) =>
            userInfo.LoginData.FirstOrDefault(x => x.Key == "Position").Value;

        public static string Name(this UserInfo userInfo) =>
            userInfo.LoginData.FirstOrDefault(x => x.Key == "Name").Value;

        public static string MiddleName(this UserInfo userInfo) =>
            userInfo.LoginData.FirstOrDefault(x => x.Key == "MiddleName").Value;

        public static string LastName(this UserInfo userInfo) =>
            userInfo.LoginData.FirstOrDefault(x => x.Key == "LastName").Value;

        public static string Email(this UserInfo userInfo) =>
            userInfo.LoginData.FirstOrDefault(x => x.Key == "Email").Value;

        public static string Address(this UserInfo userInfo) =>
            userInfo.LoginData.FirstOrDefault(x => x.Key == "Address").Value;

        public static string Phone(this UserInfo userInfo) =>
            userInfo.LoginData.FirstOrDefault(x => x.Key == "Phone").Value;

        public static string EDRPOU(this UserInfo userInfo) =>
            userInfo.LoginData.FirstOrDefault(x => x.Key == "EDRPOU").Value;

        public static string INN(this UserInfo userInfo) =>
            userInfo.LoginData.FirstOrDefault(x => x.Key == "INN").Value;

        public static string SerialNumber(this UserInfo userInfo) =>
            userInfo.LoginData.FirstOrDefault(x => x.Key == "SerialNumber").Value;

        public static string OrganizationId(this UserInfo userInfo) =>
            userInfo.LoginData.FirstOrDefault(x => x.Key == "OrganizationId").Value;
    }
}
