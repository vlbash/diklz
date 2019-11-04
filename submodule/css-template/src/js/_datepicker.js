
this.formatTextDate = function (el) {
  var text = '';
  if (typeof $(el).data('date') !== 'undefined') {
    text = self.convertDate($(el).data('date'));
    if (typeof $(el).data('date-end') !== 'undefined') {
      text = text + ' - ' + self.convertDate($(el).data('date-end'));
    }
  } else if (typeof $(el).data('date-end') !== 'undefined') {
    text = self.convertDate($(el).data('date-end'))
  }

  $(el).text(text);

}


/// _autocomplete

this.createJsGridDatePicker = function (el, date) {
  $(el).datepicker({
    dateFormat: 'dd.mm.yy',
    firstDay: 1,
  });
  if (date) {
    $(el).datepicker({
      'setDate': self.convertDate(date),
      'defaultDate': self.convertDate(date)
    });
  }
}

this.convertDate = function (date) {
  date = $.trim(date);

  var fullDate = new Date(date),
    year = fullDate.getFullYear(),
    month = fullDate.getMonth() + 1,
    day = fullDate.getDate();

  if (month < 10) {
    month = '0' + month;
  }
  if (day < 10) {
    day = '0' + day;
  }

  if (fullDate == 'Invalid Date') {
    return date;
  }

  return day + '.' + month + '.' + year;
}


this.convertDateForMoment = function (val) {
  var returnedArr = val.split('.'),
    day = returnedArr[0],
    month = returnedArr[1],
    year = returnedArr[2];


  return [year, month, day];
}



this.returnDateToIsoString = function (val) {
  var returnedDate = val.split('.'),
    day = +returnedDate[0],
    month = +returnedDate[1] - 1,
    year = +returnedDate[2];

  return new Date(year, month, day).toISOString();
}


