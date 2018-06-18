using System.ComponentModel;

namespace PolyclinicProject.Domain.Enum
{
    public enum SortOprions
    {
        [Description("Номер")]
        Number,

        [Description("Номер Desc")]
        Number_desc,

        [Description("Наименование")]
        Name,

        [Description("Наименование Desc")]
        Name_desc,

        [Description("Почта")]
        Email,

        [Description("Почта Desc")]
        Email_desc,

        [Description("Телефон")]
        PhoneNumber,

        [Description("Телефон Desc")]
        PhoneNumber_desc,
    

        [Description("Доктор")]
        Perosnal,

        [Description("Доктор Desc")]
        Perosnal_desc,

   
        [Description("Должность")]
        Position,

        [Description("Должность")]
        Position_desc,

        [Description("Поликлиника")]
        Polyclinic,

        [Description("Поликлиника Desc")]
        Polyclinic_desc,

        [Description("Дата")]
        Date,

        [Description("Дата Desc")]
        Date_desc,

        [Description("Адрес")]
        Address,

        [Description("Адрес Desc")]
        Address_desc,
    }
}