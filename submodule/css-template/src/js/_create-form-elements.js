this.selectDataTemplate = function (data, searchPanel) {
  switch (data.type) {

    case 'dataGroup':
      var groupLabel = '',
        dataGroupHtml;

      if (typeof data.labelName !== 'undefined') {
        groupLabel = '<label> ' + data.labelName + ' </label>';
      }

      dataGroupHtml =
        '<div class="input-col-2">' + groupLabel + '<div class="input-col-2-inner">';

      var temp;
      for (var h = 0; h < data.children.length; h++) {
        temp = self.selectDataTemplate(data.children[h], searchPanel);
        dataGroupHtml += temp;
      }

      dataGroupHtml += '</div></div>';
      
      return dataGroupHtml;

    case 'select':
      return self.createSelectTemplate(data);

    case 'datepicker':
      return self.createDatePickerTemplate(data, searchPanel);

    case 'hidden':
      return self.createHiddenInputTemplate(data);

    case 'datepickerBefore':
      return self.createDatePickerBeforeTemplate(data);

    case 'datepickerAfter':
      return self.createDatePickerAfterTemplate(data);

    case 'datepickerBeforeAfter':
      return self.createDatepickerBeforeAfterTemplate(data);

    case 'checkbox':
      return self.createCheckboxTemplate(data);

    case 'checkbox-tree':
      return self.createCheckboxTreeTemplate(data);

    case 'checkbox-tree-child':
      return self.createCheckboxTreeTemplate(data);

    case 'text':
      return self.createTextInputTemplate(data);

    default:
      return self.createTextInputTemplate(data);
  }
}


this.createTextInputTemplate = function (data) {
  var validateAttr = '';
  if (typeof data.validate !== 'undefined') {
    validateAttr = self.createValidateAttr(data.validate);
  }

  return '<div class="input-group">' +
    '<input class = "input" type="text" autocomplete="off" name = "' +
    data.name +
    '"' + validateAttr + '/>' +
    '<span class="input-group-bar">' +
    '</span><label>' +
    data.labelName +
    '</label><span class="js-validation"></span></div>';
}


this.createHiddenInputTemplate = function (data) {
  var val;
  if (typeof data.value !== 'undefined') {
    val = data.value;
  } else {
    val = '';
  }
  return '<input type="hidden" name = "' + data.name + '" value = "' + val + '"/>';
};

this.createCheckboxTemplate = function (data) {
  var value = '';
  if (data.value) {
    value = ' checked';
  }

  return '<div class="checkbox-group">' +
    '<div class="checkbox-group-inner">' +
    '<input id = "' +
    data.id +
    '" class="checkbox" type="checkbox" name="' +
    data.name +
    '"' +
    value +
    '/>' +
    '<label for="' +
    data.id +
    '">' +
    data.labelName +
    '</label>' +
    '</div><span class="checkbox-group-bar"></span></div>';
};

this.createCheckboxTreeTemplate = function (data) {
  var value = '';
  if (data.value) {
    value = ' checked';
  }

  return '<li class="tree-group"><span>'+ data.Code +'</span>' +
    '<input id = "' +
    data.Code +
    '" class="" type="checkbox" name="' +
    data.Name +
    '"' +
    value +
    '/>' +
    '<label for="' +
    data.Code +
    '">' +
    data.Name +
    '</label>' +
    '</li>';
};

this.createDatePickerTemplate = function (data, searchPanel) {

  var searchForm = '';
  if (searchPanel) {
    searchForm = '-input-select'
  }

  var validateAttr = '';
  if (typeof data.validate !== 'undefined') {
    validateAttr = self.createValidateAttr(data.validate);
  };

  return '<div class="datepicker-group input-group">' +
    '<input class = "input datepicker' + searchForm + '" type="text"' + 'id="' + data.name + '"' + 'name="' +
    data.name + '"' + validateAttr + '/>' +
    '<span class="input-group-bar"></span>' +
    '<label for="' + data.name + '"' + '>' +
    data.labelName +
    '</label><span class="js-validation"></span></div>';

};


this.createDatePickerAfterTemplate = function (data) {
  var validateAttr = '';
  if (typeof data.validate !== 'undefined') {
    validateAttr = self.createValidateAttr(data.validate);
  };

  return '<div class="datepicker-group input-group">' +
    '<input class = "input date-range-after" type="text" name="' +
    data.name +
    '"' + validateAttr + '/>' +
    '<span class="input-group-bar"></span>' +
    '<label>' +
    data.labelName +
    '</label><span class="js-validation"></span></div>';
};