this.normalizeDate = function (date) {

  var reg = /^\s*(3[01]|[12][0-9]|0?[1-9])\.(1[012]|0?[1-9])\.((?:19|20)\d{2})\s*$/g;
  if (reg.test(date)) {
    return date;
  } else {
    return self.convertDate(date);
  }

}
this.checkDatepickerPair = function (el) {

  if ($(el).closest('.dates-group')) {
    var parent,
      childrenInputFrom,
      childrenInputTo,
      hiddenInputFrom,
      hiddenInputTo,
      maxValue,
      minValue;

    parent = $(el).closest('.dates-group');
    hiddenInputFrom = $(parent).find('.data-range-result[name$="_From"]');
    hiddenInputTo = $(parent).find('.data-range-result[name$="_To"]');

    minValue = $(hiddenInputFrom).val();
    maxValue = $(hiddenInputTo).val();

    childrenInputTo = $(parent).find('[id$="_To"]');
    childrenInputFrom = $(parent).find('[id$="_From"]');
    
    $(childrenInputTo).datepicker("option", "minDate", new Date(minValue));
    $(childrenInputFrom).datepicker("option", "maxDate", new Date(maxValue));
  }
}
this.createDatePicker = function (el) {

  if ($(el).val()) {
    $(el).val(self.normalizeDate($(el).val()));
  }

  var elName = $(el).attr('name');
  $(el).attr('name', '');
  $(el).parent().append('<input class = "data-range-result" type = "hidden" name = "' + elName + '"/>')
  $(el).datepicker({
    showButtonPanel: true,
    closeText: 'Закрити',
    prevText: '&#x3c;Поп',
    nextText: 'Наст&#x3e;',
    currentText: 'Сьогодні',
    monthNames: ['Січень', 'Лютий', 'Березень', 'Квітень', 'Травень', 'Червень',
      'Липень', 'Серпень', 'Вересень', 'Жовтень', 'Листопад', 'Грудень'
    ],
    monthNamesShort: ['Січ', 'Лют', 'Бер', 'Квіт', 'Трав', 'Черв',
      'Лип', 'Серп', 'Вер', 'Жовт', 'Лист', 'Груд'
    ],
    dayNames: ['неділя', 'понеділок', 'вівторок', 'среда', 'четвер', 'п\'ятница', 'субота'],
    dayNamesShort: ['нд', 'пн', 'вт', 'ср', 'чт', 'пт', 'сб'],
    dayNamesMin: ['Нд', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб'],
    weekHeader: 'Ти',
    dateFormat: 'dd.mm.yy',
    firstDay: 1,
    isRTL: false,
    showMonthAfterYear: false,
    yearSuffix: '',

    defaultDate: new Date(),

    input: function () {
      // console.log('datepickerValue', $(this).val())
    },

    beforeShow: function (input, inst) {
      datePickerOpen = true; //todo
      if ($(this).hasClass('datepicker-input-select')) {
        $(this).parent().addClass('select-hidden')
      };
      $(this).closest('.input-col-2').addClass('dates-group');
    },

    onClose: function (input, inst) {


      if ($(this).hasClass('datepicker-input-select')) {

        if (self.isElValueEmpty($(this))) {
          $(this).parent().removeClass('select-hidden');
        }
      }

      $.when(self.setDataPickerHiddenValue($(this)))
        .done(self.checkDatepickerPair($(this)))
        .done(self.addEmptyNotEmptyClass($(this)));
      // pair check
      datePickerOpen = false; //todo
    }
  });
  self.setDataPickerHiddenValue(el, true);

  $('.datepicker-input-select').on('focus', function () {
    self.closeAllSelect(body);
  })
}


this.setDataPickerHiddenValue = function (el, onLoad) {

  var parent = $(el).closest('.datepicker-group'),
    dataPickerInput = $(parent).find('.hasDatepicker'),
    selectBefore = $(parent).find('.select-range-before') || false,
    selectAfter = $(parent).find('.select-range-after') || false,
    selectInput = $(parent).find('.select-range-input') || false,
    inputResult,
    inputValue;
  //todo: предустановленные фильтры

  if (onLoad) {
    inputValue = '';
    inputResult = '';
    if ($(dataPickerInput).val()) {
      inputValue = $(dataPickerInput).val();
      if (!self.isStringEmpty(inputValue)) {
        emptyDate = true;
        inputValue = self.returnDateToIsoString(inputValue);
        if (selectBefore.length || selectAfter.length) {
          // if
          inputResult = inputValue + '&' + inputValue;
        } else {
          inputResult = inputValue;
        }
      }
    }
  } else {
    inputValue = $(dataPickerInput).val();

    var currMsg = self.validateDate(dataPickerInput);
    if (currMsg) {

      self.showNotValidMsg(el, currMsg);
      return;
    }

    if (self.isStringEmpty(inputValue)) {
      if (selectBefore.length || selectAfter.length) {
        if (!self.isElValueEmpty(selectBefore) || !self.isElValueEmpty(selectAfter)) {
          inputResult = new Date().toISOString();
        }

        if (selectBefore.length && selectAfter.length) {
          inputResult = self.subtractDate(inputResult, ($(selectBefore).val())) + '&' + self.addDate(inputResult, ($(selectAfter).val()));
        } else if (selectBefore.length) {
          inputResult = self.subtractDate(inputResult, ($(selectBefore).val())) + '&' + inputResult;
        } else if (selectAfter.length) {
          inputResult = inputResult + '&' + self.addDate(inputResult, ($(selectAfter).val()));
        }
      } else if (selectInput.length) {

        if (!self.isElValueEmpty(selectInput)) {
          inputResult = self.subtractDate(new Date().toISOString(), ($(selectInput).val()))
        } else {
          inputResult = '';
        }
      } else {
        inputResult = '';
      }
    } else {

      inputResult = self.returnDateToIsoString(inputValue);

      if (selectBefore.length || selectAfter.length) {
        if (selectBefore.length && selectAfter.length) {
          inputResult = self.subtractDate(inputResult, ($(selectBefore).val())) + '&' + self.addDate(inputResult, ($(selectAfter).val()));
        } else if (selectBefore.length) {
          inputResult = self.subtractDate(inputResult, ($(selectBefore).val())) + '&' + inputResult;
        } else if (selectAfter.length) {
          inputResult = inputResult + '&' + self.addDate(inputResult, ($(selectAfter).val()));
        }
      }
      if (selectInput.length) {
        self.removeAttrSelected(selectInput);
        $(parent).removeClass('select-hidden');
      }
    }

  }
  $(parent).find('.data-range-result').val(inputResult);

}



this.subtractDate = function (date, str) {
  switch (str) {
    case 'day':
      return moment(date).subtract(1, 'days').toISOString();

    case '3day':
      return moment(date).subtract(3, 'days').toISOString();

    case 'week':
      return moment(date).subtract(1, 'week').toISOString();

    case 'month':
      return moment(date).subtract(1, 'month').toISOString();

    case 'quarter':
      return moment(date).subtract(3, 'month').toISOString();

    case 'halfYear':
      return moment(date).subtract(6, 'month').toISOString();

    case 'year':
      return moment(date).subtract(1, 'year').toISOString();

    default:
      return date;
  }
}



this.addDate = function (date, str) {
  switch (str) {
    case 'day':

      return moment(date).add(1, 'days').toISOString();

    case '3day':
      return moment(date).add(3, 'days').toISOString();

    case 'week':
      return moment(date).add(1, 'week').toISOString();

    case 'month':
      return moment(date).add(1, 'month').toISOString();

    case 'quarter':
      return moment(date).add(3, 'month').toISOString();

    case 'halfYear':
      return moment(date).add(6, 'month').toISOString();

    case 'year':
      return moment(date).add(1, 'year').toISOString();

    default:
      return date;
  }
}



this.setDatePickerInputSelectValue = function (el) {

  $(el).parent().find('.hasDatepicker').val('');

  self.setDataPickerHiddenValue($(this));

}


this.createDatePickerInputSelect = function (el) {
  var parent = $(el).closest('.datepicker-group');
  $.when($(el).after(self.createDatepickerSelect(false, false, true)))
    .done(
      function () {
        $(parent).addClass('datepicker-input-select-wrapper');
        self.createDatePicker(el);
        $(parent).find('.select-range-input').on('change', function () {
          $(parent).find('.hasDatepicker').val('');
          self.setDataPickerHiddenValue($(this));
        });
      }
    )
}


//dateRangeBefore


this.createDateRangeBefore = function (el) {
  var parent = $(el).closest('.datepicker-group');

  $.when($(el).before(self.createDatepickerSelect(true, false, false)))
    .done(
      function () {
        self.createDatePicker(el);
        $(parent).addClass('date-range-before-wrapper');
        $(parent).find('.select-range-before').on('change', function () {
          self.setDataPickerHiddenValue($(this));
        });
      }
    )
}

//dateRangeAfter
this.createDateRangeAfter = function (el) {
  var parent = $(el).closest('.datepicker-group');
  $.when($(el).after(self.createDatepickerSelect(false, true, false)))
    .done(
      function () {
        $(parent).addClass('date-range-after-wrapper');
        self.createDatePicker(el);
        $(parent).find('.select-range-after').on('change', function () {
          self.setDataPickerHiddenValue($(this));
        });
      }
    )
}

//dateRangeBeforeAfter

this.createDateRangeBeforeAfter = function (el) {
  var parent = $(el).closest('.datepicker-group');

  $.when(
    $(el).before(self.createDatepickerSelect(true, false, false)).after(self.createDatepickerSelect(false, true, false))

  ).done(
    function () {

      $(parent).addClass('date-range-before-after-wrapper');
      self.createDatePicker(el);
      $(parent).find('.select-range-before').on('change', function () {
        self.setDataPickerHiddenValue($(this));
      });
      $(parent).find('.select-range-after').on('change', function () {
        self.setDataPickerHiddenValue($(this));
      });
    }
  )

}

this.createDatepickerSelect = function (selectBefore, selectAfter, selectInput) {
  var selectClass = 'select';
  if (selectBefore) {
    selectClass += " select-range-before";
  }
  if (selectAfter) {
    selectClass += " select-range-after";
  }
  if (selectInput) {
    selectClass += " select-range-input";
  }
  var select = '<select class = "' + selectClass + '">' +
    '<option value = ""></option>' +
    '<option value = "day">1 день</option>' +
    '<option value = "3day">3 дні</option>' +
    '<option value = "week">тиждень</option>' +
    '<option value = "month">місяц</option>' +
    '<option value = "quarter">3 місяці</option>' +
    '<option value = "halfYear">6 місяців</option>' +
    '<option value = "year">рік</option>' +
    '</select>';
  return select;
}

// if ($('input').is('.date-range-before-after')) {
//       $('.date-range-before-after').each(function () {
//             self.createDateRangeBeforeAfter($(this));
//       })
// }

// if ($('input').is('.date-range-before')) {
//       $('.date-range-before').each(function () {
//             self.createDateRangeBefore($(this));
//       })
// }

// if ($('input').is('.date-range-after')) {
//       $('.date-range-after').each(function () {
//             self.createDateRangeAfter($(this));
//       })
// }

// if ($('input').is('.datepicker')) {
//       $('.datepicker').each(function () {
//             self.createDatePicker($(this));
//       })
// }


this.findFormatDateParagraph = function (el) {
  $(el).find('.format-date').each(function () {
    self.formatTextDate($(this));
  })

}