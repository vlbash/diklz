namespace App.Core.Eq
{
    public enum SlotType
    {
        Appointment = 0,
        Break = 1,
        Vacation = 2,
        Shedule = 3
    }

    public enum SchedulePropertyTypeModel : byte
    {
        Break = 0,
        Vocation = 1
    }

    public enum Repeat : byte
    {
        EveryDay = 0,
        EveryCoupleDay = 1,
        EveryCoupleWeek = 2,
        EveryOddDay = 3,
        EveryOddWeek = 4
    }

    public enum ScheduleType : byte
    {
        RecordOnReception = 0, // Прийом по запису
        LiveQueue = 1, // Жива черга
        WithoutQueue = 2, // Поза чергою
        Reservation = 3, // Бронь
        Сancellation = 4, // Скасування
        Duty = 5 // Чергування
    }
}
