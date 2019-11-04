var searchFormDataInputs = [{
        labelName: 'П.І.Б, Дата народження, Стать',
        mainInput: true,
        type: 'text',
        query: ['Name', 'Date', 'Sex']
    }, {
        labelName: 'Якась дата',
        order: 6,
        name: 'DateMinus',
        type: 'datepicker'
    }, {
        labelName: 'Дата народження',
        order: 1,
        name: 'Date',
        type: 'datepicker',
        datepickerPast:true,
        datepickerLimit:['todayMax']
    }, {
        labelName: 'Прийоми',
        order: 2,
        name: 'DatePlus',
        type: 'datepicker',
        datepickerPast:true,
        datepickerForwars:true
    }, {
        labelName: 'Стать',
        order: 3,
        name: 'Sex',
        type: 'select',
        dependency: 'Date',
        options: ['option0', 'option1', 'option2', 'option3', 'option4', 'option5'] //url-Controller
    }, {
        labelName: 'Чекбокс',
        order: 4,
        name: 'Check',
        type: 'checkbox',
        style: 'inline'
    }, {
        labelName: 'Чекбокс1',
        order: 5,
        name: 'Check1',
        type: 'checkbox',
        style: 'inline'
    }, {
        labelName: 'Прізвище, ім’я та по батькові клієнта',
        order: 0,
        name: 'Name',
        type: 'text',
        validation: ['validation-only-letters']
    }


// предустановленный фильтр, показать, что загружено с фильтром, предусмотреть отсутствие aside, предусмотреть  хлебные крошки, спрятать меню, прятать фильтры, кнопка "показать спрятанные поля", сохранить фильтр



];
var savedFilters = [{
    filterName: 'name 0',
    filter: [{
            name: 'Name0',
            value: 'Value0'
        },
        {
            name: 'Name1',
            value: 'Value1'
        },
        {
            name: 'Name1',
            value: 'Value1'
        },
        {
            name: 'Name2',
            value: 'Value2'
        }
    ],
    order: 0
},
{
    filterName: 'filter name 1',
    filter: [{
            name: 'Name0',
            value: 'Value0'
        },
        {
            name: 'Name1',
            value: 'Value1'
        },
        {
            name: 'Name1',
            value: 'Value1'
        },
        {
            name: 'Name2',
            value: 'Value2'
        }
    ],

    order: 2
},
{
    filterName: 'filter name 3',
    filter: [{
            name: 'Name0',
            value: 'Value0'
        },
        {
            name: 'Name1',
            value: 'Value1'
        },
        {
            name: 'Name1',
            value: 'Value1'
        },
        {
            name: 'Name2',
            value: 'Value2'
        }
    ],
    order: 1
},{
    filterName: 'name djdhjg0',
    filter: [{
            name: 'Name0',
            value: 'Value0'
        },
        {
            name: 'Name1',
            value: 'Value1'
        },
        {
            name: 'Name1',
            value: 'Value1'
        },
        {
            name: 'Name2',
            value: 'Value2'
        }
    ],
    order: 3
},
{
    filterName: 'dhj cdgfjdfhjfh name 1',
    filter: [{
            name: 'Name0',
            value: 'Value0'
        },
        {
            name: 'Name1',
            value: 'Value1'
        },
        {
            name: 'Name1',
            value: 'Value1'
        },
        {
            name: 'Name2',
            value: 'Value2'
        }
    ],

    order: 4
},
{
    filterName: 'xdfhsh xdfgh cvgjhn',
    filter: [{
            name: 'Name0',
            value: 'Value0'
        },
        {
            name: 'Name1',
            value: 'Value1'
        },
        {
            name: 'Name1',
            value: 'Value1'
        },
        {
            name: 'Name2',
            value: 'Value2'
        }
    ],
    order: 5
},
]

var searchFormDataJson = JSON.stringify(searchFormDataInputs);

var savedFiltersJson = JSON.stringify(savedFilters);

// todo добавить фильтр по умолчаннию


// if(typeof searchFormDataJson!= 'undefined' && searchFormDataJson.length) {
//     mt.createContentSearchForm(searchFormDataJson);
//     if (typeof savedFiltersJson!= 'undefined' && savedFiltersJson.length) {
//         mt.createSavedFilters(savedFiltersJson)
//     }
// }