this.createDatePickerBeforeTemplate = function (data) {
  var validateAttr = '';
  if (typeof data.validate !== 'undefined') {
    validateAttr = self.createValidateAttr(data.validate);
  };

  return '<div class="datepicker-group input-group">' +
    '<input class = "input date-range-before" type="text" name="' +
    data.name +
    '"' + validateAttr + '/>' +
    '<span class="input-group-bar"></span>' +
    '<label>' +
    data.labelName +
    '</label><span class="js-validation"></span></div>';
};



this.createDatepickerBeforeAfterTemplate = function (data) {
  var validateAttr = '';
  if (typeof data.validate !== 'undefined') {
    validateAttr = self.createValidateAttr(data.validate);
  };
  return '<div class="datepicker-group input-group">' +
    '<input class = "input date-range-before-after" type="text" name="' +
    data.name +
    '"' + validateAttr + '/>' +
    '<span class="input-group-bar"></span>' +
    '<label>' +
    data.labelName +
    '</label><span class="js-validation"></span></div>';
};

this.createSelectTemplate = function (data) {

  var validateAttr = '',
    onchangeUrl = '';

  if (typeof data.validate !== 'undefined') {
    validateAttr = self.createValidateAttr(data.validate);
  }

  if (typeof data.onchangeUrl !== 'undefined' || $.trim(data.onchangeUrl) != '') {
    onchangeUrl = ' onchange-url = "' + data.onchangeUrl + '"';
  }

  var optionsHtml = '<option value = ""></option>',
    currOption;


  if (typeof data.selectOptions === 'string') {
    data.selectOptions = JSON.parse(data.selectOptions);
  }

  if (data.selectOptions && typeof data.selectOptions != typeof undefined) {
    for (var k = 0, maxK = data.selectOptions.length; k < maxK; k++) {
      currOption = data.selectOptions[k];
      optionsHtml += '<option ';
      if (currOption.Disabled) {
        optionsHtml += 'disabled ';
      }
      if (currOption.Selected) {
        optionsHtml += 'selected ';
      }
      optionsHtml += 'value="' + currOption.Value + '">' + currOption.Text + '</option>';
    }
  }


  return '<div class="select-group">' +
    '<select name="' +
    data.name +
    '" class="select"' +
    onchangeUrl +
    validateAttr + '>' +
    optionsHtml +
    '</select><label>' +
    data.labelName +
    '</label><span class="js-validation"></span></div>';
};



this.activateFormControls = function (el) {
  if ($(el).find('.datepicker')) {
    $(el).find('.datepicker').each(function () {
      if ($(this).hasClass('active')) {
        return
      }
      self.addActiveClass($(this));
      self.createDatePicker($(this));
    });
  }
  if ($(el).find('.datepicker-input-select')) {
    $(el).find('.datepicker-input-select').each(function () {
      if ($(this).hasClass('active')) {
        return
      }
      self.addActiveClass($(this));
      self.createDatePickerInputSelect($(this));
    });
  }
  if ($(el).find('.date-range-before')) {
    $(el).find('.date-range-before').each(function () {
      if ($(this).hasClass('active')) {
        return
      }
      self.addActiveClass($(this));
      self.createDateRangeBefore($(this));
    });
  }
  if ($(el).find('.date-range-before-after')) {
    $(el).find('.date-range-before-after').each(function () {
      if ($(this).hasClass('active')) {
        return
      }
      self.addActiveClass($(this));
      self.createDateRangeBeforeAfter($(this));
    });
  }
  if ($(el).find('.date-range-after')) {
    $(el).find('.date-range-after').each(function () {
      if ($(this).hasClass('active')) {
        return
      }
      self.addActiveClass($(this));
      self.createDateRangeAfter($(this));
    });
  }
  if ($(el).find('.select')) {
    $(el).find('.select').each(function () {
      if ($(this).hasClass('active')) {
        return
      }
      self.addActiveClass($(this));
      self.selectWrap($(this));
    });
  }
  if ($(el).find('.input')) {
    $(el).find('.input').each(function () {
      if ($(this).hasClass('active')) {
        return
      }
      self.addActiveClass($(this));
      self.initInputAfterLoad($(this));
    });
  }
};