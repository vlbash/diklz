this.createValidateAttr = function (data) {
  var validateAttr = " validate "
  for (var i = 0; i < data.length; i++) {
    validateAttr += "validate-" + data[i] + ' ';
  }
  return validateAttr;
}

this.returnValidateMsg = function (property) {
  switch (property) {
    case 'required':
      return "Обов'язкове поле";

    case 'not-empty':
      return "Обов'язкове поле";

    case 'number':
      return 'Значення має бути числом';

    case 'phone':
      return 'Невірно введений номер';

    case 'email':
      return 'Невірний формат email';

    case 'equal-to':
      return 'Значення не співпадають';

    case 'length-max':
      return 'Максимальна кількість знаків ';

    case 'length-min':
      return 'Мінімальна кількість знаків ';

    case 'range':
      return 'Значення має бути у діапазоні від ';

    case 'url':
      return 'Невірний формат url-адреси';

    case 'creditcard':
      return 'Невірний номер картки';

    case 'pattern':
      return 'Недопустиме значення';

    case 'date':
      return 'Невірний формат дати';

  }

}

this.findValidateForm = function (el) {
  if (self.returnTagName(el) == 'form') {
    submitFunction(el);
  } else {
    $(el).find('form').each(function () {
      submitFunction($(this));
    })
  }
  $(el).find('[data-val-required]:not([type="checkbox"])').each(function () {
    currEl = $(this);
    currEl.siblings('label').append('*');
  })

  function submitFunction(form) {
  var submitBtn = $(form).find('input[type = "submit"]') || $(form).find('button[type = "submit"]') || $(form).find('.submit-form-button');
    $(submitBtn).off().on('click', function (e) {
      if (!$(form).hasClass('no-validate')) {
        self.checkFormBeforeSubmit(e, form);
      } else {
        $(form).submit();
      }
    })
  }
}

this.checkFormBeforeSubmit = function(e, form){
  e.preventDefault();
  if (self.validateForm(form)) {
    $(form).submit();
  } else{
    $('html, body').animate({
      scrollTop: ($('.asp-validation.active').first().offset().top-120)
    }, 700);
  }
}


this.showNotValidMsg = function (el, msg) {
  var elTagName = self.returnTagName(el);
  var elParent;
  if (elTagName == 'input') {
    elParent = $(el).closest('.input-group');
  }
  if (elTagName == 'select') {
    elParent = $(el).closest('.select-group');
  }

  var jsValidationMsgEl = $(elParent).find('.asp-validation');

  if (!jsValidationMsgEl.length) {
    return;
  }

  $(jsValidationMsgEl).text(msg);

  self.addActiveClass(jsValidationMsgEl);

  $(elParent).on('mouseover', destroyNotValidMsg);
  $(elParent).find('input').each(function () {
    $(this).bind('focus', destroyNotValidMsg);
  });
  //$(body).on('click', destroyNotValidMsg);

  function destroyNotValidMsg() {
    self.removeActiveClass(jsValidationMsgEl);
    setTimeout(function () {
      $(elParent).find('.asp-validation').text('');

    }, timeOutInterval)
    $(elParent).off('mouseover', destroyNotValidMsg);
    $(elParent).find('input').each(function () {
      $(this).unbind('focus', destroyNotValidMsg)
    });

  }
}

