using System.ComponentModel;

namespace PolyclinicProject.Domain.Enum
{
    public enum Sex
    {
        [Description("Мужской")]
        Мужской = 1,

        [Description("Женский")]
        Женский = 2,
    }

    public enum MonthEnum
    {
        [Description("Январь")]
        Январь = 1,
        Февраль,
        Март,
        Апрель,
        Май,
        Июнь,
        Июль,
        Август,
        Сентябрь,
        Октябрь,
        Ноябрь,
        Декабрь
    }

    public enum DayOfWeekEnum
    {
        Понедельник = 1,
        Вторник,
        Среда,
        Четверг,
        Пятница,
        Суббота,
        Воскресенье = 0
    }
}