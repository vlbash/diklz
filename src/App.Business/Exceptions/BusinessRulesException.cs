using System;

namespace App.Business.Exceptions
{
    public class BusinessRulesException: Exception
    {
        private BusinessRulesException(string message) : base(message) { }

        private BusinessRulesException(string message, Exception innerException) : base(message,innerException) { }

        public static BusinessRulesException CreateNew(string message) =>
            new BusinessRulesException(message);

        public static BusinessRulesException CreateNew(string message, Exception innerException) =>
            new BusinessRulesException(message, innerException);

        public static BusinessRulesException CreateEditViolationException(string entityName, string currentStatus) =>
            CreateNew($"{entityName} не може бути редагована в стані {currentStatus}", null);

        public static BusinessRulesException CreateAccessViolationException(string entityName, string currentStatus) =>
            CreateNew($"{entityName} не може бути відкрита в стані {currentStatus}", null);

        public static BusinessRulesException CreateUpdateViolationException(string entityName, string initialStatus, string targetStatus) =>
            CreateNew($"{entityName} не може бути переведена зі стану {initialStatus} в стан {targetStatus}", null);
    }
}