this.validateForm = function (form) {
  if (!form || !form.length) {
    return;
  }
  var validToken = true,
    currMsg,
    currEl;

  $(form).find('[data-val-required]').each(function () {
    currEl = $(this);
    var testMsg = $(this).attr('data-val-required');
    //console.log('data-val-required =',testMsg);
    currMsg = self.validateRequired(currEl);
    if (currMsg) {
      validToken = false;
      self.showNotValidMsg(currEl, currMsg);
    }
  })

  $(form).find('[no-validate]').each(function () {
    currEl = $(this);
    validToken = true;
  })

  $(form).find('[validate-not-empty]').each(function () {
    currEl = $(this);
    currMsg = self.validateNotEmpty(currEl);
    if (currMsg) {
      validToken = false;
      self.showNotValidMsg(currEl, currMsg);
    }
  })

  $(form).find('[validate-number]').each(function () {
    currEl = $(this);
    currMsg = self.validateNumber(currEl);
    if (currMsg) {
      validToken = false;
      self.showNotValidMsg(currEl, currMsg);
    }
  })

  $(form).find('[data-val-phone]').each(function () {
    currEl = $(this);
    var testMsg = $(this).attr('data-val-phone');
    //console.log('data-val-phone =',testMsg);
    currMsg = self.validatePhone(currEl);
    if (currMsg) {
      validToken = false;
      self.showNotValidMsg(currEl, currMsg);
    }
  })

  $(form).find('[data-val-email]').each(function () {
    currEl = $(this);
    currMsg = self.validateEmail(currEl);
    if (currMsg) {
      validToken = false;
      self.showNotValidMsg(currEl, currMsg);
    }
  })

  $(form).find('[data-val-equalto-other]').each(function () {
    var currEl,
      currElVal,
      currElAttr,
      equalElName,
      equalElId,
      equalElVal;

    currEl = $(this);
    currElVal = currEl.val();
    currElAttr = currEl.attr('data-val-equalto-other');

    equalElName = currElAttr.replace(/[^\w\s]/gi, '');
    equalElId = '#' + equalElName;
    equalElVal = $(equalElId).val();
    currMsg = self.validateEqualTo(currElVal, equalElVal);
    if (currMsg) {
      validToken = false;
      self.showNotValidMsg(currEl, currMsg);
      self.showNotValidMsg($(equalElId), currMsg);
    }
  })

  $(form).find('[data-val-length-max]').each(function () {
    var currEl,
      currElVal,
      currElLength,
      currElLengthMax;

    currEl = $(this);
    var testMsg = $(this).attr('data-val-length');
    //$(this).addClass('test')
    //console.log('data-val-length-max = ',testMsg);
    currElVal = currEl.val();
    currElLength = currElVal.length;

    currElLengthMax = currEl.attr('data-val-length-max');
    currMsg = self.validateLengthMax(currElLength, currElLengthMax);
    if (currMsg) {
      validToken = false;
      currMsg += currElLengthMax;
      self.showNotValidMsg(currEl, currMsg);
    }
  })

  $(form).find('[data-val-length-min]').each(function () {
    var currEl,
      currElVal,
      currElLength,
      currElLengthMin;

    currEl = $(this);
    var testMsg = $(this).attr('data-val-length');
    //console.log('data-val-length-min = ',testMsg);
    currElVal = currEl.val();
    currElLength = currElVal.length;

    currElLengthMin = currEl.attr('data-val-length-min');
    currMsg = self.validateLengthMin(currElLength, currElLengthMin);

    if (currMsg) {
      validToken = false;
      currMsg += currElLengthMin;
      self.showNotValidMsg(currEl, currMsg);
    }
  })

  $(form).find('[data-val-range]').each(function () {
    var currEl,
      currElRangeMax,
      currElRangeMin;

    currEl = $(this);
    var testMsg = $(this).attr('data-val-range');
    //console.log('data-val-range =', testMsg);
    currElRangeMax = +currEl.attr('data-val-range-max');
    currElRangeMin = +currEl.attr('data-val-range-min');

    currMsg = self.validateRange(currEl, currElRangeMin, currElRangeMax);

    if (currMsg) {
      validToken = false;
      currMsg += currElRangeMin + ' до ' + currElRangeMax;
      self.showNotValidMsg(currEl, currMsg);
    }
  })

  $(form).find('[data-val-url]').each(function () {
    currEl = $(this);
    currMsg = self.validateUrl(currEl);
    if (currMsg) {
      validToken = false;
      self.showNotValidMsg(currEl, currMsg);
    }
  })

  $(form).find('[data-val-creditcard]').each(function () {
    currEl = $(this);
    currMsg = self.validateCreditCard(currEl);
    if (currMsg) {
      validToken = false;
      self.showNotValidMsg(currEl, currMsg);
    }
  })

  // $(form).find('[data-val-regex-pattern]').each(function () {
  //   var currEl,
  //     currElPattern;

  //   currEl = $(this);
  //   var testMsg = $(this).attr('data-val-regex');
  //   console.log('data-val-regex =',testMsg);
  //   currElPattern = currEl.attr('data-val-regex-pattern');
  //   currMsg = self.validatePattern(currEl, currElPattern);

  //   if (currMsg) {
  //     validToken = false;
  //     self.showNotValidMsg(currEl, currMsg);
  //   }
  // })

  $(form).find('[validate-date]').each(function () {
    currEl = $(this);
    currMsg = self.validateDate(currEl);
    if (currMsg) {
      validToken = false;
      self.showNotValidMsg(currEl, currMsg);
    }
  })

  return validToken;
}


this.validateRequired = function (el) {
  if (self.isElValueEmpty(el)) {
    return self.returnValidateMsg('required')
  } else {
    return false;
  }
}

this.validateNotEmpty = function (el) {
  if (self.isElValueEmpty(el)) {
    return self.returnValidateMsg('not-empty')
  } else {
    return false;
  }
}

this.validateNumber = function (el) {
  var regexp = /^(0|[1-9]\d*)([\,\.]{1}\d+)?$/

  if (self.isElValueEmpty(el)) {
    return false;
  } else {
    if (!regexp.test($(el).val())) {
      return self.returnValidateMsg('number')
    } else {
      return false;
    }
  }
}

this.validatePhone = function (el) {
  var regexp = /^[0-9]{12}$/;

  if (self.isElValueEmpty(el)) {
    return false;
  } else {
    var currTrimTel = $(el).val().replace(/\D+/g, '');
    if (!regexp.test(currTrimTel)) {
      return self.returnValidateMsg('phone')
    } else {
      return false;
    }
  }
}

this.validateEmail = function (el) {
  var regexp = /^(([^<>()\[\]\.,;:\s@\"]+(\.[^<>()\[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i
  if (self.isElValueEmpty(el)) {
    return false;
  } else {
    if (!regexp.test($(el).val())) {
      return self.returnValidateMsg('email')
    } else {
      return false;
    }
  }
}

this.validateEqualTo = function (elVal, equalVal) {
  if (elVal !== equalVal) {
    return self.returnValidateMsg('equal-to');
  } else {
    return false;
  }
}

this.validateLengthMax = function (el, lengthMax) {
  if (el > lengthMax) {
    return self.returnValidateMsg('length-max');
  } else {
    return false;
  }
}

this.validateLengthMin = function (el, lengthMin) {
  if (el < lengthMin && el > 0) {
    return self.returnValidateMsg('length-min');
  } else {
    return false;
  }
}

this.validateRange = function (el, rangeMin, rangeMax) {
  elVal = +el.val();
  elValLength = el.val().length;
  if (elValLength == 0) {
    return false;
  } else if (elVal < rangeMin || elVal > rangeMax) {
    return self.returnValidateMsg('range');
  } else {
    return false;
  }
}

this.validateUrl = function (el) {
  var regexp = /^(?:(?:(?:https?|ftp):)?\/\/)(?:\S+(?::\S*)?@)?(?:(?!(?:10|127)(?:\.\d{1,3}){3})(?!(?:169\.254|192\.168)(?:\.\d{1,3}){2})(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)(?:\.(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)*(?:\.(?:[a-z\u00a1-\uffff]{2,})).?)(?::\d{2,5})?(?:[/?#]\S*)?$/g
  if (self.isElValueEmpty(el)) {
    return false;
  } else {
    if (!regexp.test($(el).val())) {
      return self.returnValidateMsg('url')
    } else {
      return false;
    }
  }
}

this.validateCreditCard = function (el) {
  var regexp = /^(?:4[0-9]{12}(?:[0-9]{3})?|[25][1-7][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})$/g

  if (self.isElValueEmpty(el)) {
    return false;
  } else {
    var currTrimVal = $(el).val().replace(/\D+/g, '');
    if (!regexp.test(currTrimVal)) {
      return self.returnValidateMsg('creditcard')
    } else {
      return false;
    }
  }
}

// this.validatePattern = function (el, regexPattern) {
//   var regexp = new RegExp(regexPattern);
//   var currTrimVal = $(el).val().replace(/\s+/g, '');
//   if (self.isElValueEmpty(el)) {
//     return false;
//   } else {
//     if (!regexp.test(currTrimVal)) {
//       return self.returnValidateMsg('pattern')
//     } else {
//       return false;
//     }
//   }
// }

this.validateDate = function (el) {
  var regexp = /^\s*(3[01]|[12][0-9]|0?[1-9])\.(1[012]|0?[1-9])\.((?:19|20)\d{2})\s*$/g
  if (self.isElValueEmpty(el)) {
    return false;
  } else {
    if (!regexp.test($(el).val())) {
      return self.returnValidateMsg('date')
    } else {
      return false;
    }
  }
}

$(".phone").inputmask({
  alias: "phone",
  mask: "+380(99)999-99-99",
  showMaskOnHover: false,
  removeMaskOnSubmit: false
});